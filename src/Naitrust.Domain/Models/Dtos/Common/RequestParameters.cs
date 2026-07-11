namespace Naitrust.Domain.Models.Dtos.Common;

public class RequestParameters
{
    const int maxPageSize = 50;
    private string _keyword = string.Empty;

    public string Keywords
    {
        get { return _keyword; }
        set { _keyword = string.IsNullOrWhiteSpace(value) ? _keyword : value; }
    }

    private int _pageSize = 10;

    public int PageSize
    {
        get { return _pageSize; }
        set { _pageSize = value > maxPageSize ? maxPageSize : value; }
    }

    public int PageNumber { get; set; } = 1;
}
