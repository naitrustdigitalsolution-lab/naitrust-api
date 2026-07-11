using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Api.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

            var response = NaitrustResponse.UnprocessableEntity("Validation failed");
            response.Data = errors;

            context.Result = new ObjectResult(response)
            {
                StatusCode = 422
            };
            return;
        }

        await next();
    }
}
