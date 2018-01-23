using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hello
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            #region Example_0

            services.AddHttpClient("example");

            #endregion

            #region Example_1_2

            // The name associates a logical name with a configuration
            services.AddHttpClient<GithubClient>("github", c =>
            {
                c.BaseAddress = new Uri("https://api.github.com/"); // Set a base address so we don't hardcode the hostname

                c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json"); // Github API versioning
                c.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample"); // Github requires a user-agent
            });

            #endregion

            #region Example_3

            // Let's inject some 'outgoing middlware'
            services.AddTransient<TimingHandler>();
            services.AddTransient<CacheHandler>();

            services.AddHttpClient("unreliable", c =>
            {
                c.BaseAddress = new Uri("http://localhost:5000/");

            })
            .AddHttpMessageHandler<TimingHandler>() // This handler is on the 'outside'
            .AddHttpMessageHandler<CacheHandler>(); // This handler is on the 'inside'

            #endregion

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
