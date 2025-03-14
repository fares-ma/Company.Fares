using Company.Fares.BLL.Interfaces;
using Company.Fares.BLL.Repositories;
using Company.Fares.DAL.Data.Contexts;
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
           
            builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository>(); // Allow  DI For DepartmentRepository
            
            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            }); // Allow  DI For CompanyDbContext
          
            
            
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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
