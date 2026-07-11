namespace Naitrust.Domain.Models.Dtos.Common;

public record PaginationRequest(int Page = 1, int PageSize = 20, string? SortBy = null, string? SortDirection = "asc");
