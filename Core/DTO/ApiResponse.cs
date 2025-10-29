using System.Net;

namespace Core.DTO;

public class ApiResponse<T, E>
{
    public T? Data { get; set; }
    public string? Message { get; set; } = String.Empty;
    public int StatusCode { get; set; }
    public E? Errors { get; set; }
}

public class ResponseError
{
    public string? Type { get; set; }
    public string? Description { get; set; }
}

public class PagedResult<T>
{
    public IEnumerable<T> Results { get; set; } = new List<T>();
        
    public string? Next { get; set; }
        
    public string? Previous { get; set; }
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}