using Naitrust.Domain.Models.Dtos.Common;
using Microsoft.EntityFrameworkCore;

namespace Naitrust.Application.Extensions;

public static class QueryableExtensions
{
    public static async Task<PaginatedResponse<T>> ToPaginatedResponseAsync<T>(
        this IQueryable<T> query, int page, int pageSize)
    {
        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResponse<T>(items, page, pageSize, totalCount, totalPages);
    }
}
