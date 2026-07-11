using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Api.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IWebHostEnvironment _hostEnvironment;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IWebHostEnvironment hostEnvironment)
    {
        _logger = logger;
        _hostEnvironment = hostEnvironment;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var code = HttpStatusCode.InternalServerError;

        switch (exception)
        {
            case UnauthorizedAccessException:
                code = HttpStatusCode.Unauthorized;
                break;
            case KeyNotFoundException:
                code = HttpStatusCode.NotFound;
                break;
            case ArgumentException:
                code = HttpStatusCode.BadRequest;
                break;
            case InvalidOperationException:
                code = HttpStatusCode.Conflict;
                break;
        }

        _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

        var response = NaitrustResponse.InternalServerError("Operation failed, please try again", code);

        if (_hostEnvironment.IsDevelopment())
            response.Message = $"{exception.Message}\n{exception.InnerException}";
        else
            response.Message = "Operation failed, please try again";

        httpContext.Response.StatusCode = (int)code;
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken: cancellationToken);
        return true;
    }
}
