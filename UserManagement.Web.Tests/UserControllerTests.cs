using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    [Fact]
    public void List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = controller.List();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void List_WhenServiceReturnsEmpty_ShouldReturnEmptyModel()
    {
        // Arrange
        var controller = CreateController();
        _userService.Setup(s => s.GetAll()).Returns(System.Array.Empty<User>());
        // Act
        var result = controller.List();

        // Assert
        result.Model.Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEmpty();
    }

    [Fact]
    public void Active_WhenServiceReturnsNoUsers_ModelMustContainNoUsers()
    {
        // Arrange
        var controller = CreateController();

        var users = System.Array.Empty<User>();

        _userService.Setup(s => s.FilterByActive(true))
            .Returns(users);

        // Act
        var result = controller.Active();

        // Assert
        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void Active_WhenServiceReturnsActiveUsers_ModelMustContainActiveUsers()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();

        // Act
        var result = controller.Active();

        // Assert
        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void Inactive_WhenServiceReturnsNoUsers_ModelMustContainNoUsers()
    {
        // Arrange
        var controller = CreateController();

        var users = System.Array.Empty<User>();

        _userService.Setup(s => s.FilterByActive(false))
            .Returns(users);

        // Act
        var result = controller.Inactive();

        // Assert
        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void Inactive_WhenServiceReturnsInactiveUsers_ModelMustContainInactiveUsers()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers(isActive: false);

        // Act
        var result = controller.Inactive();

        // Assert
        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void List_WhenUserHasDateOfBirth_ModelShouldIncludeDateOfBirth()
    {
        // Arrange
        var controller = CreateController();
        var dateOfBirth = new DateTime(2000, 1, 1);
        var user = new User
        {
            Forename = "Johnny", Surname = "Test", Email = "JohnnyTest@example.com", DateOfBirth = dateOfBirth
        };

        var users = new[] { user };


        _userService.Setup(s => s.GetAll()).Returns(users);

        // Act
        var result = controller.List();

        // Arrange
        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void List_WhenUserHasNullDateOfBirth_ModelShouldHaveNullDateOfBirth()
    {
        // Arrange
        var controller = CreateController();
        var user = new User
        {
            Forename = "Johnny", Surname = "Test", Email = "JohnnyTest@example.com", DateOfBirth = null
        };

        var users = new[] { user };


        _userService.Setup(s => s.GetAll()).Returns(users);

        // Act
        var result = controller.List();

        // Arrange
        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void Active_WhenUserHasDateOfBirth_ModelShouldIncludeDateOfBirth()
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


        _userService.Setup(s => s.GetAll()).Returns(users);

        // Act
        var result = controller.List();

        // Arrange
        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void Active_WhenUserHasNullDateOfBirth_ModelShouldHaveNullDateOfBirth()
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


        _userService.Setup(s => s.GetAll()).Returns(users);

        // Act
        var result = controller.List();

        // Arrange
        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void Inactive_WhenUserHasDateOfBirth_ModelShouldIncludeDateOfBirth()
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


        _userService.Setup(s => s.GetAll()).Returns(users);

        // Act
        var result = controller.List();

        // Arrange
        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void Inactive_WhenUserHasNullDateOfBirth_ModelShouldHaveNullDateOfBirth()
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


        _userService.Setup(s => s.GetAll()).Returns(users);

        // Act
        var result = controller.List();

        // Arrange
        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().BeEquivalentTo(users);
    }

    #region View Tests

    [Fact]
    public void View_WhenUserExists_ShouldReturnViewWithUserViewModel()
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

        _userService.Setup(x => x.GetById(1)).Returns(user);

        // Act
        var result = controller.View(1);

        // Assert
        _userService.Verify(x => x.GetById(1), Times.AtMostOnce);

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
    public void View_WhenUserExists_ShouldCallService()
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

        _userService.Setup(x => x.GetById(1)).Returns(user);

        // Act
        var result = controller.View(1);

        // Assert
        _userService.Verify(x => x.GetById(1), Times.AtMostOnce);
    }

    [Fact]
    public void View_WhenUserDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var controller = CreateController();
        _userService.Setup(x => x.GetById(999)).Returns((User?)null);

        // Act
        var result = controller.View(999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public void View_WhenUserHasDateOfBirth_ShouldReturnViewWithDateOfBirth()
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

        _userService.Setup(x => x.GetById(1)).Returns(user);

        // Act
        var result = controller.View(1);

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<UserViewModel>().Subject;
        model.DateOfBirth.Should().NotBeNull();
    }

    [Fact]
    public void View_WhenUserHasNullDateOfBirth_ShouldReturnViewWithNullDateOfBirth()
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

        _userService.Setup(x => x.GetById(1)).Returns(user);

        // Act
        var result = controller.View(1);

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<UserViewModel>().Subject;
        model.DateOfBirth.Should().BeNull();
    }


    [Fact]
    public void View_WhenUserHasActiveStatus_ShouldReturnViewWithUserWithActiveStatus()
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

        _userService.Setup(x => x.GetById(1)).Returns(user);

        // Act
        var result = controller.View(1);

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<UserViewModel>().Subject;
        model.IsActive.Should().BeTrue();
    }

    [Fact]
    public void View_WhenUserHasInactiveStatus_ShouldReturnViewWithUserWithInactiveStatus()
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

        _userService.Setup(x => x.GetById(1)).Returns(user);

        // Act
        var result = controller.View(1);

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<UserViewModel>().Subject;
        model.IsActive.Should().BeFalse();
    }

    #endregion

    #region Create Tests

    [Fact]
    public void Create_Get_ShouldReturnViewWithEmptyModel()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.Create();

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.Model.Should().BeOfType<UserViewModel>();
    }

    // ERROR
    [Fact]
    public void Create_Post_WhenModelIsValid_ShouldCreateUserAndRedirectToList()
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

        _userService.Setup(x => x.Create(It.IsAny<User>())).Returns(createdUser);
        //_userService.Setup(x => x.)

        // Act
        var result = controller.Create(model);

        // Assert
        var redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
        redirectResult.ActionName.Should().Be(nameof(UsersController.List));

        _userService.Verify(x => x.Create(It.Is<User>(u =>
            u.Id == createdUser.Id &&
            u.Forename == model.Forename &&
            u.Surname == model.Surname && u.Email == model.Email
        )), Times.AtMostOnce);

        controller.TempData["Message"].Should().NotBeNull();
        controller.TempData["Message"].Should().Be($"CREATE successful: User {model.Forename} {model.Surname}");
    }

    [Fact]
    public void Create_Post_WhenModelIsInvalid_ShouldNotCallService()
    {
        // Arrange
        var controller = CreateController();
        controller.ModelState.AddModelError("Error", "Error");

        var model = new UserViewModel
        {
            Forename = "", Surname = "", Email = "incorrectEmail", IsActive = true,
        };

        // Act
        var result = controller.Create(model);

        // Assert
        _userService.Verify(x => x.Create(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public void Create_Post_WhenModelIsInvalid_ShouldReturnViewWithModel()
    {
        // Arrange
        var controller = CreateController();
        controller.ModelState.AddModelError("Error", "Error");

        var model = new UserViewModel
        {
            Forename = "", Surname = "", Email = "incorrectEmail", IsActive = true,
        };

        // Act
        var result = controller.Create(model);

        // Assert
        _userService.Verify(x => x.Create(It.IsAny<User>()), Times.Never);

        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.Model.Should().Be(model);
    }

    [Fact]
    public void Create_Post_WhenUserWithNullDateOfBirthProvided_ShouldCallServiceAndCreateUser()
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

        _userService.Setup(x => x.Create(It.IsAny<User>())).Returns(createdUser);

        // Act
        var result = controller.Create(model);

        // Assert
        _userService.Verify(x => x.Create(It.IsAny<User>()), Times.AtMostOnce);

        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    public void Create_Post_WhenUserWithActiveStatus_ShouldCallServiceAndCreateUser()
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

        _userService.Setup(x => x.Create(It.IsAny<User>())).Returns(createdUser);

        // Act
        var result = controller.Create(model);

        // Assert
        _userService.Verify(x => x.Create(It.IsAny<User>()), Times.AtMostOnce);

        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    public void Create_Post_WhenUserWithInactiveStatus_ShouldCallServiceAndCreateUser()
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

        _userService.Setup(x => x.Create(It.IsAny<User>())).Returns(createdUser);

        // Act
        var result = controller.Create(model);

        // Assert
        _userService.Verify(x => x.Create(It.IsAny<User>()), Times.AtMostOnce);

        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    public void Create_Post_WhenUserWithDateOfBirthProvided_ShouldCallServiceAndCreateUser()
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

        _userService.Setup(x => x.Create(It.IsAny<User>())).Returns(createdUser);

        // Act
        var result = controller.Create(model);

        // Assert
        _userService.Verify(x => x.Create(It.IsAny<User>()), Times.AtMostOnce);

        result.Should().BeOfType<RedirectToActionResult>();
    }

    #endregion

    #region Edit Tests

    [Fact]
    public void Edit_Get_WhenUserExists_ShouldCallService()
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

        _userService.Setup(x => x.GetById(1)).Returns(user);

        // Act
        var result = controller.Edit(1);

        // Assert
        _userService.Verify(x => x.GetById(1), Times.AtMostOnce);
    }

    [Fact]
    public void Edit_Get_WhenUserExists_ShouldReturnViewWithUserViewModel()
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

        _userService.Setup(x => x.GetById(1)).Returns(user);

        // Act
        var result = controller.Edit(1);

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<UserViewModel>().Subject;
        model.Id.Should().Be(1);
        model.Forename.Should().Be(user.Forename);
        model.Surname.Should().Be(user.Surname);
        model.Email.Should().Be(user.Email);
    }

    [Fact]
    public void Edit_Get_WhenUserDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var controller = CreateController();
        _userService.Setup(x => x.GetById(999)).Returns((User?)null);

        // Act
        var result = controller.Edit(999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public void Edit_Post_WhenModelIsValid_ShouldUpdateUserAndRedirectToList()
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

        _userService.Setup(x => x.GetById(1)).Returns(existingUser);
        _userService.Setup(x => x.Update(It.IsAny<User>())).Returns(existingUser);

        // Act
        var result = controller.Edit(1, model);

        // Assert
        var redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
        redirectResult.ActionName.Should().Be(nameof(UsersController.List));
        _userService.Verify(x => x.Update(It.Is<User>(u =>
            u.Id == model.Id &&
            u.Forename == model.Forename &&
            u.Surname == model.Surname &&
            u.Email == model.Email
        )), Times.Once);

        controller.TempData["Message"].Should().NotBeNull();
        controller.TempData["Message"].Should().Be($"UPDATE successful: User {model.Forename} {model.Surname}");
    }

    [Fact]
    public void Edit_Post_WhenIdsDoNotMatch_ShouldReturnBadRequest()
    {
        // Arrange
        var controller = CreateController();
        var model = new UserViewModel { Id = 1 };

        // Act
        var result = controller.Edit(2, model); // ID mismatch

        // Assert
        result.Should().BeOfType<BadRequestResult>();
        _userService.Verify(x => x.Update(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public void Edit_Post_WhenModelIsInvalid_ShouldReturnViewWithModel()
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
        var result = controller.Edit(1, model);

        // Assert
        _userService.Verify(x => x.Update(It.IsAny<User>()), Times.Never);

        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.Model.Should().Be(model);
    }

    [Fact]
    public void Edit_Post_WhenUserDoesNotExist_ShouldReturnNotFound()
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

        _userService.Setup(x => x.GetById(999)).Returns((User?)null);

        // Act
        var result = controller.Edit(999, model);

        // Assert
        _userService.Verify(x => x.Update(It.IsAny<User>()), Times.Never);

        result.Should().BeOfType<NotFoundResult>();
    }

    #endregion

    #region Delete Tests

    [Fact]
    public void Delete_Get_WhenUserExists_ShouldCallService()
    {
        var controller = CreateController();
        var user = new User
        {
            Forename = "Coles",
            Surname = "Law",
            Email = "ShreddedLettuce@example.com",
            IsActive = true
        };

        _userService.Setup(x => x.GetById(1)).Returns(user);

        // Act
        var result = controller.Delete(1);

        // Assert
        _userService.Verify(x => x.GetById(1), Times.AtMostOnce);
    }

    [Fact]
    public void Delete_Get_WhenUserExists_ShouldReturnViewWithUserViewModel()
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

        _userService.Setup(x => x.GetById(1)).Returns(user);

        // Act
        var result = controller.Delete(1);

        // Assert
        _userService.Verify(x => x.GetById(1), Times.AtMostOnce);

        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<UserViewModel>().Subject;

        model.Id.Should().Be(1);
        model.Forename.Should().Be(user.Forename);
        model.Surname.Should().Be(user.Surname);
        model.Email.Should().Be(user.Email);
        model.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Delete_Get_WhenUserDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var controller = CreateController();
        _userService.Setup(x => x.GetById(999)).Returns((User?)null);

        // Act
        var result = controller.Delete(999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public void ConfirmDelete_Post_WhenUserExists_ShouldDeleteUserAndRedirectToList()
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

        _userService.Setup(x => x.Delete(1)).Returns(deletedUser);

        // Act
        var result = controller.ConfirmDelete(1);

        // Assert
        _userService.Verify(x => x.Delete(1), Times.AtMostOnce);

        var redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
        redirectResult.ActionName.Should().Be(nameof(UsersController.List));

        controller.TempData["Message"].Should().NotBeNull();
        controller.TempData["Message"].Should().Be($"DELETE successful: User {deletedUser.Forename} {deletedUser.Surname}");
    }

    [Fact]
    public void ConfirmDelete_Post_WhenUserDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var controller = CreateController();
        _userService.Setup(x => x.Delete(999)).Returns((User?)null);

        // Act
        var result = controller.ConfirmDelete(999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public void ConfirmDelete_Post_WhenInactiveUserProvided_ShouldDeleteUser()
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

        _userService.Setup(x => x.Delete(1)).Returns(user);

        // Act
        var result = controller.ConfirmDelete(1);

        // Assert
        _userService.Verify(x => x.Delete(1), Times.Once);

        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    public void ConfirmDelete_Post_WhenActiveUserProvided_ShouldDeleteUser()
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

        _userService.Setup(x => x.Delete(1)).Returns(user);

        // Act
        var result = controller.ConfirmDelete(1);

        // Assert
        _userService.Verify(x => x.Delete(1), Times.Once);

        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    public void ConfirmDelete_Post_WhenUserWithDateOfBirthProvided_ShouldDeleteUser()
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

        _userService.Setup(x => x.Delete(1)).Returns(user);

        // Act
        var result = controller.ConfirmDelete(1);

        // Assert
        _userService.Verify(x => x.Delete(1), Times.Once);

        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    public void ConfirmDelete_Post_WhenUserWithNullDateOfBirthProvided_ShouldDeleteUser()
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

        _userService.Setup(x => x.Delete(1)).Returns(user);

        // Act
        var result = controller.ConfirmDelete(1);

        // Assert
        _userService.Verify(x => x.Delete(1), Times.Once);

        result.Should().BeOfType<RedirectToActionResult>();
    }

    #endregion

    private User[] SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com",
        bool isActive = true)
    {
        var users = new[] { new User { Forename = forename, Surname = surname, Email = email, IsActive = isActive } };

        _userService
            .Setup(s => s.GetAll())
            .Returns(users);

        _userService.Setup(s => s.FilterByActive(true))
            .Returns(users);

        _userService.Setup(s => s.FilterByActive(false))
            .Returns(users);

        return users;
    }

    private readonly Mock<IUserService> _userService = new();

    private UsersController CreateController()
    {
        var controller = new UsersController(_userService.Object);

        controller.TempData = new TempDataDictionary(
            new DefaultHttpContext(),
            Mock.Of<ITempDataProvider>());

        return controller;
    }
}
