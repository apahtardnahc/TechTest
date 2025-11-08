using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using UserManagement.Models;
using UserManagement.Repository.Interfaces;
using UserManagement.Services.Domain.Implementations;

namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepository = new();
    private UserService CreateService() => new(_userRepository.Object);

    [Fact]
    public async Task GetAllAsync_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await service.GetAllAsync();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeSameAs(users);
    }

    #region FilterByActiveAsync tests

    [Fact]
    public async Task WhenNoUsers_FilterByActiveAsync_WhenTrue_ShouldReturnNoUsers()
    {

        // Arrange
        var service = CreateService();
        var users = System.Array.Empty<User>().ToList();

        _userRepository
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(users);

        _userRepository
            .Setup(x => x.GetByActiveStatusAsync(true))
            .ReturnsAsync(users);

        // Act
        var result = await service.FilterByActiveAsync(true);

        // Assert
        result.Should().HaveCount(0);
    }

    [Fact]
    public async Task FilterByActiveAsync_WhenTrue_ShouldReturnOnlyActiveUsers()
    {
        // Arrange
        var service = CreateService();
        var users = new[]
        {
            new User
            {
                Forename = "ActiveUserForename1",
                Surname = "ActiveUserSurname1",
                DateOfBirth = new DateTime(2000, 1, 1),
                Email = "ActiveUserEmail1@example.com",
                IsActive = true
            },
            new User
            {
                Forename = "ActiveUserForename2",
                Surname = "ActiveUserSurname2",
                DateOfBirth = new DateTime(2000, 2, 1),
                Email = "ActiveUserEmail2@example.com",
                IsActive = true
            }
        }.ToList();

        _userRepository
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(users);

        _userRepository
            .Setup(x => x.GetByActiveStatusAsync(true))
            .ReturnsAsync(users);

        // Act
        var result = await service.FilterByActiveAsync(true);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(user => user.IsActive);

    }

    [Fact]
    public async Task WhenNoUsers_FilterByActiveAsync_WhenFalse_ShouldReturnNoUsers()
    {

        // Arrange
        var service = CreateService();
        var users = System.Array.Empty<User>().ToList();


        _userRepository
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(users);

        _userRepository
            .Setup(x => x.GetByActiveStatusAsync(false))
            .ReturnsAsync(users);

        // Act
        var result = await service.FilterByActiveAsync(false);

        // Assert
        result.Should().HaveCount(0);
    }

    [Fact]
    public async Task FilterByActiveAsync_WhenFalse_ShouldReturnOnlyInactiveUsers()
    {
        // Arrange
        var service = CreateService();
        var users = new[]
        {
            new User
            {
                Forename = "InactiveUserForename1",
                Surname = "InactiveUserSurname1",
                DateOfBirth = new DateTime(2000, 1, 1),
                Email = "InactiveUserEmail1@example.com",
                IsActive = false
            },
            new User
            {
                Forename = "InactiveUserForename2",
                Surname = "InactiveUserSurname2",
                DateOfBirth = new DateTime(2000, 1, 2),
                Email = "InactiveUserEmail2@example.com",
                IsActive = false
            },
        }.ToList();

        _userRepository
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(users);

        _userRepository
            .Setup(x => x.GetByActiveStatusAsync(false))
            .ReturnsAsync(users);

        // Act
        var result = await service.FilterByActiveAsync(false);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(user => user.IsActive == false);
    }

    [Fact]
    public async Task FilterByActiveAsync_WhenTrue_CallsRepository()
    {
        // Arrange
        var service = CreateService();
        var users = new[]
        {
            new User
            {
                Forename = "ActiveUserForename1",
                Surname = "ActiveUserSurname1",
                DateOfBirth = new DateTime(2000, 1, 1),
                Email = "ActiveUserEmail1@example.com",
                IsActive = true
            },
            new User
            {
                Forename = "ActiveUserForename2",
                Surname = "ActiveUserSurname2",
                DateOfBirth = new DateTime(2000, 2, 1),
                Email = "ActiveUserEmail2@example.com",
                IsActive = true
            }
        }.ToList();

        _userRepository
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(users);

        _userRepository
            .Setup(x => x.GetByActiveStatusAsync(true))
            .ReturnsAsync(users);

        // Act
        var result = await service.FilterByActiveAsync(true);

        // Assert
        _userRepository.Verify(x => x.GetByActiveStatusAsync(true), Times.AtMostOnce);
        result.Should().Equal(users);
    }

    [Fact]
    public async Task FilterByActiveAsync_WhenFalse_CallsRepository()
    {
        // Arrange
        var service = CreateService();
        var users = new[]
        {
            new User
            {
                Forename = "InactiveUserForename1",
                Surname = "InactiveUserSurname1",
                DateOfBirth = new DateTime(2000, 1, 1),
                Email = "InactiveUserEmail1@example.com",
                IsActive = false
            },
            new User
            {
                Forename = "InactiveUserForename2",
                Surname = "InactiveUserSurname2",
                DateOfBirth = new DateTime(2000, 1, 2),
                Email = "InactiveUserEmail2@example.com",
                IsActive = false
            },
        }.ToList();

        _userRepository
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(users);

        _userRepository
            .Setup(x => x.GetByActiveStatusAsync(false))
            .ReturnsAsync(users);

        // Act
        var result = await service.FilterByActiveAsync(false);

        // Assert
        _userRepository.Verify(x => x.GetByActiveStatusAsync(false), Times.AtMostOnce);
        result.Should().Equal(users);
    }

    #endregion

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_WhenUserExists_ShouldReturnUser()
    {
        // Arrange
        var service = CreateService();
        var user = new User
        {
            Id = 1,
            Forename = "InactiveUserForename1",
            Surname = "InactiveUserSurname1",
            DateOfBirth = new DateTime(2000, 1, 1),
            Email = "InactiveUserEmail1@example.com",
            IsActive = false
        };

        _userRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);

        // Act
        var result = await service.GetByIdAsync(1);

        // Assert
        _userRepository.Verify(x => x.GetByIdAsync(1), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task GetByIdAsync_WhenUserDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var service = CreateService();
        _userRepository
            .Setup(x => x.GetByIdAsync(999))
            .ReturnsAsync((User?)null);

        // Act
        var result = await service.GetByIdAsync(999);

        // Assert
        _userRepository.Verify(x => x.GetByIdAsync(999), Times.AtMostOnce);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_WhenInactiveUserExists_ShouldReturnInactiveUser()
    {
        // Arrange
        var service = CreateService();
        var user = new User
        {
            Id = 1,
            Forename = "InactiveUserForename1",
            Surname = "InactiveUserSurname1",
            DateOfBirth = new DateTime(2000, 1, 1),
            Email = "InactiveUserEmail1@example.com",
            IsActive = false
        };

        _userRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);

        // Act
        var result = await service.GetByIdAsync(1);

        // Assert
        _userRepository.Verify(x => x.GetByIdAsync(1), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(user);
        result.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdAsync_WhenActiveUserExists_ShouldReturnActiveUser()
    {
        // Arrange
        var service = CreateService();
        var user = new User
        {
            Id = 1,
            Forename = "ActiveForename",
            Surname = "ActiveSurname",
            DateOfBirth = new DateTime(2000, 1, 1),
            Email = "ActiveEmail@example.com",
            IsActive = true
        };

        _userRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);

        // Act
        var result = await service.GetByIdAsync(1);

        // Assert
        _userRepository.Verify(x => x.GetByIdAsync(1), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(user);
        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task GetByIdAsync_WhenUserWithNullDateOfBrith_ShouldReturnUserWithNullDateOfBirth()
    {
        // Arrange
        var service = CreateService();
        var user = new User
        {
            Id = 1,
            Forename = "ActiveForename",
            Surname = "ActiveSurname",
            DateOfBirth = null,
            Email = "ActiveEmail@example.com",
            IsActive = true
        };

        _userRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);

        // Act
        var result = await service.GetByIdAsync(1);

        // Assert
        _userRepository.Verify(x => x.GetByIdAsync(1), Times.Once);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(user);
        result.DateOfBirth.Should().BeNull();
    }

    [Fact]
    public async Task GetById_WhenUserWithDateOfBirth_ShouldReturnUserWithDateOfBirth()
    {
        // Arrange
        var service = CreateService();
        var user = new User
        {
            Id = 1,
            Forename = "ActiveForename",
            Surname = "ActiveSurname",
            DateOfBirth = new DateTime(2000, 1, 1),
            Email = "ActiveEmail@example.com",
            IsActive = true
        };

        _userRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);

        // Act
        var result = await service.GetByIdAsync(1);

        // Assert
        _userRepository.Verify(x => x.GetByIdAsync(1), Times.Once);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(user);
        result.DateOfBirth.Should().NotBeNull();
    }

    #endregion

    #region CreateAsync Tests

    [Fact]
    public async Task CreateAsync_WhenValidUserProvided_ShouldCallRepository()
    {
        // Arrange
        var service = CreateService();
        var newUser = new User
        {
            Forename = "Janet",
            Surname = "Mortimer",
            Email = "jM@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(1995, 5, 15)
        };

        var createdUser = new User
        {
            Id = 1,
            Forename = newUser.Forename,
            Surname = newUser.Surname,
            Email = newUser.Email,
            IsActive = newUser.IsActive,
            DateOfBirth = newUser.DateOfBirth
        };

        _userRepository
            .Setup(x => x.CreateAsync(newUser))
            .ReturnsAsync(createdUser);

        // Act
        var result = await service.CreateAsync(newUser);

        // Assert
        _userRepository.Verify(x => x.CreateAsync(newUser), Times.AtMostOnce);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateAsync_WhenValidUserProvided_ShouldCallRepositoryAndReturnUser()
    {
        // Arrange
        var service = CreateService();
        var newUser = new User
        {
            Forename = "Janet",
            Surname = "Mortimer",
            Email = "jM@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(1995, 5, 15)
        };

        var createdUser = new User
        {
            Id = 1,
            Forename = newUser.Forename,
            Surname = newUser.Surname,
            Email = newUser.Email,
            IsActive = newUser.IsActive,
            DateOfBirth = newUser.DateOfBirth
        };

        _userRepository
            .Setup(x => x.CreateAsync(newUser))
            .ReturnsAsync(createdUser);

        // Act
        var result = await service.CreateAsync(newUser);

        // Assert
        _userRepository.Verify(x => x.CreateAsync(newUser), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Forename.Should().Be(newUser.Forename);
        result.Surname.Should().Be(newUser.Surname);
        result.Email.Should().Be(newUser.Email);
        result.IsActive.Should().Be(newUser.IsActive);
        result.DateOfBirth.Should().Be(newUser.DateOfBirth);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task CreateAsync_WhenActiveUserProvided_ShouldCreateAndReturnActiveUser(bool isActive)
    {
        // Arrange
        var service = CreateService();
        var newUser = new User
        {
            Forename = "Janet",
            Surname = "Mortimer",
            Email = "jM@example.com",
            IsActive = isActive,
            DateOfBirth = null
        };

        var createdUser = new User
        {
            Id = 1,
            Forename = newUser.Forename,
            Surname = newUser.Surname,
            Email = newUser.Email,
            IsActive = newUser.IsActive,
            DateOfBirth = newUser.DateOfBirth
        };

        _userRepository
            .Setup(x => x.CreateAsync(newUser))
            .ReturnsAsync(createdUser);

        // Act
        var result = await service.CreateAsync(newUser);

        // Assert
        _userRepository.Verify(x => x.CreateAsync(newUser), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Forename.Should().Be(newUser.Forename);
        result.Surname.Should().Be(newUser.Surname);
        result.Email.Should().Be(newUser.Email);
        result.IsActive.Should().Be(newUser.IsActive);
        result.DateOfBirth.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_WhenUserWithNullDateOfBirth_ShouldCreateAndReturnUser()
    {
        // Arrange
        var service = CreateService();
        var newUser = new User
        {
            Forename = "Janet",
            Surname = "Mortimer",
            Email = "jM@example.com",
            IsActive = true,
            DateOfBirth = null
        };

        var createdUser = new User
        {
            Id = 1,
            Forename = newUser.Forename,
            Surname = newUser.Surname,
            Email = newUser.Email,
            IsActive = newUser.IsActive,
            DateOfBirth = newUser.DateOfBirth
        };

        _userRepository
            .Setup(x => x.CreateAsync(newUser))
            .ReturnsAsync(createdUser);

        // Act
        var result = await service.CreateAsync(newUser);

        // Assert
        _userRepository.Verify(x => x.CreateAsync(newUser), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Forename.Should().Be(newUser.Forename);
        result.Surname.Should().Be(newUser.Surname);
        result.Email.Should().Be(newUser.Email);
        result.IsActive.Should().Be(newUser.IsActive);
        result.DateOfBirth.Should().BeNull();
    }

    #endregion

    #region UpdateAsync Tests

    [Fact]
    public async Task UpdateAsync_WhenValidUserProvided_ShouldCallRepository()
    {
        // Arrange
        var service = CreateService();
        var updatedUser = new User
        {
            Id = 1,
            Forename = "UpdatedForename",
            Surname = "UpdatedSurname",
            Email = "updated@example.com",
            IsActive = false,
            DateOfBirth = new DateTime(2000, 10, 10)
        };

        _userRepository
            .Setup(x => x.UpdateAsync(updatedUser))
            .ReturnsAsync(updatedUser);

        // Act
        var result = await service.UpdateAsync(updatedUser);

        // Assert
        _userRepository.Verify(x => x.UpdateAsync(updatedUser), Times.AtMostOnce);
    }

    [Fact]
    public async Task UpdateAsync_WhenValidUserProvided_ShouldCallRepositoryAndReturnUpdatedUser()
    {
        // Arrange
        var service = CreateService();
        var updatedUser = new User
        {
            Id = 1,
            Forename = "UpdatedForename",
            Surname = "UpdatedSurname",
            Email = "updated@example.com",
            IsActive = false,
            DateOfBirth = new DateTime(2000, 10, 10)
        };

        _userRepository
            .Setup(x => x.UpdateAsync(updatedUser))
            .ReturnsAsync(updatedUser);

        // Act
        var result = await service.UpdateAsync(updatedUser);

        // Assert
        _userRepository.Verify(x => x.UpdateAsync(updatedUser), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(updatedUser);
    }

    [Fact]
    public async Task UpdateAsync_WhenUserActiveStatusChanged_ShouldUpdateSuccessfully()
    {
        // Arrange
        var service = CreateService();
        var user = new User
        {
            Id = 1,
            Forename = "J",
            Surname = "Lo",
            Email = "jLo@example.com",
            IsActive = false,
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        _userRepository
            .Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(user);

        // Act
        var result = await service.UpdateAsync(user);

        // Assert
        _userRepository.Verify(x => x.UpdateAsync(user), Times.Once);

        result.Should().NotBeNull();
        result.IsActive.Should().BeFalse();

    }

    [Fact]
    public async Task UpdateAsync_WhenUserInactiveStatusChanged_ShouldUpdateSuccessfully()
    {
        // Arrange
        var service = CreateService();
        var user = new User
        {
            Id = 1,
            Forename = "J",
            Surname = "Lo",
            Email = "jLo@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        _userRepository
            .Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(user);

        // Act
        var result = await service.UpdateAsync(user);

        // Assert
        _userRepository.Verify(x => x.UpdateAsync(user), Times.Once);

        result.Should().NotBeNull();
        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_WhenUserDateOfBirthChangedToNull_ShouldUpdateSuccessfully()
    {
        // Arrange
        var service = CreateService();
        var user = new User
        {
            Id = 1,
            Forename = "J",
            Surname = "Lo",
            Email = "jLo@example.com",
            IsActive = true,
            DateOfBirth = null
        };

        _userRepository
            .Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(user);

        // Act
        var result = await service.UpdateAsync(user);

        // Assert
        _userRepository.Verify(x => x.UpdateAsync(user), Times.Once);

        result.Should().NotBeNull();
        result.DateOfBirth.Should().BeNull();
    }

    #endregion

    #region DeleteAsync Tests

    [Fact]
    public async Task DeleteAsync_WhenUserExists_ShouldCallRepository()
    {
        var service = CreateService();
        var user = new User
        {
            Id = 1,
            Forename = "Jaffa",
            Surname = "Cakes",
            Email = "orangeBiscuits@example.com",
            IsActive = true,
            DateOfBirth = null
        };

        _userRepository.Setup(x => x.DeleteAsync(1)).ReturnsAsync(user);

        // Act
        var result = await service.DeleteAsync(1);

        // Assert
        _userRepository.Verify(x => x.DeleteAsync(1), Times.AtMostOnce);
    }

    [Fact]
    public async Task DeleteAsync_WhenUserExists_ShouldCallRepositoryAndReturnUser()
    {
        var service = CreateService();
        var user = new User
        {
            Id = 1,
            Forename = "Jaffa",
            Surname = "Cakes",
            Email = "orangeBiscuits@example.com",
            IsActive = true,
            DateOfBirth = null
        };

        _userRepository.Setup(x => x.DeleteAsync(1)).ReturnsAsync(user);

        // Act
        var result = await service.DeleteAsync(1);

        // Assert
        _userRepository.Verify(x => x.DeleteAsync(1), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task DeleteAsync_WhenUserDoesNotExists_ShouldCallRepositoryAndReturnNull()
    {
        var service = CreateService();

        _userRepository.Setup(x => x.DeleteAsync(1)).ReturnsAsync((User?)null);

        // Act
        var result = await service.DeleteAsync(1);

        // Assert
        _userRepository.Verify(x => x.DeleteAsync(1), Times.AtMostOnce);

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WhenActiveUserExists_ShouldDeleteActiveUser()
    {
        var service = CreateService();
        var user = new User
        {
            Id = 1,
            Forename = "Jaffa",
            Surname = "Cakes",
            Email = "orangeBiscuits@example.com",
            IsActive = true,
            DateOfBirth = null
        };

        _userRepository.Setup(x => x.DeleteAsync(1)).ReturnsAsync(user);

        // Act
        var result = await service.DeleteAsync(1);

        // Assert
        _userRepository.Verify(x => x.DeleteAsync(1), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_WhenInactiveUserExists_ShouldDeleteInactiveUser()
    {
        var service = CreateService();
        var user = new User
        {
            Id = 1,
            Forename = "Rich",
            Surname = "Tea",
            Email = "teaBiscuits@example.com",
            IsActive = false,
            DateOfBirth = null
        };

        _userRepository.Setup(x => x.DeleteAsync(1)).ReturnsAsync(user);

        // Act
        var result = await service.DeleteAsync(1);

        // Assert
        _userRepository.Verify(x => x.DeleteAsync(1), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.IsActive.Should().BeFalse();
    }

    #endregion

    private List<User> SetupUsers(string forename = "Johnny", string surname = "User",
        string email = "juser@example.com", bool isActive = true)
    {
        var users = new List<User>
        {
            new User
            {
                Forename = forename,
                Surname = surname,
                Email = email,
                IsActive = isActive,
                DateOfBirth = new DateTime(2000, 1, 1)
            }
        };

        _userRepository
            .Setup(s => s.GetAllAsync())
            .ReturnsAsync(users);

        return users;
    }
}
