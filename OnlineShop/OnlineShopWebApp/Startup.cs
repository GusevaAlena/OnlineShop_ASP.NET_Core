using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using OnlineShop.Db;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Db.Repositories;
using OnlineShop.Db.Models;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OnlineShopWebApp.Helpers;

namespace OnlineShopWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("online_shop");
            services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer(connection));

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<DatabaseContext>();

            services.ConfigureApplicationCookie(options =>
            {          
                options.ExpireTimeSpan = TimeSpan.FromHours(24);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.Cookie = new CookieBuilder
                {
                    IsEssential = true
                };
            }
            );

            services.AddTransient<IProductRepository, ProductDbRepository>();
            services.AddTransient<ICartRepository, CartDbRepository>();
            services.AddTransient<IFavoriteRepository, FavoriteDbRepository>();
            services.AddTransient<ICompareRepository, CompareDbRepository>();
            services.AddTransient<IOrderRepository, OrderDbRepository>();
            services.AddTransient<IAddressRepository, AddressDbRepository>();
            services.AddTransient<IPaymentMethodRepository, PaymentMethodsDbRepository>();
            services.AddTransient<IStatusRepository, StatusDbRepository>();

            services.AddMemoryCache();
            services.AddHostedService<ProductsCache>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Add("Cache-Control", "public,max-age-600");
                }
            });

            app.UseRouting();
  
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "MyArea",
                   pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
