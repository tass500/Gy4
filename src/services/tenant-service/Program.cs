using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using TenantService.Data;
using TenantService.Models;

namespace TenantService
{
    // Non-static class for logging
    public class ProgramLogger { }

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();

            // Add DbContext with InMemory database
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TenantServiceDb"));

            // Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            // Configure Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Tenant Service API", 
                    Version = "v1",
                    Description = "API for managing tenants in the logistics system",
                    Contact = new OpenApiContact
                    {
                        Name = "Logistics Team",
                        Email = "support@logistics.example.com"
                    }
                });

                // Include XML comments for API documentation
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tenant Service API V1");
                    c.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root
                });
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthorization();
            app.MapControllers();

            // Seed the in-memory database with test data
            await using (var scope = app.Services.CreateAsyncScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    await SeedDatabase(context, app.Logger);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<ProgramLogger>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            // Health check endpoint
            app.MapGet("/api/health", () => 
                Results.Ok(new { 
                    status = "Healthy", 
                    service = "Tenant Service", 
                    timestamp = DateTime.UtcNow 
                }));

            // Root endpoint redirects to Swagger
            app.MapGet("/", () => Results.Redirect("/swagger"));

            await app.RunAsync();
        }

        private static async Task SeedDatabase(ApplicationDbContext context, ILogger logger)
        {
            // Create the database and apply any pending migrations
            await context.Database.EnsureCreatedAsync();
            
            // Check if we already have any tenants
            if (!await context.Tenants.AnyAsync())
            {
                // If no tenants exist, add some test data
                var tenants = new[]
                {
                    new Tenant
                    {
                        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Name = "Teszt Bérlő 1",
                        Domain = "teszt1.local",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Tenant
                    {
                        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        Name = "Teszt Bérlő 2",
                        Domain = "teszt2.local",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    }
                };
                
                await context.Tenants.AddRangeAsync(tenants);
                await context.SaveChangesAsync();
                logger.LogInformation("Added test data to the database.");
            }
        }
    }
}
