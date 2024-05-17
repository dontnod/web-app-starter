namespace Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using WebAppStarter.Application.Common.Interfaces;
using WebAppStarter.Infrastructure.Data;
using WebAppStarter.Infrastructure.Data.Interceptors;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseSqlite(connectionString);
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"));

        services.AddSingleton(TimeProvider.System);

        return services;
    }
}
