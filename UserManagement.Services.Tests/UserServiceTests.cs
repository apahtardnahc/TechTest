using System;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;

namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    [Fact]
    public void GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetAll();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeSameAs(users);
    }

    [Fact]
    public void WhenNoUsers_FilterByActive_WhenTrue_ShouldReturnNoUsers()
    {

        // Arrange
        var service = CreateService();
        var users = System.Array.Empty<User>().AsQueryable();

        _dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(users);

        // Act
        var result = service.FilterByActive(true);

        // Assert
        result.Should().HaveCount(0);
    }

    [Fact]

    // With the filter on there should be normal users too
    public void FilterByActive_WhenTrue_ShouldReturnOnlyActiveUsers()
    {
        // Arrange
        var service = CreateService();
        var users = new[]
        {
              new User {Forename = "ActiveUserForename1", Surname = "ActiveUserSurname1", DateOfBirth = new DateTime(2000, 1, 1), Email= "ActiveUserEmail1@example.com", IsActive = true},
              new User {Forename = "ActiveUserForename2", Surname = "ActiveUserSurname2", DateOfBirth = new DateTime(2000, 2, 1), Email= "ActiveUserEmail2@example.com", IsActive = true}
        }.AsQueryable();

        _dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(users);

        // Act
        var result = service.FilterByActive(true);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(user => user.IsActive);

    }

    [Fact]
    public void FilterByActive_WithMixedUsers_WhenTrue_ShouldReturnOnlyActiveUsers()
    {
        // Arrange
        var service = CreateService();
        var users = new[]
       {
              new User {Forename = "InactiveUserForename1", Surname = "InactiveUserSurname1", DateOfBirth = new DateTime(2000, 1, 1), Email= "InactiveUserEmail1@example.com", IsActive = false},
              new User {Forename = "InactiveUserForename2", Surname = "InactiveUserSurname2", DateOfBirth = new DateTime(2000, 2, 2), Email= "InactiveUserEmail2@example.com", IsActive = false},
              new User {Forename = "ActiveUserForename1", Surname = "ActiveUserSurname1", DateOfBirth = new DateTime(2000, 3, 3), Email= "ActiveUserEmail1@example.com", IsActive = true},
              new User {Forename = "ActiveUserForename2", Surname = "ActiveUserSurname2", DateOfBirth = new DateTime(2000, 4, 4), Email= "ActiveUserEmail2@example.com", IsActive = true}
        }.AsQueryable();

        _dataContext
        .Setup(s => s.GetAll<User>())
        .Returns(users);

        // Act
        var result = service.FilterByActive(true);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(user => user.IsActive);
    }

    [Fact]
    public void WhenNoUsers_FilterByActive_WhenFalse_ShouldReturnNoUsers()
    {

        // Arrange
        var service = CreateService();
        var users = System.Array.Empty<User>().AsQueryable();

        _dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(users);

        // Act
        var result = service.FilterByActive(false);

        // Assert
        result.Should().HaveCount(0);
    }

    [Fact]
    public void FilterByActive_WhenFalse_ShouldReturnOnlyInactiveUsers()
    {
        // Arrange
        var service = CreateService();
        var users = new[]
        {
              new User {Forename = "InactiveUserForename1", Surname = "InactiveUserSurname1", DateOfBirth = new DateTime(2000, 1, 1), Email= "InactiveUserEmail1@example.com", IsActive = false},
              new User {Forename = "InactiveUserForename2", Surname = "InactiveUserSurname2", DateOfBirth = new DateTime(2000, 1, 2), Email= "InactiveUserEmail2@example.com", IsActive = false},

        }.AsQueryable();

        _dataContext
           .Setup(s => s.GetAll<User>())
           .Returns(users);

        // Act
        var result = service.FilterByActive(false);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(user => user.IsActive == false);
    }

    [Fact]
    public void FilterByActive_WithMixedUsers_WhenFalse_ShouldReturnOnlyActiveUsers()
    {
        // Arrange
        var service = CreateService();
        var users = new[]
      {
              new User {Forename = "InactiveUserForename1", Surname = "InactiveUserSurname1", DateOfBirth = new DateTime(2000, 1, 1), Email= "InactiveUserEmail1@example.com", IsActive = false},
              new User {Forename = "InactiveUserForename2", Surname = "InactiveUserSurname2", DateOfBirth = new DateTime(2000, 2, 2), Email= "InactiveUserEmail2@example.com", IsActive = false},
              new User {Forename = "ActiveUserForename1", Surname = "ActiveUserSurname1", DateOfBirth = new DateTime(2000, 3, 3), Email= "ActiveUserEmail1@example.com", IsActive = true},
              new User {Forename = "ActiveUserForename2", Surname = "ActiveUserSurname2", DateOfBirth = new DateTime(2000, 4, 4), Email= "ActiveUserEmail2@example.com", IsActive = true}
        }.AsQueryable();

        _dataContext
       .Setup(s => s.GetAll<User>())
       .Returns(users);

        // Act
        var result = service.FilterByActive(false);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(user => user.IsActive == false);
    }

    private IQueryable<User> SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true)
    {
        var users = new[]
        {
            new User
            {
                Forename = forename,
                Surname = surname,
                Email = email,
                IsActive = isActive,
                DateOfBirth = new DateTime(2000, 1,1)
            }
        }.AsQueryable();

        _dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(users);

        return users;
    }

    private readonly Mock<IDataContext> _dataContext = new();
    private UserService CreateService() => new(_dataContext.Object);


}
