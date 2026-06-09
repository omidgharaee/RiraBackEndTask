using Microsoft.EntityFrameworkCore;
using RiraBackEndTask.Modules.Persons.Infrastructure.Persistence;

namespace RiraBackEndTask.Api.Extensions;

public static class HostExtensions
{
    public static IHost ApplyAllMigrations(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        var services = scope.ServiceProvider;

        var logger = services.GetRequiredService<
            ILoggerFactory>()
            .CreateLogger("Migration");

        try
        {
            logger.LogInformation(
                "Starting database migrations...");

            var personsDbContext =
                services.GetRequiredService<PersonsDbContext>();

            personsDbContext.Database.Migrate();

            logger.LogInformation(
                "Database migrations completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogCritical(
                ex,
                "An error occurred while applying database migrations.");

            throw;
        }

        return host;
    }
}