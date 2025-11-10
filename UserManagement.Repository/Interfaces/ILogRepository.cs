using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Web.Models.Logs;

namespace UserManagement.Repository.Interfaces;

public interface ILogRepository
{
    Task<List<Log>> GetAllAsync();
    Task<Log?> GetByIdAsync(long id);
    Task<IEnumerable<Log>> GetByUserIdAsync(long userId);
    Task<IEnumerable<Log>> GetPaginatedLogsAsync(int currentPageIndex, int pageSize);
    Task<int> GetTotalLogCountAsync();
    Task CreateAsync(Log log);
}

