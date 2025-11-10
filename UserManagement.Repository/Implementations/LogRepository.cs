using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Repository.Interfaces;
using UserManagement.Web.Models.Logs;

namespace UserManagement.Repository.Implementations;

public class LogRepository : ILogRepository
{
    private readonly IDataContext _dataContext;

    public LogRepository(IDataContext dataContext) => _dataContext = dataContext;

    public async Task<List<Log>> GetAllAsync()
    {
        return await _dataContext.Logs
            .Include(log => log.User)
            .OrderByDescending(log => log.TimeStamp)
            .ToListAsync();
    }
    
    public async Task<Log?> GetByIdAsync(long id)
    {
        return await _dataContext.Logs
            .FirstOrDefaultAsync(log => log.Id == id);
    }

    public async Task<IEnumerable<Log>> GetByUserIdAsync(long userId)
    {
        return await _dataContext.Logs
            .Where(log => log.UserId == userId)
            .Include(log => log.User)
            .OrderByDescending(log => log.TimeStamp)
            .ToListAsync();
    }

    // https://learn.microsoft.com/en-us/aspnet/core/data/ef-rp/sort-filter-page?view=aspnetcore-9.0#add-paging
    public async Task<IEnumerable<Log>> GetPaginatedLogsAsync(int currentPageIndex, int pageSize)
    {
        return await _dataContext.Logs
            .Include(log => log.User)
            .OrderByDescending(log => log.TimeStamp)
            .Skip((currentPageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalLogCountAsync()
    {
        return await _dataContext.Logs.CountAsync();
    }

    public async Task CreateAsync(Log log)
    {
        await _dataContext.CreateAsync(log);
    }
}
