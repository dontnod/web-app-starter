namespace WebAppStarter.Api.Infrastructure;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

public class GlobalProducesResponseTypeConvention : IApplicationModelConvention
{
    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            foreach (var action in controller.Actions)
            {
                // Add global response types
                action.Filters.Add(
                    new ProducesResponseTypeAttribute(
                        typeof(ValidationProblemDetails),
                        StatusCodes.Status400BadRequest
                    )
                );
                action.Filters.Add(
                    new ProducesResponseTypeAttribute(
                        typeof(ProblemDetails),
                        StatusCodes.Status404NotFound
                    )
                );
                action.Filters.Add(
                    new ProducesResponseTypeAttribute(
                        typeof(ProblemDetails),
                        StatusCodes.Status500InternalServerError
                    )
                );
            }
        }
    }
}
