using Company.Fares.BLL.Interfaces;
using Company.Fares.BLL.Repositories;
using Company.Fares.DAL.Data.Contexts;
using Company.Fares.DAL.Models;
using Company.Fares.PL.Helpers;
using Company.Fares.PL.Mapping;
using Company.Fares.PL.Services;
using Company.Fares.PL.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Company.Fares.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;

            }).AddGoogle(o =>
            {
                o.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                o.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            });
            builder.Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = FacebookDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;

            }).AddFacebook(o =>
            {
                o.ClientId = builder.Configuration["Authentication:Facebook:AppId"];
                o.ClientSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
            });


            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); // Allow Dependency injection for DepartmentRepository
            builder.Services.AddScoped<IEmployeeRepository, Employeerepository>(); // Allow Dependency injection for DepartmentRepository
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(typeof(EmployeeProfile));
            builder.Services.AddAutoMapper(typeof(DepartmentProfile));
            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); // instead of writing connection string
            }); // Allow Dependency Injection for CompanyDbContext // which allow CLR to create object from it whenever he wants

            // differ by Life Time
            //builder.Services.AddScoped();     // Create object life time per request - unreachable object
            //builder.Services.AddTransient();  // Create object life time per operation 
            //builder.Services.AddSingleton();  // Create object life time per application

            builder.Services.AddScoped<IScopedService, ScopedService>(); // per request
            builder.Services.AddTransient<ITransentService, TransentService>(); // per operation
            builder.Services.AddSingleton<ISingletonService, SingletonService>(); //per application

            builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<CompanyDbContext>().AddDefaultTokenProviders();
            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
                config.AccessDeniedPath = "/Account/AccessDenied";
            });


            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
            builder.Services.AddScoped<IMailService, MailService>();
            builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection(nameof(TwilioSettings)));
            builder.Services.AddScoped<ITwilioService, TwilioService>();


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


