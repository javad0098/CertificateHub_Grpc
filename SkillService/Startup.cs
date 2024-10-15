using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SkillService.Data;
using SkillService.EventProcessing;
using SkillService.SyncDataServices.Grpc;
using SkillsService.AsyncDataServices;

namespace SkillService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(opt =>
            opt.UseInMemoryDatabase("InMemory"));
            services.AddScoped<ISkillRepo, SkillRepo>();
            services.AddScoped<ICertificateDataClient, CertificateDataClient>();

            services.AddControllers()
                     .AddJsonOptions(options =>
                     {
                         // This ensures that enums are serialized/deserialized as strings
                         options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                     });
            services.AddSingleton<IEventProcessor, EventProcessor>();
            services.AddHostedService<MessageBusSubscriber>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Certificate API",
                    Version = "v1",
                    Description = "API documentation for CertificateService",
                });
            });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Enable Swagger middleware in the development environment
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SkillService API v1");
                    c.RoutePrefix = string.Empty; // Set Swagger UI at the root of the application
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            PrepDb.PrepPopulation(app);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
