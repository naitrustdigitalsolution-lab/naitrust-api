using System.Net;

namespace Naitrust.Domain.Models.Dtos.Common;

public class NaitrustResponse<T>
{
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public string? Message { get; set; }
    public T? Data { get; set; }
    public bool IsSuccessful { get; private set; }

    private NaitrustResponse(HttpStatusCode statusCode, string message, T? data, bool isSuccessful)
    {
        StatusCode = statusCode;
        Message = message;
        Data = data;
        IsSuccessful = isSuccessful;
    }

    public static NaitrustResponse<T> Success(string message = "Request successful", T? data = default)
        => new(HttpStatusCode.OK, message, data, true);

    public static NaitrustResponse<T> Created(string message = "Resource created successfully", T? data = default)
        => new(HttpStatusCode.Created, message, data, true);

    public static NaitrustResponse<T> BadRequest(string message = "Bad request")
        => new(HttpStatusCode.BadRequest, message, default, false);

    public static NaitrustResponse<T> Unauthorized(string message = "Unauthorized")
        => new(HttpStatusCode.Unauthorized, message, default, false);

    public static NaitrustResponse<T> Forbidden(string message = "Forbidden")
        => new(HttpStatusCode.Forbidden, message, default, false);

    public static NaitrustResponse<T> NotFound(string message = "Resource not found")
        => new(HttpStatusCode.NotFound, message, default, false);

    public static NaitrustResponse<T> Conflict(string message = "Conflict detected")
        => new(HttpStatusCode.Conflict, message, default, false);

    public static NaitrustResponse<T> InternalServerError(string message = "Internal server error")
        => new(HttpStatusCode.InternalServerError, message, default, false);

    public static NaitrustResponse<T> Failure(HttpStatusCode statusCode, string message)
        => new(statusCode, message, default, false);
}

public class NaitrustResponse
{
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public string? Message { get; set; }
    public object? Data { get; set; }
    public bool IsSuccessful { get; private set; }

    public NaitrustResponse() { }

    public NaitrustResponse(HttpStatusCode statusCode, string message, object? data, bool isSuccessful)
    {
        StatusCode = statusCode;
        Message = message;
        Data = data;
        IsSuccessful = isSuccessful;
    }

    // SUCCESS RESPONSES (2xx)
    public static NaitrustResponse Success(HttpStatusCode statusCode = HttpStatusCode.OK, string message = "Request successful", object? data = null)
        => new(statusCode, message, data, true);

    public static NaitrustResponse Created(HttpStatusCode statusCode = HttpStatusCode.Created, string message = "Resource created successfully", object? data = null)
        => new(statusCode, message, data, true);

    public static NaitrustResponse NoContent(HttpStatusCode statusCode = HttpStatusCode.NoContent, string message = "Request successful, no content returned")
        => new(statusCode, message, null, true);

    // CLIENT ERROR RESPONSES (4xx)
    public static NaitrustResponse BadRequest(string message = "Bad request", HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => new(statusCode, message, null, false);

    public static NaitrustResponse Unauthorized(string message = "Unauthorized", HttpStatusCode statusCode = HttpStatusCode.Unauthorized)
        => new(statusCode, message, null, false);

    public static NaitrustResponse Forbidden(string message = "Forbidden", HttpStatusCode statusCode = HttpStatusCode.Forbidden)
        => new(statusCode, message, null, false);

    public static NaitrustResponse NotFound(string message = "Resource not found", HttpStatusCode statusCode = HttpStatusCode.NotFound)
        => new(statusCode, message, null, false);

    public static NaitrustResponse Conflict(string message = "Conflict", HttpStatusCode statusCode = HttpStatusCode.Conflict)
        => new(statusCode, message, null, false);

    public static NaitrustResponse Gone(string message = "Resource no longer available", HttpStatusCode statusCode = HttpStatusCode.Gone)
        => new(statusCode, message, null, false);

    public static NaitrustResponse TooManyRequests(string message = "Too many requests", HttpStatusCode statusCode = HttpStatusCode.TooManyRequests)
        => new(statusCode, message, null, false);

    public static NaitrustResponse UnprocessableEntity(string message = "Unprocessable entity", HttpStatusCode statusCode = HttpStatusCode.UnprocessableEntity)
        => new(statusCode, message, null, false);

    // SERVER ERROR RESPONSES (5xx)
    public static NaitrustResponse InternalServerError(string message = "Internal server error", HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        => new(statusCode, message, null, false);

    public static NaitrustResponse ServiceUnavailable(string message = "Service unavailable", HttpStatusCode statusCode = HttpStatusCode.ServiceUnavailable)
        => new(statusCode, message, null, false);

    // CUSTOM RESPONSE
    public static NaitrustResponse Custom(HttpStatusCode statusCode, bool isSuccessful, string message, object? data = null)
        => new(statusCode, message, data, isSuccessful);
}
