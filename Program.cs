using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System;
using Core.Flash;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // Configure the database context (MySQL).
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(8, 0, 21))));

        // Configure authentication services
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/User/Login"; // Adjust login path
                options.LogoutPath = "/User/Logout"; // Adjust logout path
                options.ExpireTimeSpan = TimeSpan.FromMinutes(1);  // Expire cookie after 1 minute
                options.SlidingExpiration = false;  // Disable sliding expiration
            });
        builder.Services.AddAutoMapper(typeof(Program));
        // Build the application.
        builder.Logging.AddConsole();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        // Enable authentication and authorization middleware
        app.UseAuthentication();
        app.UseAuthorization();

        // Define default route
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        // Run the application.
        app.Run();
        CarRentalStatistics statistics = new CarRentalStatistics();
        statistics.GetTotalRentedCarsByBrand();

    }
    
       
    
}
