namespace Naitrust.Domain.Models.Dtos.Common;

public record PaginatedResponse<T>(List<T> Items, int Page, int PageSize, int TotalCount, int TotalPages);
