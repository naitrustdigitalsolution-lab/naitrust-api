namespace Naitrust.Domain.Models.Dtos.Common;

public record ErrorDetail(string Code, string Message, object? Details = null);
