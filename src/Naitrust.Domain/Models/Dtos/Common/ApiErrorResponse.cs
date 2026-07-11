namespace Naitrust.Domain.Models.Dtos.Common;

public record ApiErrorResponse(bool Success, ErrorDetail Error);
