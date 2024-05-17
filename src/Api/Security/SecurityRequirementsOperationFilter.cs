namespace WebAppStarter.Api.Security;

using DotSwashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

/// <summary>
/// Operation filter to add security requirements to Swagger documentation
/// based on the presence of <see cref="AuthorizeAttribute"/> on controllers and actions.
/// </summary>
public class SecurityRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Retrieve the Authorize attributes from the method
        var methodAttributes = context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>();

        // Retrieve the Authorize attributes from the controller, if available
        var controllerAttributes = context.MethodInfo.DeclaringType?
            .GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>() ?? [];

        // Combine the policies from method and controller attributes
        var requiredScopes = methodAttributes.Concat(controllerAttributes)
            .Select(attr => attr.Policy)
            .Distinct();

        if (requiredScopes.Any())
        {
            operation.Responses.Add("401", new OpenApiResponse
            {
                Description = "Unauthorized",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(ProblemDetails), context.SchemaRepository),
                    },
                },
            });
            operation.Responses.Add("403", new OpenApiResponse
            {
                Description = "Forbidden",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(ProblemDetails), context.SchemaRepository),
                    },
                },
            });

            // Reference the OAuth2 security scheme (defined in the DependencyInjection -> AddSwagger -> AddSecurityDefinition )
            var oAuthScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth", },
            };

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new()
                {
                    [oAuthScheme] = requiredScopes.ToList(),
                },
            };
        }
    }
}