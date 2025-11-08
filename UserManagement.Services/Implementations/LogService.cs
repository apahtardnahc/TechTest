using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Repository.Interfaces;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Logs;

namespace UserManagement.Services.Domain.Implementations;

public class LogService : ILogService
{
    private readonly ILogRepository _logRepository;

    public LogService(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    /// <summary>
    /// Return all logs
    /// </summary>
    /// 
    public async Task<IEnumerable<Log>> GetAllAsync()
    {
        return await _logRepository.GetAllAsync();
    }

    /// <summary>
    /// Returns a log by its specific id
    /// </summary>
    /// 
    public async Task<Log?> GetByIdAsync(long id)
    {
        return await _logRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Returns a log by its user associated id
    /// </summary>
    /// 
    public async Task<IEnumerable<Log>> GetByUserIdAsync(long userId)
    {
        return await _logRepository.GetByUserIdAsync(userId);
    }

    /// <summary>
    /// Logs an action into the database
    /// </summary>
    /// 
    public async Task CreateAsync(string action, long? userId, string details)
    {
        var log = new Log()
        {
            UserId = userId,
            Action = action,
            Details = details,
            TimeStamp = DateTime.UtcNow,
        };

        await _logRepository.CreateAsync(log);
    }

    /// <summary>
    /// Using for pagination
    /// </summary>
    /// 
    public async Task<PagedResult<Log>> GetPaginatedLogsAsync(int currentPageIndex, int pageSize)
    {
        var logs = await _logRepository.GetPaginatedLogsAsync(currentPageIndex, pageSize);
        var totalCount = await _logRepository.GetTotalLogCountAsync();

        // Preventing rounding error
        var totalPages = (int)Math.Ceiling(((double)totalCount / pageSize));

        return new PagedResult<Log>
        {
            Items = logs,
            CurrentPageIndex = currentPageIndex,
            PageSize = pageSize,
            TotalPages = totalPages,
            TotalCount = totalCount
        };
    }
}
