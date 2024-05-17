namespace WebAppStarter.Infrastructure.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class ApplicationDbContextInitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
#if USE_SQL_LITE
        await initialiser.InitialiseAsync();
#endif

        await initialiser.SeedAsync();
    }
}
