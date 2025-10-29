using System.Linq.Expressions;

namespace Core.DTO;

public class QuerySearch
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchQuery { get; set; }
}

public class PaginationParams<T>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string SearchQuery { get; set; }
    public string[] Fields { get; set; }
    public Expression<Func<T, bool>> Filter { get; set; }
    public string OrderByField { get; set; }
    public string Descending { get; set; } = "desc";
    
    public List<string>? IncludeProperties { get; set; }

    public PaginationParams(int page, int pageSize, string searchQuery = null, 
        string[] fields = null, Expression<Func<T, bool>> filter = null,
        string orderByField = null, string descending = "desc", List<string>? includeProperties = null)
    {
        Page = page;
        PageSize = pageSize;
        SearchQuery = searchQuery;
        Fields = fields;
        Filter = filter;
        OrderByField = orderByField;
        Descending = descending;
        IncludeProperties = includeProperties;
    }

}