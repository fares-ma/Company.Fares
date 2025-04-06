using Company.Fares.BLL.Interfaces;
using Company.Fares.BLL.Repositories;
using Company.Fares.DAL.Data.Contexts;
using Company.Fares.DAL.Models;
using Company.Fares.PL.Helpers;
using Company.Fares.PL.Mapping;
using Company.Fares.PL.Services;
using Company.Fares.PL.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Company.Fares.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews(); // Register Bulit in MVC Services
            //builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); // Allow  DI For DepartmentRepository
            //builder.Services.AddScoped<IEmployeeRepository, Employeerepository>(); // Allow  DI For EmployeeRepository
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // Allow  DI For UnitOfWork

            builder.Services.AddAutoMapper(typeof(EmployeeProfile));


            builder.Services.AddIdentity<AppUser, IdentityRole>()
               .AddEntityFrameworkStores<CompanyDbContext>()
               .AddDefaultTokenProviders();

            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));

            builder.Services.AddScoped<IMailService, MailService>();

            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            }); // Allow  DI For CompanyDbContext


            builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile())); // Allow  DI For AutoMapper


            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
               
            }); 

            // Life Time
            //builder.Services.AddScoped();    // Create One Object Per Request - UnReachable Object
            //builder.Services.AddTransient(); // Create One Object Per Operation - Reachable Object
            //builder.Services.AddSingleton(); // Create One Object Per Application - Reachable Object

            builder.Services.AddScoped<IScopedService, ScopedService>(); // Per Request
            builder.Services.AddTransient<ITransentService, TransentService>(); // Per Operation
            builder.Services.AddSingleton<ISingletonService, SingletonService>(); // Per Application

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}


