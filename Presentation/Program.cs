using Data;
using Data.Abstract;
using Data.Concrete;
using Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Password.RequireDigit = false; // rakam gerektirmez
    options.Password.RequireUppercase = false; // büyük harf gerektirmez
    options.Password.RequireNonAlphanumeric = false; // özel karakter gerektirmez
    options.Password.RequiredLength = 6; // þifre uzunluðu en az 6 karakter

    options.User.RequireUniqueEmail = true; // kullanýcý e-postalarýnýn benzersiz olmasýný zorunlu kýlar
})
.AddEntityFrameworkStores<AppDbContext>() // Entity Framework Core kullanarak veritabaný iþlemlerini yapar
.AddDefaultTokenProviders(); 

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";       // giriþ yapýlmamýþsa buraya yönlendir
    options.AccessDeniedPath = "/Account/AccessDenied"; // yetkisiz eriþimde buraya yönlendir
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);  // cookie süresi
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area}/{controller=Default}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
