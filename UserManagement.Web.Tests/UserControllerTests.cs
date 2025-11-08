using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Logs;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    [Fact]
    public async Task List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.List();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task List_WhenServiceReturnsEmpty_ShouldReturnEmptyModel()
    {
        // Arrange
        var controller = CreateController();
        _userService.Setup(s => s.GetAllAsync()).ReturnsAsync(System.Array.Empty<User>());
        // Act
        var result = await controller.List();

        // Assert
        result.Model.Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task Active_WhenServiceReturnsNoUsers_ModelMustContainNoUsers()
    {
        // Arrange
        var controller = CreateController();

        var users = System.Array.Empty<User>();

        _userService.Setup(s => s.FilterByActiveAsync(true))
            .ReturnsAsync(users);

        // Act
        var result = await controller.Active();

        // Assert
        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task Active_WhenServiceReturnsActiveUsers_ModelMustContainActiveUsers()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();

        // Act
        var result = await controller.Active();

        // Assert
        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task Inactive_WhenServiceReturnsNoUsers_ModelMustContainNoUsers()
    {
        // Arrange
        var controller = CreateController();

        var users = System.Array.Empty<User>();

        _userService.Setup(s => s.FilterByActiveAsync(false))
            .ReturnsAsync(users);

        // Act
        var result = await controller.Inactive();

        // Assert
        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task Inactive_WhenServiceReturnsInactiveUsers_ModelMustContainInactiveUsers()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers(isActive: false);

        // Act
        var result = await controller.Inactive();

        // Assert
        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task List_WhenUserHasDateOfBirth_ModelShouldIncludeDateOfBirth()
    {
        // Arrange
        var controller = CreateController();
        var dateOfBirth = new DateTime(2000, 1, 1);
        var user = new User
        {
            Forename = "Johnny",
            Surname = "Test",
            Email = "JohnnyTest@example.com",
            DateOfBirth = dateOfBirth
        };

        var users = new[] { user };


        _userService.Setup(s => s.GetAllAsync()).ReturnsAsync(users);

        // Act
        var result = await controller.List();

        // Arrange
        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task List_WhenUserHasNullDateOfBirth_ModelShouldHaveNullDateOfBirth()
    {
        // Arrange
        var controller = CreateController();
        var user = new User
        {
            Forename = "Johnny",
            Surname = "Test",
            Email = "JohnnyTest@example.com",
            DateOfBirth = null
        };

        var users = new[] { user };


        _userService.Setup(s => s.GetAllAsync()).ReturnsAsync(users);

        // Act
        var result = await controller.List();

        // Arrange
        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task Active_WhenUserHasDateOfBirth_ModelShouldIncludeDateOfBirth()
    {
        // Arrange
        var controller = CreateController();
        var dateOfBirth = new DateTime(2000, 1, 1);
        var user = new User
        {
            Forename = "Johnny",
            Surname = "Test",
            Email = "JohnnyTest@example.com",
            IsActive = true,
            DateOfBirth = dateOfBirth
        };

        var users = new[] { user };


        _userService.Setup(s => s.GetAllAsync()).ReturnsAsync(users);

        // Act
        var result = await controller.List();

        // Arrange
        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task Active_WhenUserHasNullDateOfBirth_ModelShouldHaveNullDateOfBirth()
    {
        // Arrange
        var controller = CreateController();
        var user = new User
        {
            Forename = "Johnny",
            Surname = "Test",
            Email = "JohnnyTest@example.com",
            IsActive = true,
            DateOfBirth = null
        };

        var users = new[] { user };


        _userService.Setup(s => s.GetAllAsync()).ReturnsAsync(users);

        // Act
        var result = await controller.List();

        // Arrange
        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task Inactive_WhenUserHasDateOfBirth_ModelShouldIncludeDateOfBirth()
    {
        // Arrange
        var controller = CreateController();
        var dateOfBirth = new DateTime(2000, 1, 1);
        var user = new User
        {
            Forename = "Johnny",
            Surname = "Test",
            Email = "JohnnyTest@example.com",
            IsActive = false,
            DateOfBirth = dateOfBirth
        };

        var users = new[] { user };


        _userService.Setup(s => s.GetAllAsync()).ReturnsAsync(users);

        // Act
        var result = await controller.List();

        // Arrange
        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task Inactive_WhenUserHasNullDateOfBirth_ModelShouldHaveNullDateOfBirth()
    {
        // Arrange
        var controller = CreateController();
        var user = new User
        {
            Forename = "Johnny",
            Surname = "Test",
            Email = "JohnnyTest@example.com",
            IsActive = false,
            DateOfBirth = null
        };

        var users = new[] { user };


        _userService.Setup(s => s.GetAllAsync()).ReturnsAsync(users);

        // Act
        var result = await controller.List();

        // Arrange
        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().BeEquivalentTo(users);
    }

    #region View Tests

    [Fact]
    public async Task View_WhenUserExists_ShouldReturnViewWithUserViewModel()
    {
        // Arrange
        var controller = CreateController();
        var user = new User
        {
            Id = 1,
            Forename = "Johnny",
            Surname = "Bravo",
            Email = "JBravo@example.com",
            IsActive = true,
            DateOfBirth = null,
        };

        _userService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);

        // Act
        var result = await controller.View(1);

        // Assert
        _userService.Verify(x => x.GetByIdAsync(1), Times.AtMostOnce);

        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<UserViewModel>().Subject;

        model.Id.Should().Be(1);
        model.Forename.Should().Be(user.Forename);
        model.Surname.Should().Be(user.Surname);
        model.Email.Should().Be(user.Email);
        model.IsActive.Should().Be(user.IsActive);
        model.DateOfBirth.Should().Be(user.DateOfBirth);
    }

    [Fact]
    public async Task View_WhenUserExists_ShouldCallService()
    {
        // Arrange
        var controller = CreateController();
        var user = new User
        {
            Id = 1,
            Forename = "Johnny",
            Surname = "Bravo",
            Email = "JBravo@example.com",
            IsActive = true,
            DateOfBirth = null,
        };

        _userService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);

        // Act
        var result = await controller.View(1);

        // Assert
        _userService.Verify(x => x.GetByIdAsync(1), Times.AtMostOnce);
    }

    [Fact]
    public async Task View_WhenUserDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var controller = CreateController();
        _userService.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((User?)null);

        // Act
        var result = await controller.View(999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task View_WhenUserHasDateOfBirth_ShouldReturnViewWithDateOfBirth()
    {
        // Arrange
        var controller = CreateController();
        var user = new User
        {
            Id = 1,
            Forename = "Johnny",
            Surname = "Bravo",
            Email = "JBravo@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(2000, 1, 1),
        };

        _userService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);

        // Act
        var result = await controller.View(1);

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<UserViewModel>().Subject;
        model.DateOfBirth.Should().NotBeNull();
    }

    [Fact]
    public async Task View_WhenUserHasNullDateOfBirth_ShouldReturnViewWithNullDateOfBirth()
    {
        // Arrange
        var controller = CreateController();
        var user = new User
        {
            Id = 1,
            Forename = "Johnny",
            Surname = "Bravo",
            Email = "JBravo@example.com",
            IsActive = true,
            DateOfBirth = null,
        };

        _userService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);

        // Act
        var result = await controller.View(1);

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<UserViewModel>().Subject;
        model.DateOfBirth.Should().BeNull();
    }


    [Fact]
    public async Task View_WhenUserHasActiveStatus_ShouldReturnViewWithUserWithActiveStatus()
    {
        // Arrange
        var controller = CreateController();
        var user = new User
        {
            Id = 1,
            Forename = "Johnny",
            Surname = "Bravo",
            Email = "JBravo@example.com",
            IsActive = true,
            DateOfBirth = null,
        };

        _userService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);

        // Act
        var result = await controller.View(1);

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<UserViewModel>().Subject;
        model.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task View_WhenUserHasInactiveStatus_ShouldReturnViewWithUserWithInactiveStatus()
    {
        // Arrange
        var controller = CreateController();
        var user = new User
        {
            Id = 1,
            Forename = "Johnny",
            Surname = "Bravo",
            Email = "JBravo@example.com",
            IsActive = false,
            DateOfBirth = null,
        };

        _userService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);

        // Act
        var result = await controller.View(1);

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<UserViewModel>().Subject;
        model.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task View_WhenUserExists_ShouldCallLogsService()
    {
        // Arrange
        var controller = CreateController();
        var user = new User
        {
            Id = 1,
            Forename = "Johnny",
            Surname = "Bravo",
            Email = "JBravo@example.com",
            IsActive = true,
            DateOfBirth = null,
        };

        var logs = new[]
        {
            new Log
            {
                Id = 1,
                UserId = 1,
                Action = "CREATE",
                Details = "Created user: Johnny Bravo",
                TimeStamp = DateTime.UtcNow.AddDays(-2)
            },
            new Log
            {
                Id = 2,
                UserId = 1,
                Action = "UPDATE",
                Details = "Updated user: Johnny Bravo",
                TimeStamp = DateTime.UtcNow.AddDays(-1)
            }
        };

        _userService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);
        _logService.Setup(x => x.GetByUserIdAsync(1)).ReturnsAsync(logs);

        // Act
        var result = await controller.View(1);

        // Assert
        _userService.Verify(x => x.GetByIdAsync(1), Times.AtMostOnce);
        _logService.Verify(x => x.GetByIdAsync(1), Times.AtMostOnce);
    }

    [Fact]
    public async Task View_WhenUserExists_ShouldReturnUserWithUsersActivityLogs()
    {
        // Arrange
        var controller = CreateController();
        var user = new User
        {
            Id = 1,
            Forename = "Johnny",
            Surname = "Bravo",
            Email = "JBravo@example.com",
            IsActive = true,
            DateOfBirth = null,
        };

        var logs = new[]
        {
            new Log
            {
                Id = 1,
                UserId = 1,
                Action = "CREATE",
                Details = "Created user: Johnny Bravo",
                TimeStamp = DateTime.UtcNow.AddDays(-2)
            },
            new Log
            {
                Id = 2,
                UserId = 1,
                Action = "UPDATE",
                Details = "Updated user: Johnny Bravo",
                TimeStamp = DateTime.UtcNow.AddDays(-1)
            }
        };

        _userService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);
        _logService.Setup(x => x.GetByUserIdAsync(1)).ReturnsAsync(logs);

        // Act
        var result = await controller.View(1);

        // Assert
        _userService.Verify(x => x.GetByIdAsync(1), Times.AtMostOnce);
        _logService.Verify(x => x.GetByIdAsync(1), Times.AtMostOnce);

        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<UserViewModel>().Subject;

        model.UserActivityLogs.Should().HaveCount(2);
        model.UserActivityLogs[0].Action.Should().Be("CREATE");
        model.UserActivityLogs[1].Action.Should().Be("UPDATE");
    }

    [Fact]
    public async Task View_WhenUserExistsWithNoLogs_ShouldReturnUserWithEmptyUsersActivityLogs()
    {
        // Arrange
        var controller = CreateController();
        var user = new User
        {
            Id = 1,
            Forename = "Johnny",
            Surname = "Bravo",
            Email = "JBravo@example.com",
            IsActive = true,
            DateOfBirth = null,
        };

        var logs = Array.Empty<Log>();

        _userService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);
        _logService.Setup(x => x.GetByUserIdAsync(1)).ReturnsAsync(logs);

        // Act
        var result = await controller.View(1);

        // Assert
        _userService.Verify(x => x.GetByIdAsync(1), Times.AtMostOnce);
        _logService.Verify(x => x.GetByIdAsync(1), Times.AtMostOnce);

        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<UserViewModel>().Subject;

        model.UserActivityLogs.Should().BeEmpty();
    }

    #endregion

    #region Create Tests

    [Fact]
    public Task Create_Get_ShouldReturnViewWithEmptyModel()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.Create();

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.Model.Should().BeOfType<UserViewModel>();

        return Task.CompletedTask;
    }

    // ERROR
    [Fact]
    public async Task Create_Post_WhenModelIsValid_ShouldCreateUserAndRedirectToList()
    {
        // Arrange
        var controller = CreateController();

        var model = new UserViewModel
        {
            Forename = "Johnny",
            Surname = "Bravo",
            Email = "JBravo@example.com",
            IsActive = false,
            DateOfBirth = null,
        };

        var createdUser = new User
        {
            Id = 1,
            Forename = model.Forename,
            Surname = model.Surname,
            Email = model.Email,
            IsActive = model.IsActive,
            DateOfBirth = model.DateOfBirth
        };

        _userService.Setup(x => x.CreateAsync(It.IsAny<User>())).ReturnsAsync(createdUser);

        // Act
        var result = await controller.Create(model);

        // Assert
        var redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
        redirectResult.ActionName.Should().Be(nameof(UsersController.List));

        _userService.Verify(x => x.CreateAsync(It.Is<User>(u =>
            u.Id == createdUser.Id &&
            u.Forename == model.Forename &&
            u.Surname == model.Surname && u.Email == model.Email
        )), Times.AtMostOnce);

        controller.TempData["Message"].Should().NotBeNull();
        controller.TempData["Message"].Should().Be($"CREATE successful: User {model.Forename} {model.Surname}");
    }

    [Fact]
    public async Task Create_Post_WhenModelIsInvalid_ShouldNotCallService()
    {
        // Arrange
        var controller = CreateController();
        controller.ModelState.AddModelError("Error", "Error");

        var model = new UserViewModel
        {
            Forename = "",
            Surname = "",
            Email = "incorrectEmail",
            IsActive = true,
        };

        // Act
        var result = await controller.Create(model);

        // Assert
        _userService.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Create_Post_WhenModelIsInvalid_ShouldReturnViewWithModel()
    {
        // Arrange
        var controller = CreateController();
        controller.ModelState.AddModelError("Error", "Error");

        var model = new UserViewModel
        {
            Forename = "",
            Surname = "",
            Email = "incorrectEmail",
            IsActive = true,
        };

        // Act
        var result = await controller.Create(model);

        // Assert
        _userService.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Never);

        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.Model.Should().Be(model);
    }

    [Fact]
    public async Task Create_Post_WhenUserWithNullDateOfBirthProvided_ShouldCallServiceAndCreateUser()
    {
        // Arrange
        var controller = CreateController();

        var model = new UserViewModel
        {
            Forename = "Johnny",
            Surname = "Bravo",
            Email = "JBravo@example.com",
            IsActive = false,
            DateOfBirth = null,
        };

        var createdUser = new User
        {
            Id = 1,
            Forename = model.Forename,
            Surname = model.Surname,
            Email = model.Email,
            IsActive = model.IsActive,
            DateOfBirth = null
        };

        _userService.Setup(x => x.CreateAsync(It.IsAny<User>())).ReturnsAsync(createdUser);

        // Act
        var result = await controller.Create(model);

        // Assert
        _userService.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.AtMostOnce);

        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    public async Task Create_Post_WhenUserWithActiveStatus_ShouldCallServiceAndCreateUser()
    {
        // Arrange
        var controller = CreateController();

        var model = new UserViewModel
        {
            Forename = "Johnny",
            Surname = "Bravo",
            Email = "JBravo@example.com",
            IsActive = true,
            DateOfBirth = null,
        };

        var createdUser = new User
        {
            Id = 1,
            Forename = model.Forename,
            Surname = model.Surname,
            Email = model.Email,
            IsActive = model.IsActive,
            DateOfBirth = null
        };

        _userService.Setup(x => x.CreateAsync(It.IsAny<User>())).ReturnsAsync(createdUser);

        // Act
        var result = await controller.Create(model);

        // Assert
        _userService.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.AtMostOnce);

        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    public async Task Create_Post_WhenUserWithInactiveStatus_ShouldCallServiceAndCreateUser()
    {
        // Arrange
        var controller = CreateController();

        var model = new UserViewModel
        {
            Forename = "Johnny",
            Surname = "Bravo",
            Email = "JBravo@example.com",
            IsActive = false,
            DateOfBirth = null,
        };

        var createdUser = new User
        {
            Id = 1,
            Forename = model.Forename,
            Surname = model.Surname,
            Email = model.Email,
            IsActive = model.IsActive,
            DateOfBirth = null
        };

        _userService.Setup(x => x.CreateAsync(It.IsAny<User>())).ReturnsAsync(createdUser);

        // Act
        var result = await controller.Create(model);

        // Assert
        _userService.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.AtMostOnce);

        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    public async Task Create_Post_WhenUserWithDateOfBirthProvided_ShouldCallServiceAndCreateUser()
    {
        // Arrange
        var controller = CreateController();

        var model = new UserViewModel
        {
            Forename = "Johnny",
            Surname = "Bravo",
            Email = "JBravo@example.com",
            IsActive = false,
            DateOfBirth = new DateTime(2000, 10, 10),
        };

        var createdUser = new User
        {
            Id = 1,
            Forename = model.Forename,
            Surname = model.Surname,
            Email = model.Email,
            IsActive = model.IsActive,
            DateOfBirth = null
        };

        _userService.Setup(x => x.CreateAsync(It.IsAny<User>())).ReturnsAsync(createdUser);

        // Act
        var result = await controller.Create(model);

        // Assert
        _userService.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.AtMostOnce);

        result.Should().BeOfType<RedirectToActionResult>();
    }

    #endregion

    #region Edit Tests

    [Fact]
    public async Task Edit_Get_WhenUserExists_ShouldCallService()
    {
        // Arrange
        var controller = CreateController();
        var user = new User
        {
            Id = 1,
            Forename = "Johnny",
            Surname = "Bravo",
            Email = "JBravo@example.com",
            IsActive = false,
            DateOfBirth = new DateTime(2000, 10, 10),
        };

        _userService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);

        // Act
        var result = await controller.Edit(1);

        // Assert
        _userService.Verify(x => x.GetByIdAsync(1), Times.AtMostOnce);
    }

    [Fact]
    public async Task Edit_Get_WhenUserExists_ShouldReturnViewWithUserViewModel()
    {
        // Arrange
        var controller = CreateController();
        var user = new User
        {
            Id = 1,
            Forename = "Johnny",
            Surname = "Bravo",
            Email = "JBravo@example.com",
            IsActive = false,
            DateOfBirth = new DateTime(2000, 10, 10),
        };

        _userService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);

        // Act
        var result = await controller.Edit(1);

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<UserViewModel>().Subject;
        model.Id.Should().Be(1);
        model.Forename.Should().Be(user.Forename);
        model.Surname.Should().Be(user.Surname);
        model.Email.Should().Be(user.Email);
    }

    [Fact]
    public async Task Edit_Get_WhenUserDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var controller = CreateController();
        _userService.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((User?)null);

        // Act
        var result = await controller.Edit(999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Edit_Post_WhenModelIsValid_ShouldUpdateUserAndRedirectToList()
    {
        // Arrange
        var controller = CreateController();
        var existingUser = new User
        {
            Id = 1,
            Forename = "Simba",
            Surname = "Nala",
            Email = "savannahPlains@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        var model = new UserViewModel
        {
            Id = 1,
            Forename = "Lion",
            Surname = "Lioness",
            Email = "tundra@example.com",
            IsActive = false,
            DateOfBirth = new DateTime(1990, 2, 2)
        };

        _userService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(existingUser);
        _userService.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(existingUser);

        // Act
        var result = await controller.Edit(1, model);

        // Assert
        var redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
        redirectResult.ActionName.Should().Be(nameof(UsersController.List));
        _userService.Verify(x => x.UpdateAsync(It.Is<User>(u =>
            u.Id == model.Id &&
            u.Forename == model.Forename &&
            u.Surname == model.Surname &&
            u.Email == model.Email
        )), Times.Once);

        controller.TempData["Message"].Should().NotBeNull();
        controller.TempData["Message"].Should().Be($"UPDATE successful: User {model.Forename} {model.Surname}");
    }

    [Fact]
    public async Task Edit_Post_WhenIdsDoNotMatch_ShouldReturnBadRequest()
    {
        // Arrange
        var controller = CreateController();
        var model = new UserViewModel { Id = 1 };

        // Act
        var result = await controller.Edit(2, model); // ID mismatch

        // Assert
        result.Should().BeOfType<BadRequestResult>();
        _userService.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Edit_Post_WhenModelIsInvalid_ShouldReturnViewWithModel()
    {
        // Arrange
        var controller = CreateController();
        controller.ModelState.AddModelError("Forename", "Required");

        var model = new UserViewModel
        {
            Id = 1,
            Forename = "", // Invalid
            Email = "invalid"
        };

        // Act
        var result = await controller.Edit(1, model);

        // Assert
        _userService.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);

        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.Model.Should().Be(model);
    }

    [Fact]
    public async Task Edit_Post_WhenUserDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var controller = CreateController();
        var model = new UserViewModel
        {
            Id = 999,
            Forename = "Coles",
            Surname = "Law",
            Email = "ShreddedLettuce@example.com",
            IsActive = true
        };

        _userService.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((User?)null);

        // Act
        var result = await controller.Edit(999, model);

        // Assert
        _userService.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);

        result.Should().BeOfType<NotFoundResult>();
    }

    #endregion

    #region Delete Tests

    [Fact]
    public async Task Delete_Get_WhenUserExists_ShouldCallService()
    {
        var controller = CreateController();
        var user = new User
        {
            Forename = "Coles",
            Surname = "Law",
            Email = "ShreddedLettuce@example.com",
            IsActive = true
        };

        _userService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);

        // Act
        var result = await controller.Delete(1);

        // Assert
        _userService.Verify(x => x.GetByIdAsync(1), Times.AtMostOnce);
    }

    [Fact]
    public async Task Delete_Get_WhenUserExists_ShouldReturnViewWithUserViewModel()
    {
        var controller = CreateController();
        var user = new User
        {
            Id = 1,
            Forename = "Coles",
            Surname = "Law",
            Email = "ShreddedLettuce@example.com",
            IsActive = true
        };

        _userService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);

        // Act
        var result = await controller.Delete(1);

        // Assert
        _userService.Verify(x => x.GetByIdAsync(1), Times.AtMostOnce);

        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<UserViewModel>().Subject;

        model.Id.Should().Be(1);
        model.Forename.Should().Be(user.Forename);
        model.Surname.Should().Be(user.Surname);
        model.Email.Should().Be(user.Email);
        model.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_Get_WhenUserDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var controller = CreateController();
        _userService.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((User?)null);

        // Act
        var result = await controller.Delete(999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task ConfirmDelete_Post_WhenUserExists_ShouldDeleteUserAndRedirectToList()
    {
        var controller = CreateController();
        var deletedUser = new User()
        {
            Id = 1,
            Forename = "Forename",
            Surname = "Surname",
            Email = "email@email.com",
            IsActive = true,
            DateOfBirth = null,
        };

        _userService.Setup(x => x.DeleteAsync(1)).ReturnsAsync(deletedUser);

        // Act
        var result = await controller.ConfirmDelete(1);

        // Assert
        _userService.Verify(x => x.DeleteAsync(1), Times.AtMostOnce);

        var redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
        redirectResult.ActionName.Should().Be(nameof(UsersController.List));

        controller.TempData["Message"].Should().NotBeNull();
        controller.TempData["Message"].Should()
            .Be($"DELETE successful: User {deletedUser.Forename} {deletedUser.Surname}");
    }

    [Fact]
    public async Task ConfirmDelete_Post_WhenUserDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var controller = CreateController();
        _userService.Setup(x => x.DeleteAsync(999)).ReturnsAsync((User?)null);

        // Act
        var result = await controller.ConfirmDelete(999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task ConfirmDelete_Post_WhenInactiveUserProvided_ShouldDeleteUser()
    {
        // Arrange
        var controller = CreateController();
        var user = new User
        {
            Id = 1,
            Forename = "InactiveForename",
            Surname = "InactiveSurname",
            Email = "inactiveEmail@example.com",
            IsActive = false,
            DateOfBirth = new DateTime(1985, 3, 20)
        };

        _userService.Setup(x => x.DeleteAsync(1)).ReturnsAsync(user);

        // Act
        var result = await controller.ConfirmDelete(1);

        // Assert
        _userService.Verify(x => x.DeleteAsync(1), Times.Once);

        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    public async Task ConfirmDelete_Post_WhenActiveUserProvided_ShouldDeleteUser()
    {
        // Arrange
        var controller = CreateController();
        var user = new User
        {
            Id = 1,
            Forename = "InactiveForename",
            Surname = "InactiveSurname",
            Email = "inactiveEmail@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(1985, 3, 20)
        };

        _userService.Setup(x => x.DeleteAsync(1)).ReturnsAsync(user);

        // Act
        var result = await controller.ConfirmDelete(1);

        // Assert
        _userService.Verify(x => x.DeleteAsync(1), Times.Once);

        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    public async Task ConfirmDelete_Post_WhenUserWithDateOfBirthProvided_ShouldDeleteUser()
    {
        // Arrange
        var controller = CreateController();
        var user = new User
        {
            Id = 1,
            Forename = "InactiveForename",
            Surname = "InactiveSurname",
            Email = "inactiveEmail@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(1985, 3, 20)
        };

        _userService.Setup(x => x.DeleteAsync(1)).ReturnsAsync(user);

        // Act
        var result = await controller.ConfirmDelete(1);

        // Assert
        _userService.Verify(x => x.DeleteAsync(1), Times.Once);

        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    public async Task ConfirmDelete_Post_WhenUserWithNullDateOfBirthProvided_ShouldDeleteUser()
    {
        // Arrange
        var controller = CreateController();
        var user = new User
        {
            Id = 1,
            Forename = "InactiveForename",
            Surname = "InactiveSurname",
            Email = "inactiveEmail@example.com",
            IsActive = true,
            DateOfBirth = null
        };

        _userService.Setup(x => x.DeleteAsync(1)).ReturnsAsync(user);

        // Act
        var result = await controller.ConfirmDelete(1);

        // Assert
        _userService.Verify(x => x.DeleteAsync(1), Times.Once);

        result.Should().BeOfType<RedirectToActionResult>();
    }

    #endregion

    #region Logging Tests

    #region View Logging Tests

    [Fact]
    public async Task View_WhenUserExists_ShouldLogViewAction()
    {
        // Arrange
        var controller = CreateController();
        var user = new User()
        {
            Id = 1,
            Forename = "Felix",
            Surname = "Bjorn",
            Email = "pewdipie@example.com",
            IsActive = true
        };

        _userService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);

        // Act
        var result = await controller.View(1);

        // Assert
        _logService.Verify(x => x.CreateAsync(
            "VIEW",
            1,
            "Viewed user: Johnny Bravo"), Times.AtMostOnce());
    }

    [Fact]
    public async Task View_WhenUserDoesNotExist_ShouldNotLogAction()
    {
        // Arrange
        var controller = CreateController();
        _userService.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((User?)null);

        // Act
        var result = await controller.View(999);

        // Assert
        _logService.Verify(x => x.CreateAsync(
            It.IsAny<string>(),
            It.IsAny<long?>(),
            It.IsAny<string>()), Times.Never);
    }

    #endregion

    #region Create Logging Tests

    [Fact]
    public async Task Create_WhenModelIsValid_ShouldCallLogger()
    {
        // Arrange
        var controller = CreateController();
        var model = new UserViewModel
        {
            Forename = "Johnny",
            Surname = "Bravo",
            Email = "JBravo@example.com",
            IsActive = true
        };

        var user = new User
        {
            Forename = model.Forename,
            Surname = model.Surname,
            Email = model.Email,
            IsActive = model.IsActive
        };

        var createdUser = new User
        {
            Id = 1,
            Forename = model.Forename,
            Surname = model.Surname,
            Email = model.Email,
            IsActive = model.IsActive
        };

        _userService.Setup(x => x.CreateAsync(It.IsAny<User>())).ReturnsAsync(createdUser);

        // Act
        var result = await controller.Create(model);

        // Assert
        _logService.Verify(x => x.CreateAsync(
            It.IsAny<string>(),
            It.IsAny<long?>(),
            It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Create_WhenModelIsValid_ShouldLogCreateAction()
    {
        // Arrange
        var controller = CreateController();
        var model = new UserViewModel
        {
            Forename = "Johnny",
            Surname = "Bravo",
            Email = "JBravo@example.com",
            IsActive = true
        };

        var createdUser = new User
        {
            Id = 1,
            Forename = model.Forename,
            Surname = model.Surname,
            Email = model.Email,
            IsActive = model.IsActive
        };

        _userService.Setup(x => x.CreateAsync(It.IsAny<User>())).ReturnsAsync(createdUser);

        // Act
        var result = await controller.Create(model);

        // Assert
        _logService.Verify(x => x.CreateAsync(
            "CREATE",
            1,
            "CREATED User: Johnny Bravo"
        ), Times.Once);
    }

    [Fact]
    public async Task Create_WhenModelIsInvalid_ShouldNotLogAction()
    {
        // Arrange
        var controller = CreateController();
        controller.ModelState.AddModelError("Error", "Error");
        var model = new UserViewModel
        {
            Forename = "Johnny",
            Surname = "Bravo",
            Email = "JBravo@example.com",
            IsActive = true
        };

        // Act
        var result = await controller.Create(model);

        // Assert
        _logService.Verify(x => x.CreateAsync(
            It.IsAny<string>(),
            It.IsAny<long?>(),
            It.IsAny<string>()), Times.Never);
    }

    #endregion

    #region Edit Logging Tests

    [Fact]
    public async Task Edit_WhenModelIsValid_ShouldCallLogger()
    {
        // Arrange
        var controller = CreateController();
        var existingUser = new User
        {
            Id = 1,
            Forename = "Old",
            Surname = "Name",
            Email = "old@example.com",
            IsActive = true
        };

        var model = new UserViewModel
        {
            Id = 1,
            Forename = "New",
            Surname = "Name",
            Email = "new@example.com",
            IsActive = false
        };

        var updatedUser = new User
        {
            Id = 1,
            Forename = model.Forename,
            Surname = model.Surname,
            Email = model.Email,
            IsActive = model.IsActive
        };

        _userService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(existingUser);
        _userService.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(updatedUser);

        // Act
        var result = await controller.Edit(1, model);

        // Assert
        _logService.Verify(x => x.CreateAsync(
            It.IsAny<string>(),
            It.IsAny<long?>(),
            It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Edit_WhenModelIsValid_ShouldLogUpdateAction()
    {
        // Arrange
        var controller = CreateController();
        var existingUser = new User
        {
            Id = 1,
            Forename = "Old",
            Surname = "Name",
            Email = "old@example.com",
            IsActive = true
        };

        var model = new UserViewModel
        {
            Id = 1,
            Forename = "New",
            Surname = "Name",
            Email = "new@example.com",
            IsActive = false
        };

        var updatedUser = new User
        {
            Id = 1,
            Forename = model.Forename,
            Surname = model.Surname,
            Email = model.Email,
            IsActive = model.IsActive
        };

        _userService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(existingUser);
        _userService.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(updatedUser);

        // Act
        var result = await controller.Edit(1, model);

        // Assert
        _logService.Verify(x => x.CreateAsync(
            "UPDATE",
            1,
            "UPDATED User: New Name"), Times.Once);
    }

    [Fact]
    public async Task Edit_Post_WhenModelIsInvalid_ShouldNotLogAction()
    {
        // Arrange
        var controller = CreateController();
        controller.ModelState.AddModelError("Error", "Error");

        var model = new UserViewModel
        {
            Id = 1,
            Forename = "",
            Email = "invalid"
        };

        // Act
        var result = await controller.Edit(1, model);

        // Assert
        _logService.Verify(x => x.CreateAsync(
            It.IsAny<string>(),
            It.IsAny<long?>(),
            It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Edit_Post_WhenUserDoesNotExist_ShouldNotLogAction()
    {
        // Arrange
        var controller = CreateController();
        var model = new UserViewModel
        {
            Id = 999,
            Forename = "Test",
            Surname = "User",
            Email = "test@example.com"
        };

        _userService.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((User?)null);

        // Act
        var result = await controller.Edit(999, model);

        // Assert
        _logService.Verify(x => x.CreateAsync(
            It.IsAny<string>(),
            It.IsAny<long?>(),
            It.IsAny<string>()
        ), Times.Never);
    }

    #endregion

    #region Delete Logging Tests

    [Fact]
    public async Task ConfirmDelete_WhenUserExists_ShouldCallLogger()
    {
        // Arrange
        var controller = CreateController();
        var deletedUser = new User
        {
            Id = 1,
            Forename = "Forename",
            Surname = "Surname",
            Email = "deleted@example.com",
            IsActive = true
        };

        _userService.Setup(x => x.DeleteAsync(1)).ReturnsAsync(deletedUser);

        // Act
        var result = await controller.ConfirmDelete(1);

        // Assert
        _logService.Verify(x => x.CreateAsync(
            It.IsAny<string>(),
            It.IsAny<long?>(),
            It.IsAny<string>()
        ), Times.Once);
    }

    [Fact]
    public async Task ConfirmDelete_WhenUserExists_ShouldLogDeleteAction()
    {
        // Arrange
        var controller = CreateController();
        var deletedUser = new User
        {
            Id = 1,
            Forename = "Forename",
            Surname = "Surname",
            Email = "deleted@example.com",
            IsActive = true
        };

        _userService.Setup(x => x.DeleteAsync(1)).ReturnsAsync(deletedUser);

        // Act
        var result = await controller.ConfirmDelete(1);

        // Assert
        _logService.Verify(x => x.CreateAsync(
            "DELETE",
            1,
            "DELETED User: Forename Surname"
        ), Times.Once);
    }

    [Fact]
    public async Task ConfirmDelete_Post_WhenUserDoesNotExist_ShouldNotLogAction()
    {
        // Arrange
        var controller = CreateController();
        _userService.Setup(x => x.DeleteAsync(999)).ReturnsAsync((User?)null);

        // Act
        var result = await controller.ConfirmDelete(999);

        // Assert
        _logService.Verify(x => x.CreateAsync(
            It.IsAny<string>(),
            It.IsAny<long?>(),
            It.IsAny<string>()
        ), Times.Never);
    }

    #endregion

    #endregion

    private User[] SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com",
        bool isActive = true)
    {
        var users = new[] { new User { Forename = forename, Surname = surname, Email = email, IsActive = isActive } };

        _userService
            .Setup(s => s.GetAllAsync())
            .ReturnsAsync(users);

        _userService.Setup(s => s.FilterByActiveAsync(true))
            .ReturnsAsync(users);

        _userService.Setup(s => s.FilterByActiveAsync(false))
            .ReturnsAsync(users);

        return users;
    }

    private readonly Mock<IUserService> _userService = new();
    private readonly Mock<ILogService> _logService = new();

    private UsersController CreateController()
    {
        var controller = new UsersController(_userService.Object, _logService.Object);

        controller.TempData = new TempDataDictionary(
            new DefaultHttpContext(),
            Mock.Of<ITempDataProvider>());

        return controller;
    }
}
