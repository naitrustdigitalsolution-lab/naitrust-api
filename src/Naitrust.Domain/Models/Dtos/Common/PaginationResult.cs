namespace Naitrust.Domain.Models.Dtos.Common;

public class PaginationResult<T> where T : class
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public int PageSize { get; set; }
    public IEnumerable<T> Data { get; set; } = null!;
}
