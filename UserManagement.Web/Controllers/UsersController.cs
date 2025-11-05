using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    // Add route to this maybe list?
    [HttpGet]
    public ViewResult List()
    {
        var items = _userService.GetAll().Select(MapToListItem);

        var model = new UserListViewModel { Items = items.ToList() };

        return View(model);
    }

    [HttpGet("active")]
    public ViewResult Active()
    {
        var items = _userService.FilterByActive(true).Select(MapToListItem);
        
        var model = new UserListViewModel { Items = items.ToList() };

        return View("List", model);
    }

    [HttpGet("inactive")]
    public ViewResult Inactive()
    {
        var items = _userService.FilterByActive(false).Select(MapToListItem);

        var model = new UserListViewModel { Items = items.ToList() };

        return View("List", model);
    }

    // View action
    [HttpGet("view/{id}")]
    public IActionResult View(long id)
    {
        var user = _userService.GetById(id);

        // Check if user is null
        if (user == null)
        {
            // Add better error/handling of responses
            return NotFound();
        }

        var model = UserToModel(user);

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
    public IActionResult Create(UserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = ModelToUser(model);

        _userService.Create(user);

        SetMessage("CREATE", user);
        return RedirectToAction(nameof(List));
    }

    // Edit
    [HttpGet("edit/{id}")]
    public IActionResult Edit(long id)
    {
        var user = _userService.GetById(id);

        if (user == null)
        {
            return NotFound();
        }

        var model = UserToModel(user);

        return View(model);
    }

    [HttpPost("edit/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(long id, UserViewModel model)
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

        var user = _userService.GetById(id);

        if (user == null)
        {
            return NotFound();
        }

        UpdateUserFromViewModel(user, model);

        _userService.Update(user);

        SetMessage("UPDATE", user);
        return RedirectToAction(nameof(List));
    }

    // Delete
    [HttpGet("delete/{id}")]
    public IActionResult Delete(long id)
    {
        var user = _userService.GetById(id);

        if (user == null)
        {
            return NotFound();
        }

        var model = UserToModel(user);

        return View(model);
    }

    [HttpPost("delete/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult ConfirmDelete(long id)
    {
        var user = _userService.Delete(id);

        if (user == null)
        {
            return NotFound();
        }

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
