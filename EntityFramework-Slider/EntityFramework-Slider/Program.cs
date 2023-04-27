using EntityFramework_Slider.Data;
using EntityFramework_Slider.Services;
using EntityFramework_Slider.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Sesion istifade edecymizi bildiririk
builder.Services.AddSession();


builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

 
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //bu servislerde Coockideki datalara catmaq ucundu(istifade edeceymizi burda bildirik)
builder.Services.AddScoped<ILayoutService, LayoutService>(); //istifade edeceymiz servisin adin bildirik  AddScopped-birdefe request atir onu istifade edir ama yeni insatnsda tezesin yaradir,ama varsa(request) kohnesin istifade edir
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<IExpertService, ExpertService>();
builder.Services.AddScoped<IFooterService, FooterService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();   //Sesion istifade edecymizi bildiririk
app.UseRouting();

app.UseAuthorization();


//for-adminPanel
app.MapControllerRoute(
     name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
