using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Web.Models.Logs;

namespace UserManagement.Services.Domain.Interfaces;

public interface ILogService
{
    Task<IEnumerable<Log>> GetAllAsync();
    Task<Log?> GetByIdAsync(long id);
    Task<IEnumerable<Log>> GetByUserIdAsync(long userId);
    //Implement Pagination with a DTO
    // https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/sort-filter-page?view=aspnetcore-9.0
    Task<PagedResult<Log>> GetPaginatedLogsAsync(int currentPageIndex, int pageSize);
    Task CreateAsync(string action, long? userId, string details);
}
