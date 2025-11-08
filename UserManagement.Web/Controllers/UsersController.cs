using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly ILogService _logService;

    public UsersController(IUserService userService, ILogService logService)
    {
        _userService = userService;
        _logService = logService;
    } 

    // Add route to this maybe list?
    [HttpGet]
    public async Task<ViewResult> List()
    {
        var items = (await _userService.GetAllAsync()).Select(MapToListItem);

        var model = new UserListViewModel { Items = items.ToList() };

        return View(model);
    }

    [HttpGet("active")]
    public async Task<ViewResult> Active()
    {
        var items = (await _userService.FilterByActiveAsync(true)).Select(MapToListItem);
        
        var model = new UserListViewModel { Items = items.ToList() };

        return View("List", model);
    }

    [HttpGet("inactive")]
    public async Task<ViewResult> Inactive()
    {
        var items = (await _userService.FilterByActiveAsync(false)).Select(MapToListItem);

        var model = new UserListViewModel { Items = items.ToList() };

        return View("List", model);
    }

    // View action
    [HttpGet("view/{id}")]
    public async Task<IActionResult> View(long id)
    {
        var user = await _userService.GetByIdAsync(id);

        // Check if user is null
        if (user == null)
        {
            // Add better error/handling of responses
            return NotFound();
        }

        // Attempting to log the action
        await _logService.CreateAsync("VIEW", id, $"VIEWED User: {user.Forename} {user.Surname}");

        // Return logs with this so they can see it at bottom of screen

        var userLogs = await _logService.GetByUserIdAsync(id);

        var userActivityLogs = userLogs.Select(log => new UserActivityLogViewModel()
        {
            Id = log.Id,
            Action = log.Action,
            Details = log.Details,
            TimeStamp = log.TimeStamp
        }).ToList();

        var model = UserToModel(user);

        model.UserActivityLogs = userActivityLogs;

        
        return View(model);
    }

    // Create
    [HttpGet("create")]
    public IActionResult Create()
    {
        return View(new UserViewModel());
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = ModelToUser(model);

        var createdUser = await _userService.CreateAsync(user);

        await _logService.CreateAsync("CREATE", createdUser.Id, $"CREATED User: {createdUser.Forename} {createdUser.Surname}");
        SetMessage("CREATE", user);

        return RedirectToAction(nameof(List));
    }

    // Edit
    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(long id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        var model = UserToModel(user);

        return View(model);
    }

    [HttpPost("edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, UserViewModel model)
    {

        //Check that the ids match
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userService.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        UpdateUserFromViewModel(user, model);

        var updatedUser = await _userService.UpdateAsync(user);

        await _logService.CreateAsync("UPDATE", updatedUser.Id, $"UPDATED User: {updatedUser.Forename} {updatedUser.Surname}");
        SetMessage("UPDATE", user);

        return RedirectToAction(nameof(List));
    }

    // Delete
    [HttpGet("delete/{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        var model = UserToModel(user);

        return View(model);
    }

    [HttpPost("delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmDelete(long id)
    {
        var user = await _userService.DeleteAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        await _logService.CreateAsync("DELETE", id, $"DELETED User: {user.Forename} {user.Surname}");
        SetMessage("DELETE", user);
        return RedirectToAction(nameof(List));
    }


    #region Helper functions
    /// <summary>
    /// Attempting to map User to a User Model
    /// </summary>
    public UserViewModel UserToModel(User user)
    {
        return new UserViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            IsActive = user.IsActive,
            DateOfBirth = user?.DateOfBirth
        };
    }

    /// <summary>
    /// Attempting to map a User Model to a User
    /// </summary>
    private User ModelToUser(UserViewModel model)
    {
        return new User
        {
            Id = model.Id,
            Forename = model.Forename,
            Surname = model.Surname,
            Email = model.Email,
            IsActive = model.IsActive,
            DateOfBirth = model.DateOfBirth
        };
    }

    /// <summary>
    /// Updates an existing User model from UserViewModel
    /// </summary>
    private void UpdateUserFromViewModel(User user, UserViewModel model)
    {
        user.Forename = model.Forename;
        user.Surname = model.Surname;
        user.Email = model.Email;
        user.IsActive = model.IsActive;
        user.DateOfBirth = model.DateOfBirth;
    }

    private UserListItemViewModel MapToListItem(User user)
    {
        return new UserListItemViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth
        };
    }

    private void SetMessage(string action, User user)
    {
        TempData["Message"] = $"{action} successful: User {user.Forename} {user.Surname}";
    }
    #endregion

}
