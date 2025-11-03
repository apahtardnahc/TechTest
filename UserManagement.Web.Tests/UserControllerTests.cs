using System;
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

    //[Fact]
    //public void Active_WithMixedUsers_WhenServiceCalled_ShouldOnlyReturnActiveUsers_And_ModelMustContainOnlyActiveUsers()
    //{
    //    // Arrange
    //    var controller = CreateController();
    //    var misedUsers = new[]
    //   {
    //          new User {Forename = "InactiveUserForename1", Surname = "InactiveUserSurname1", Email= "InactiveUserEmail1@example.com", IsActive = false},
    //          new User {Forename = "InactiveUserForename2", Surname = "InactiveUserSurname2", Email= "InactiveUserEmail2@example.com", IsActive = false},
    //          new User {Forename = "ActiveUserForename1", Surname = "ActiveUserSurname1", Email= "ActiveUserEmail1@example.com", IsActive = true},
    //          new User {Forename = "ActiveUserForename2", Surname = "ActiveUserSurname2", Email= "ActiveUserEmail2@example.com", IsActive = true}
    //    }.AsQueryable();

    //    var 

    //}

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
               Forename = "Johnny",
               Surname = "Test",
               Email = "JohnnyTest@example.com",
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
    public void List_WhenUserHasNullDateOfBirth_ModelShouldHaveNullDateOfBirth()
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

    private User[] SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true)
    {
        var users = new[]
        {
            new User
            {
                Forename = forename,
                Surname = surname,
                Email = email,
                IsActive = isActive
            }
        };

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
    private UsersController CreateController() => new(_userService.Object);
}
