using System;

namespace UserManagement.Web.Models.Logs;

public class LogListViewModel
{
    public List<LogListItemViewModel> Items { get; set; } = new();
    // Section for Pagination 
    public int CurrentPageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }

    public bool HasPreviousPage => CurrentPageIndex > 1;
    public bool HasNextPage => CurrentPageIndex < TotalPages;
}

public class LogListItemViewModel
{
    public long Id { get; set; }
    public long? UserId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public DateTime TimeStamp { get; set; }
    public string? UserForename { get; set; } = string.Empty;
    public string? UserSurname { get; set; } = string.Empty;
}

