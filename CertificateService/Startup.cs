using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using CertificateService.Data;
using CertificateService.SyncDataServices.Http;
using CertificateService.AsyncDataServices;
using CertificateService.SyncDataServices.Grpc;

namespace CertificateService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private readonly bool _isEfMigration;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
            _isEfMigration = AppDomain.CurrentDomain.FriendlyName.Equals("ef", StringComparison.OrdinalIgnoreCase);
        }

        public void ConfigureServices(IServiceCollection services)
        {


            if (_env.IsProduction() || _isEfMigration)
            {
                string connectionString = _configuration.GetConnectionString("CertificateConn");

                try
                {
                    Console.WriteLine("--> using sql light db");

                    services.AddDbContext<AppDBContext>(opt =>
                    opt.UseSqlServer(connectionString));
                    Console.WriteLine("app db set");

                }
                catch (Exception ex)
                {
                    throw new Exception($"the connection string has not be able to set {ex.Message}", ex);
                }

            }
            else
            {
                Console.WriteLine("--> using InMemory db");
                services.AddDbContext<AppDBContext>(opt =>
                               opt.UseInMemoryDatabase("InMemory"));
            }
            services.AddHttpClient<ISkillDataClient, HttpSkillDataClient>();

            services.AddScoped<ICertificateRepo, CertificateRepo>();
            services.AddSingleton<IMessageBusClient, MessageBusClient>();

            services.AddControllers(); // Register MVC controllers
            services.AddGrpc();

            // Add Swagger services
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Enable Swagger middleware in the development environment
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Certificate Service API v1");
                    c.RoutePrefix = string.Empty; 
            }

            app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS

            app.UseRouting();

            app.UseAuthorization();

            PrepDB.PrepPopulation(app, env.IsProduction());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Map controller routes
                endpoints.MapGrpcService<GrpcCertificateService>();

                endpoints.MapGet("/protos/certificates.proto", async context =>
                {
                    await context.Response.WriteAsync(File.ReadAllText("Protos/certificates.proto"));
                });
            });

        }
    }
}
