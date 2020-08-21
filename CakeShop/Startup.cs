using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CakeShop.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CakeShop
{
    public class Startup
    {
        // (A) 
        // With the -> IConfiguration <- we have access to the properties which are set in  appsettings.json
        // Currently I will need it to have access to the connection string in appsettings.json
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // (B)
            // Get the connection string. after having added above 
            //|ConfigureServices| the -->  public IConfiguration Configuration { get; }

            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Identity
            services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDBContext>();


            // added scopes (Dependency Injection)
            services.AddScoped<IPieRepository, PieRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            // enabling and configuring the Session \\
            services.AddScoped<ShoppingCart>(sp => ShoppingCart.GetCart(sp)); //invoke for the user 
                                                                              //the GetCart method on the ShoppingCart Class
                                                            //sp -> is the IService Provider
                                                            //with AddScoped: if a shopping Cart exists it will be associated to the user
                                                            //elsewere a new Shopping cart will be created and be associated to the user
            services.AddHttpContextAccessor();
            services.AddSession();
                            //----\\


            // (1) added AddControllersWithViews
            services.AddControllersWithViews();

            // added as required, due to Identity 
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // (1) added for making Http request to Https 
            app.UseHttpsRedirection();

            // (2) added to serve static files e.g. images, JavaScript files, CSS etc.
            //it searches wwwroot for static files
            app.UseStaticFiles();

            // session middleware
            app.UseSession();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

           

            app.UseEndpoints(endpoints =>
            {
                // (3) added default routing

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages(); // due to identity needs
            });
        }
    }
}
