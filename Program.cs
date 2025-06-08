using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkshopManager.Data;
using WorkshopManager.Models;
using WorkshopManager.Services;
using WorkshopManager.Mappers;

namespace WorkshopManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Add services to the container.
            builder.Services.AddDbContext<WorkshopDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IClientService, ClientService>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<IClientMapper, ClientMapper>();
            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<WorkshopDbContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
