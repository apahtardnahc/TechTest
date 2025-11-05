using System;
using System.Linq;
using UserManagement.Models;
using UserManagement.Repository.Interfaces;
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

    #region FilterByActive tests




    [Fact]
    public void WhenNoUsers_FilterByActive_WhenTrue_ShouldReturnNoUsers()
    {

        // Arrange
        var service = CreateService();
        var users = System.Array.Empty<User>().AsQueryable();

        _userRepository
            .Setup(x => x.GetByActiveStatus(true))
            .Returns(users);

        // Act
        var result = service.FilterByActive(true);

        // Assert
        result.Should().HaveCount(0);
    }

    [Fact]
    public void FilterByActive_WhenTrue_ShouldReturnOnlyActiveUsers()
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
        }.AsQueryable();

        _userRepository
            .Setup(x => x.GetByActiveStatus(true))
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

        _userRepository
            .Setup(x => x.GetByActiveStatus(false))
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
        }.AsQueryable();

        _userRepository
            .Setup(x => x.GetByActiveStatus(false))
            .Returns(users);

        // Act
        var result = service.FilterByActive(false);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(user => user.IsActive == false);
    }

    [Fact]
    public void FilterByActive_WhenTrue_CallsRepository()
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
        }.AsQueryable();

        _userRepository
            .Setup(x => x.GetByActiveStatus(true))
            .Returns(users);

        // Act
        var result = service.FilterByActive(true);

        // Assert
        _userRepository.Verify(x => x.GetByActiveStatus(true), Times.AtMostOnce);
        result.Should().Equal(users);
    }

    [Fact]
    public void FilterByActive_WhenFalse_CallsRepository()
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
        }.AsQueryable();

        _userRepository
            .Setup(x => x.GetByActiveStatus(false))
            .Returns(users);

        // Act
        var result = service.FilterByActive(false);

        // Assert
        _userRepository.Verify(x => x.GetByActiveStatus(false), Times.AtMostOnce);
        result.Should().Equal(users);
    }

    #endregion

    #region GetById Tests

    [Fact]
    public void GetById_WhenUserExists_ShouldReturnUser()
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

        _userRepository.Setup(x => x.GetById(1)).Returns(user);

        // Act
        var result = service.GetById(1);

        // Assert
        _userRepository.Verify(x => x.GetById(1), Times.Once);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(user);
    }

    [Fact]
    public void GetById_WhenUserDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var service = CreateService();
        _userRepository
            .Setup(x => x.GetById(999))
            .Returns((User?)null);

        // Act
        var result = service.GetById(999);

        // Assert
        _userRepository.Verify(x => x.GetById(999), Times.AtMostOnce);
        result.Should().BeNull();
    }

    [Fact]
    public void GetById_WhenInactiveUserExists_ShouldReturnInactiveUser()
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

        _userRepository.Setup(x => x.GetById(1)).Returns(user);

        // Act
        var result = service.GetById(1);

        // Assert
        _userRepository.Verify(x => x.GetById(1), Times.Once);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(user);
        result.IsActive.Should().BeFalse();
    }

    [Fact]
    public void GetById_WhenActiveUserExists_ShouldReturnActiveUser()
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

        _userRepository.Setup(x => x.GetById(1)).Returns(user);

        // Act
        var result = service.GetById(1);

        // Assert
        _userRepository.Verify(x => x.GetById(1), Times.Once);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(user);
        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public void GetById_WhenUserWithNullDateOfBrith_ShouldReturnUserWithNullDateOfBirth()
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

        _userRepository.Setup(x => x.GetById(1)).Returns(user);

        // Act
        var result = service.GetById(1);

        // Assert
        _userRepository.Verify(x => x.GetById(1), Times.Once);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(user);
        result.DateOfBirth.Should().BeNull();
    }

    [Fact]
    public void GetById_WhenUserWithDateOfBirth_ShouldReturnUserWithDateOfBirth()
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

        _userRepository.Setup(x => x.GetById(1)).Returns(user);

        // Act
        var result = service.GetById(1);

        // Assert
        _userRepository.Verify(x => x.GetById(1), Times.Once);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(user);
        result.DateOfBirth.Should().NotBeNull();
    }

    #endregion

    #region Create Tests

    [Fact]
    public void Create_WhenValidUserProvided_ShouldCallRepository()
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
            .Setup(x => x.Create(newUser))
            .Returns(createdUser);

        // Act
        var result = service.Create(newUser);

        // Assert
        _userRepository.Verify(x => x.Create(newUser), Times.AtMostOnce);

        result.Should().NotBeNull();
    }

    [Fact]
    public void Create_WhenValidUserProvided_ShouldCallRepositoryAndReturnUser()
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
            .Setup(x => x.Create(newUser))
            .Returns(createdUser);

        // Act
        var result = service.Create(newUser);

        // Assert
        _userRepository.Verify(x => x.Create(newUser), Times.AtMostOnce);

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
    public void Create_WhenActiveUserProvided_ShouldCreateAndReturnActiveUser(bool isActive)
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
            .Setup(x => x.Create(newUser))
            .Returns(createdUser);

        // Act
        var result = service.Create(newUser);

        // Assert
        _userRepository.Verify(x => x.Create(newUser), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Forename.Should().Be(newUser.Forename);
        result.Surname.Should().Be(newUser.Surname);
        result.Email.Should().Be(newUser.Email);
        result.IsActive.Should().Be(newUser.IsActive);
        result.DateOfBirth.Should().BeNull();
    }

    [Fact]
    public void Create_WhenUserWithNullDateOfBirth_ShouldCreateAndReturnUser()
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
            .Setup(x => x.Create(newUser))
            .Returns(createdUser);

        // Act
        var result = service.Create(newUser);

        // Assert
        _userRepository.Verify(x => x.Create(newUser), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Forename.Should().Be(newUser.Forename);
        result.Surname.Should().Be(newUser.Surname);
        result.Email.Should().Be(newUser.Email);
        result.IsActive.Should().Be(newUser.IsActive);
        result.DateOfBirth.Should().BeNull();
    }

    #endregion

    #region Update Tests

    [Fact]
    public void Update_WhenValidUserProvided_ShouldCallRepository()
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
            .Setup(x => x.Update(updatedUser))
            .Returns(updatedUser);

        // Act
        var result = service.Update(updatedUser);

        // Assert
        _userRepository.Verify(x => x.Update(updatedUser), Times.AtMostOnce);
    }

    [Fact]
    public void Update_WhenValidUserProvided_ShouldCallRepositoryAndReturnUpdatedUser()
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
            .Setup(x => x.Update(updatedUser))
            .Returns(updatedUser);

        // Act
        var result = service.Update(updatedUser);

        // Assert
        _userRepository.Verify(x => x.Update(updatedUser), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(updatedUser);
    }

    [Fact]
    public void Update_WhenUserActiveStatusChanged_ShouldUpdateSuccessfully()
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
            .Setup(x => x.Update(user))
            .Returns(user);

        // Act
        var result = service.Update(user);

        // Assert
        _userRepository.Verify(x => x.Update(user), Times.Once);

        result.Should().NotBeNull();
        result.IsActive.Should().BeFalse();

    }

    [Fact]
    public void Update_WhenUserInactiveStatusChanged_ShouldUpdateSuccessfully()
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
            .Setup(x => x.Update(user))
            .Returns(user);

        // Act
        var result = service.Update(user);

        // Assert
        _userRepository.Verify(x => x.Update(user), Times.Once);

        result.Should().NotBeNull();
        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Update_WhenUserDateOfBirthChangedToNull_ShouldUpdateSuccessfully()
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
            .Setup(x => x.Update(user))
            .Returns(user);

        // Act
        var result = service.Update(user);

        // Assert
        _userRepository.Verify(x => x.Update(user), Times.Once);

        result.Should().NotBeNull();
        result.DateOfBirth.Should().BeNull();
    }

    #endregion

    #region Delete Tests

    [Fact]
    public void Delete_WhenUserExists_ShouldCallRepository()
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

        _userRepository.Setup(x => x.Delete(1)).Returns(user);

        // Act
        var result = service.Delete(1);

        // Assert
        _userRepository.Verify(x => x.Delete(1), Times.AtMostOnce);
    }

    [Fact]
    public void Delete_WhenUserExists_ShouldCallRepositoryAndReturnUser()
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

        _userRepository.Setup(x => x.Delete(1)).Returns(user);

        // Act
        var result = service.Delete(1);

        // Assert
        _userRepository.Verify(x => x.Delete(1), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(user);
    }

    [Fact]
    public void Delete_WhenUserDoesNotExists_ShouldCallRepositoryAndReturnNull()
    {
        var service = CreateService();

        _userRepository.Setup(x => x.Delete(1)).Returns((User?)null);

        // Act
        var result = service.Delete(1);

        // Assert
        _userRepository.Verify(x => x.Delete(1), Times.AtMostOnce);

        result.Should().BeNull();
    }

    [Fact]
    public void Delete_WhenActiveUserExists_ShouldDeleteActiveUser()
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

        _userRepository.Setup(x => x.Delete(1)).Returns(user);

        // Act
        var result = service.Delete(1);

        // Assert
        _userRepository.Verify(x => x.Delete(1), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Delete_WhenInactiveUserExists_ShouldDeleteInactiveUser()
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

        _userRepository.Setup(x => x.Delete(1)).Returns(user);

        // Act
        var result = service.Delete(1);

        // Assert
        _userRepository.Verify(x => x.Delete(1), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.IsActive.Should().BeFalse();
    }

    #endregion

    private IQueryable<User> SetupUsers(string forename = "Johnny", string surname = "User",
        string email = "juser@example.com", bool isActive = true)
    {
        var users = new[]
        {
            new User
            {
                Forename = forename,
                Surname = surname,
                Email = email,
                IsActive = isActive,
                DateOfBirth = new DateTime(2000, 1, 1)
            }
        }.AsQueryable();

        _userRepository
            .Setup(s => s.GetAll())
            .Returns(users);

        return users;
    }

    private readonly Mock<IUserRepository> _userRepository = new();
    private UserService CreateService() => new(_userRepository.Object);
}
