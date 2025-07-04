﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkshopManager.Data;
using WorkshopManager.Models;
using WorkshopManager.Services;
using WorkshopManager.Mappers;

namespace WorkshopManager
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Add services to the container.
            builder.Services.AddDbContext<WorkshopDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IClientService, ClientService>();
            builder.Services.AddScoped<IClientMapper, ClientMapper>();
            builder.Services.AddScoped<IVehicleService, VehicleService>();
            builder.Services.AddScoped<IVehicleMapper, VehicleMapper>();
            builder.Services.AddScoped<IServiceOrderService, ServiceOrderService>();
            builder.Services.AddScoped<IServiceOrderMapper, ServiceOrderMapper>();
            builder.Services.AddScoped<IServiceTaskService, ServiceTaskService>();
            builder.Services.AddScoped<IServiceTaskMapper, ServiceTaskMapper>();
            builder.Services.AddScoped<IPartService, PartService>();
            builder.Services.AddScoped<IPartMapper, PartMapper>();
            builder.Services.AddScoped<IPartCatalogItemService, PartCatalogItemService>();
            builder.Services.AddScoped<IPartCatalogItemMapper, PartCatalogItemMapper>();
            builder.Services.AddScoped<IOrderCommentService, OrderCommentService>();
            builder.Services.AddScoped<IOrderCommentMapper, OrderCommentMapper>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<WorkshopDbContext>().AddDefaultTokenProviders();
            builder.Services.AddRazorPages();
            builder.Services.AddHostedService<OpenOrderReportBackgroundService>();
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                await SeedRolesAsync(scope.ServiceProvider);
            }

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
            app.MapRazorPages();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "Admin", "Mechanic", "Receptionist", "Client" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
