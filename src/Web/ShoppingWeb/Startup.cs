using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ShoppingWeb.ApiContainer;
using ShoppingWeb.ApiContainer.Interfaces;
using ShoppingWeb.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingWeb
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
            services.Configure<ApiSetting>(Configuration.GetSection(nameof(ApiSetting)));
            services.AddSingleton<IApiSettings>(s=>s.GetRequiredService<IOptions<ApiSetting>>().Value);
            services.AddRazorPages();

            services.AddHttpClient();

            services.AddTransient<IOrderApi, OrderApi>();
          //  services.AddTransient<IBasketApi, BasketApi>();
           // services.AddTransient<ICatalogApi, CatalogApi>();

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
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
