namespace Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using WebAppStarter.Api.Infrastructure;
using WebAppStarter.Api.Models;
using WebAppStarter.Api.Security;
using WebAppStarter.Api.Services;
using WebAppStarter.Application.Common.Interfaces;
using WebAppStarter.Infrastructure.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ClaimSettings>(
            configuration.GetSection("AzureAd:ClaimSettings"));

        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });

        services.AddControllers(options =>
        {
            options.Conventions.Add(new GlobalProducesResponseTypeConvention());
        });

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<ICurrentUser, CurrentUser>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddExceptionHandler<CustomExceptionHandler>();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAppStarter API", Version = "v1" });

            c.OperationFilter<SecurityRequirementsOperationFilter>();
            c.EnableAnnotations();
            c.DescribeAllParametersInCamelCase();

            c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme.",
            });
        });

        // Allowing CORS for all domains and HTTP methods for the purpose of the sample
        // In production, modify this with the actual domains and HTTP methods you want to allow
        services.AddCors(o => o.AddPolicy("development", builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        }));

        return services;
    }
}
