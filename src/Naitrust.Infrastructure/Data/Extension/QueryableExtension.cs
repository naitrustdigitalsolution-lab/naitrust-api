using Microsoft.EntityFrameworkCore;

namespace Naitrust.Infrastructure.Data.Extension;

public static class QueryableExtension
{
    public static Paginated<T> Paginate<T>(this IQueryable<T> query, int page = 1, int pageSize = 15)
    {
        IQueryable<T> data = query.Skip((page - 1) * pageSize).Take(pageSize);
        int totalRecords = query.Count();
        return new Paginated<T>(data, totalRecords, page, pageSize);
    }

    public static Paginated<T> Paginate<T>(this IEnumerable<T> query, int page = 1, int pageSize = 15)
    {
        IEnumerable<T> data = query.Skip((page - 1) * pageSize).Take(pageSize);
        int totalRecords = query.Count();
        return new Paginated<T>(data, totalRecords, page, pageSize);
    }

    public static async Task<Paginated<T>> PaginateAsync<T>(this IQueryable<T> query, int page = 1, int pageSize = 15)
    {
        var data = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        int totalRecords = await query.CountAsync();

        return new Paginated<T>(data, totalRecords, page, pageSize);
    }
}

public class Paginated<T>
{
    public IEnumerable<T> Data { get; set; }
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }

    public Paginated(IQueryable<T> data, int totalRecords, int page, int pageSize)
    {
        Data = data;
        TotalCount = totalRecords;
        Page = page;
        TotalPages = (int)Math.Ceiling((double)totalRecords / (double)pageSize);
        HasNextPage = Page < TotalPages;
        HasPreviousPage = Page > 1;
    }

    public Paginated(IEnumerable<T> data, int totalRecords, int page, int pageSize)
    {
        Data = data;
        TotalCount = totalRecords;
        Page = page;
        TotalPages = (int)Math.Ceiling((double)totalRecords / (double)pageSize);
        HasNextPage = Page < TotalPages;
        HasPreviousPage = Page > 1;
    }
}
