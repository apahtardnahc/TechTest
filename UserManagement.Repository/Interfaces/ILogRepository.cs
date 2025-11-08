using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Web.Models.Logs;

namespace UserManagement.Repository.Interfaces;

public interface ILogRepository
{
    //IEnumerable<Log> GetAll();
    //Log? GetById(long id);
    //IEnumerable<Log> GetByUserId(long userId);
    //IEnumerable<Log> GetPaginatedLogs(int currentPageIndex, int pageSize);
    //int GetTotalLogCount();
    //Log Create(Log log);
    Task<List<Log>> GetAllAsync();
    Task<Log?> GetByIdAsync(long id);
    Task<IEnumerable<Log>> GetByUserIdAsync(long userId);
    Task<IEnumerable<Log>> GetPaginatedLogsAsync(int currentPageIndex, int pageSize);
    Task<int> GetTotalLogCountAsync();
    Task CreateAsync(Log log);
}

