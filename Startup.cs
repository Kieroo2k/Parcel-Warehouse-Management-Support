using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParcelWarehouseManagementSupport.Data;
using Microsoft.AspNetCore.Identity;
using ParcelWarehouseManagementSupport.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;

namespace ParcelWarehouseManagementSupport
{
    [Authorize]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddDbContext<ParcelWarehouseDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ParcelWarehouseDBContext")));

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<ApplicationUser> userManager)
        {
            CreateRoles(userManager).Wait();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
        private async Task CreateRoles(UserManager<ApplicationUser> userManager)
        {

            var UserManager = userManager;


            ApplicationUser user = await UserManager.FindByEmailAsync("kulikbart@gmail.com");
            UserManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
        }
    }
}
