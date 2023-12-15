using AspNetCoreHero.ToastNotification;
using Do_an_CCNPMM.Areas.Admin.Controllers;
using Do_an_CCNPMM.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Google;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Http;

namespace Do_an_CCNPMM
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
            var stringconn = Configuration.GetConnectionString("data");
            services.AddDbContext<WEBBANGIAYContext>(options => options.UseSqlServer(stringconn));

            services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddNotyf(config =>
                {
                    config.DurationInSeconds = 3;
                    config.IsDismissable = true;
                    config.Position = NotyfPosition.TopRight;
                }
            );
            services.AddRazorPages();
            services.AddSession();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddGoogle(googleOptions =>
                {
                    IConfigurationSection googleAuthNSention = Configuration.GetSection("Authentication:Google");
                    googleOptions.ClientId = googleAuthNSention["ClientId"];
                    googleOptions.ClientSecret = googleAuthNSention["ClientSecret"];
                }
                )
             
                .AddCookie("AdminLogin",p =>
                {
                    p.Cookie.Name = "AdminLoginCookie";
                    p.ExpireTimeSpan = TimeSpan.FromDays(1);
                    p.LoginPath = $"/Admin/login";
                    p.LogoutPath = $"/Admin/logout";
                    p.AccessDeniedPath = "/not-found.html";
                })

                .AddCookie("ShipperLogin",p =>
                {
                    p.Cookie.Name = "ShipperLoginCookie";
                    p.ExpireTimeSpan = TimeSpan.FromDays(1);
                    p.LoginPath = $"/Shipper/login";
                    p.LogoutPath = $"/Shipper/logout";
                    p.AccessDeniedPath = "/not-found.html";
                })

                .AddCookie("UserLogin", p =>
                    {
                        p.Cookie.Name = "UserLoginCookie";
                        p.ExpireTimeSpan = TimeSpan.FromDays(1);
                        p.LoginPath = $"/User/login";
                        p.LogoutPath = $"/User/logout";
                        p.AccessDeniedPath = "/not-found.html";
                }

            );


            services.AddMvc(options => options.EnableEndpointRouting = false);

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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseSession();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Loadsanpham}/{id?}"
                );
                endpoints.MapRazorPages();

            });

            app.UseNotyf();
        }
    }
}
