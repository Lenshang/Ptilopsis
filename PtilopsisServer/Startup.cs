using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using PtilopsisServer.Middleware;

namespace PtilopsisServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<ApiControlMiddleware>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseMiddleware<ApiControlMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/api", async context =>
                {
                    await context.Response.WriteAsync("Ptilopsis (insider test)");
                });
                endpoints.MapControllers();
            });

            if (env.IsDevelopment())
            {
                app.UseStaticFiles(); // For the wwwroot folder
                var workDir = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "ptilopsis-web", "build");
                app.UseFileServer(new FileServerOptions
                {
                    FileProvider = new PhysicalFileProvider(
                        Path.Combine(Environment.CurrentDirectory)),
                    RequestPath = "/view",
                    EnableDirectoryBrowsing = true
                });
            }
        }
    }
}
