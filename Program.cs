using CUAHANG_TAPHOA.Models;
using CUAHANG_TAPHOA.Reponsitory;
using Gocery.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});//L?u ý ? Addsesstion trên Build

// trên buil
builder.Services.AddIdentity<AppUserModel, IdentityRole>()
	.AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();


builder.Services.Configure<IdentityOptions>(options =>
{
	// Password settings.
	options.Password.RequireDigit = true; // ki?u s?
	options.Password.RequireLowercase = true; // ch? th??ng
	options.Password.RequireNonAlphanumeric = false; // ký t? ??c bi?t
	options.Password.RequireUppercase = false; // ch? hoa
	options.Password.RequiredLength = 6; // chi?u dài ký t?
	options.Password.RequiredUniqueChars = 1; // yêu c?u ký t? ??c bi?t và duy nh?t
	
	options.User.RequireUniqueEmail = true;
}); 

var app = builder.Build();

//?K thêm ?? làm load trang http khi sai
app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");

app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
} 

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization(); // xác th?c xem có quy?n gì vs: quy?n admin , use




//map back
app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Product}/{action=Index}/{id?}");

// này c?u hình thêm nh?ng  n?u mu?n thêm thì ph?i ?? trên cái m?c ??nh
app.MapControllerRoute(
    name: "category",
    pattern: "/category/{Slug?}",
    defaults: new { controller = "Category", action = "Index" });

app.MapControllerRoute(
    name: "brand",
    pattern: "/brand/{Slug?}",
    defaults: new { controller = "Brand", action = "Index" });

//map c?a font
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//Seeding Data c?u hình thêm khi t?o s?n ph?m trong seed
var context = app.Services.CreateAsyncScope().ServiceProvider.GetRequiredService<DataContext>();
SeedData.SeedingData(context);

app.Run();
