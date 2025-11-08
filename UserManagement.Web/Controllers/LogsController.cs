using System.Linq;
using System.Threading.Tasks;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Logs;

namespace UserManagement.Web.Controllers;
[Route("logs")]
public class LogsController : Controller
{
    private readonly ILogService _logService;

    public LogsController(ILogService logService) => _logService = logService;

    [HttpGet]
    public async Task<ViewResult> List(int currentPageIndex = 1, int pageSize = 10)
    {
        var pagedLogs = await _logService.GetPaginatedLogsAsync(currentPageIndex, pageSize);

        var model = new LogListViewModel
        {
            Items = pagedLogs.Items.Select(MapToListItem).ToList(),
            CurrentPageIndex = pagedLogs.CurrentPageIndex,
            PageSize = pagedLogs.PageSize,
            TotalPages = pagedLogs.TotalPages,
            TotalCount = pagedLogs.TotalCount,
        };

        return View(model);
    }

    [HttpGet("detail/{id}")]
    public async Task<IActionResult> Detail(long id)
    {
        var log = await _logService.GetByIdAsync(id);

        if (log == null)
        {
            return NotFound();
        }

        var model = new LogViewModel()
        {
            Id = log.Id,
            UserId = log.UserId,
            Action = log.Action,
            Details = log.Details,
            TimeStamp = log.TimeStamp,
            UserForename = log.User?.Forename,
            UserSurname = log.User?.Surname
        };

        return View(model);
    }

    [HttpGet("user/{userId}")]
    public async Task<ViewResult> UserLogs(long userId)
    {
        var logs = await _logService.GetByUserIdAsync(userId);

        var model = new LogListViewModel
        {
            Items = logs.Select(MapToListItem).ToList()
        };

        return View("List", model);
    }

    #region Helper functions

    private LogListItemViewModel MapToListItem(Log log)
    {
        return new LogListItemViewModel
        {
            Id = log.Id,
            UserId = log.UserId,
            Action = log.Action,
            Details = log.Details,
            TimeStamp = log.TimeStamp,
            UserForename = log.User?.Forename,
            UserSurname = log.User?.Surname
        };
    }

    #endregion
}
