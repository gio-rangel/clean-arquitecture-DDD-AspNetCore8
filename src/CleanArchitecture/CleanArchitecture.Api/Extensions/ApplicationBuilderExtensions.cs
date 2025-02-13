using CleanArchitecture.Api.Middleware;
using CleanArchitecture.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async void ApplyMigrationAsync (this IApplicationBuilder app)
    {
        using(var scope = app.ApplicationServices.CreateScope())
        {
            var service = scope.ServiceProvider;

            var loggerFactory = service.GetRequiredService<ILoggerFactory>();

            try 
            {
                var context = service.GetRequiredService<ApplicationDbContext>(); 

                await context.Database.MigrateAsync();

            } catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "Migration failed.");
            }
        }
    }

    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}