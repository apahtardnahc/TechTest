using System.Collections.Generic;

namespace UserManagement.Services.Domain;
//https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/sort-filter-page?view=aspnetcore-9.0
public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public int CurrentPageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }

    public bool HasPreviousPage => CurrentPageIndex > 1;
    public bool HasNextPage => CurrentPageIndex < TotalPages;
}
