using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Naitrust.Api.Filters;

public class AuditLogFilter : IAsyncActionFilter
{
    private readonly ILogger<AuditLogFilter> _logger;

    public AuditLogFilter(ILogger<AuditLogFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
        var action = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
        var correlationId = context.HttpContext.Items["X-Correlation-Id"]?.ToString() ?? "N/A";

        _logger.LogInformation(
            "Action: {Action} | User: {UserId} | CorrelationId: {CorrelationId}",
            action, userId, correlationId);

        var result = await next();

        if (result.Exception != null)
        {
            _logger.LogWarning(
                "Action failed: {Action} | User: {UserId} | Error: {Error}",
                action, userId, result.Exception.Message);
        }
    }
}
