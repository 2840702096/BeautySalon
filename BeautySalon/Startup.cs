using BeautySalon.Models.Context;
using BeautySalon.Models.Services;
using BeautySalon.Models.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySalon
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
            services.AddControllersWithViews();
            services.AddSession();
            services.AddHttpContextAccessor();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(options =>
            {
                options.LoginPath = "/Home/Index";
                options.LogoutPath = "/Home/Index";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);

            });

            #region Context

            services.AddDbContext<SalonContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SalonConnectionString"));
            });

            #endregion

            #region IOC

            services.AddTransient<IHelpingServices, HelpingServices>();

            #endregion

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
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "Areas",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Home}/{action=index}/{id?}");

                endpoints.MapAreaControllerRoute(
                    name: "Areas",
                    areaName: "SubAdmin",
                    pattern: "SubAdmin/{controller=Home}/{action=index}/{id?}");

                endpoints.MapAreaControllerRoute(
                   name: "Areas",
                   areaName: "Profile",
                   pattern: "Profile/{controller=Home}/{action=index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
