using MacCrawler.Implementation;
using MacCrawler.Interface;
using MacCrawler.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MacCrawler
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
            services.AddDbContext<MacCrawlerContext>();
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin", builder =>
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod());
            });
            services.AddScoped<IKnownManufactures, IMKnownManufacures>();
            services.AddScoped<IKnownModel, IMKnownModel>();
            services.AddScoped<IKnownReferenceNumbers, IMKnownReferenceNumber>();
            services.AddScoped<ISheet1, ServicesSheet1>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("AllowAnyOrigin");
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
