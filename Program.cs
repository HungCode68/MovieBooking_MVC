using Microsoft.EntityFrameworkCore;
using MovieBookingMVC.Controllers;
using MovieBookingMVC.Data;
using MovieBookingMVC.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddSession();
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<AccountController>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7279/");
});
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ISeatService, SeatService>();
builder.Services.AddScoped<IShowtimeService, ShowtimeService>();
builder.Services.AddAuthentication("Cookies")
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Đường dẫn đến trang đăng nhập
        options.AccessDeniedPath = "/Account/AccessDenied"; // Đường dẫn khi không có quyền truy cập
    });

builder.Services.AddAuthorization();

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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Showtime}/{action=Index}/{id?}");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
