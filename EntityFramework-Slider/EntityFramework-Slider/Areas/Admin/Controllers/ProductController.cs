using EntityFramework_Slider.Areas.Admin.ViewModels;
using EntityFramework_Slider.Heplers;
using EntityFramework_Slider.Models;
using EntityFramework_Slider.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace EntityFramework_Slider.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Index(int page = 1,int take = 4) //page-hansi seyfededi hecne gondermesek Default 1-ci seyfeni goturur ama gelirse hemin reqemin seyfesin goturur,take-necedene gotursun data
        {
            List<Product> products = await _productService.GetPaginatedDatas(page,take);  //databzadaki butun produktlar

            List<ProductListVM> mappedDatas = GetMappedDatas(products);  //VM -nen gonderik datanin lazimsiz propertileri getmesin deye           

            int pageCount = await GetPageCountAsync(take); //method seyfedeki pagination sayini tapmaq ucun 

            Paginate<ProductListVM> paginaDatas = new(mappedDatas, page, pageCount);//paginate- gonderirik secilmis propertili datalari=datas,
                                                                                    //page=hansi seyfedeyik,
                                                                                    //pagecountu=paginatlari siralamaq ucun fora salib
          
            return View(paginaDatas);

        }

        private List<ProductListVM> GetMappedDatas(List<Product> products)
        {
            List<ProductListVM> mappedDatas = new(); //bir clasi istifade etmek ucun instans almaq lazimdi
            foreach (var product in products)
            {
                ProductListVM productVM = new()   //her VM bir product(VM propertisini beraber edirik productun bize lazim olan propertilerine)
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Count = product.Count,
                    CategoryName = product.Category.Name,
                    MainImage = product.Images.Where(m => m.IsMain).FirstOrDefault()?.Image

                };
                mappedDatas.Add(productVM); //sonra VM-leri yani her producti yiqiriq Liste
            }
            return mappedDatas;
        }  //method mapped-yani beraberlesdirilmis-VM modele beraberlesdiririk databazadaki productun propertilerini.
                                                                                //bu method bize lazimdiki butun propertileri gondermemek ucun viewa ancaq lazim olanlari!


        private async Task<int> GetPageCountAsync(int take)  //method seyfedeki pagination sayini tapmaq ucun 
        {
            var productCount = await _productService.GetCountAsync();  //productlari cem sayi

            return (int) Math.Ceiling((decimal)productCount / take);  //productlari cem sayi boluruk gotureceymiz product sayina(her seyfede nece olsun product sayina)
                                                                        //Math.Ceeling-istifade edirik yuvarlasdirmaq. meselen(product 5 / page 2)-bize 3 versin deye
        }                                                               //math ceeling methodu bizden decimal teleb edir deye kast edirik decimala
                                                                        //sonra return etdiyimizi yeniden kast edirik int-e return type int-di deye.
    }
}
