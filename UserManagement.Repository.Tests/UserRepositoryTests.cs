using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Repository.Implementations;

namespace UserManagement.Repository.Tests;

public class UserRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnAnEmptyCollection_WhenNoUsers()
    {
        // Arrange
        var mockDataContext = new Mock<IDataContext>();
        mockDataContext.Setup(x => x.GetAllAsync<User>())
                .ReturnsAsync(new List<User>());

        var repository = new UserRepository(mockDataContext.Object);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        result.AsEnumerable().Should().NotBeNull();
        result.AsEnumerable().Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_WhenCalled_ShouldReturnAllUsers()
    {
        // Arrange
        var mockDataContext = new Mock<IDataContext>();

        var users = new List<User>
        {
            new User
            {
                Forename = "John",
                Surname = "Cena",
                Email = "CantSeeMe@example.com",
                IsActive = true,
                DateOfBirth = new DateTime(2000, 1, 1)
            },
            new User
            {
                Forename = "Under",
                Surname = "Taker",
                Email = "Tombstone@example.com",
                IsActive = true,
                DateOfBirth = new DateTime(2000, 1, 1)
            },
        };

        mockDataContext.Setup(x => x.GetAllAsync<User>())
            .ReturnsAsync(users);

        var repository = new UserRepository(mockDataContext.Object);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(users);
    }

    [Fact]
    public async Task GetByActiveStatusAsync_WhenCalledWithTrue_ShouldReturnActiveUsers()
    {
        // Arrange
        var mockDataContext = new Mock<IDataContext>();

        var users = new List<User>
        {
            new User
            {
                Forename = "John",
                Surname = "Cena",
                Email = "CantSeeMe@example.com",
                IsActive = true,
                DateOfBirth = new DateTime(2000, 1, 1)
            },
        };

        mockDataContext.Setup(x => x.GetAllAsync<User>()).ReturnsAsync(users);
        var repository = new UserRepository(mockDataContext.Object);

        // Act
        var result = await repository.GetByActiveStatusAsync(true);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task GetByActiveStatusAsync_WhenCalledWithFalse_ShouldReturnInactiveUsers()
    {
        // Arrange
        var mockDataContext = new Mock<IDataContext>();

        var users = new List<User>
        {
            new User
            {
                Forename = "Under",
                Surname = "Taker",
                Email = "Tombstone@example.com",
                IsActive = false,
                DateOfBirth = new DateTime(2000, 1, 1)
            },
        };

        mockDataContext.Setup(x => x.GetAllAsync<User>()).ReturnsAsync(users);
        var repository = new UserRepository(mockDataContext.Object);

        // Act
        var result = await repository.GetByActiveStatusAsync(false);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task GetByActiveStatusAsync_WhenCalledWithTrueAndMixedUsers_ShouldReturnOnlyActiveUsers()
    {
        // Arrange
        var mockDataContext = new Mock<IDataContext>();

        var users = new List<User>
        {
            new User
            {
                Forename = "John",
                Surname = "Cena",
                Email = "CantSeeMe@example.com",
                IsActive = true,
                DateOfBirth = new DateTime(2000, 1, 1)
            },
            new User
            {
                Forename = "Under",
                Surname = "Taker",
                Email = "Tombstone@example.com",
                IsActive = false,
                DateOfBirth = new DateTime(2000, 1, 1)
            },
        };

        mockDataContext.Setup(x => x.GetAllAsync<User>()).ReturnsAsync(users);
        var repository = new UserRepository(mockDataContext.Object);

        // Act
        var result = await repository.GetByActiveStatusAsync(true);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task GetByActiveStatusAsync_WhenCalledWithFalseAndMixedUsers_ShouldReturnOnlyInactiveUsers()
    {
        // Arrange
        var mockDataContext = new Mock<IDataContext>();

        var users = new List<User>
        {
            new User
            {
                Forename = "John",
                Surname = "Cena",
                Email = "CantSeeMe@example.com",
                IsActive = true,
                DateOfBirth = new DateTime(2000, 1, 1)
            },
            new User
            {
                Forename = "Under",
                Surname = "Taker",
                Email = "Tombstone@example.com",
                IsActive = false,
                DateOfBirth = new DateTime(2000, 1, 1)
            },
        };

        mockDataContext.Setup(x => x.GetAllAsync<User>()).ReturnsAsync(users);
        var repository = new UserRepository(mockDataContext.Object);

        // Act
        var result = await repository.GetByActiveStatusAsync(false);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().IsActive.Should().BeFalse();
    }


    [Fact]
    public async Task GetByIdAsync_WhenUserListEmpty_ShouldReturnNull()
    {
        // Arrange
        var mockDataContext = new Mock<IDataContext>();
        var users = new List<User>();

        mockDataContext.Setup(x => x.GetAllAsync<User>()).ReturnsAsync(users);
        var repository = new UserRepository(mockDataContext.Object);

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_WhenUserExists_ShouldReturnUser()
    {
        // Arrange
        var mockDataContext = new Mock<IDataContext>();
        var users = new List<User>
        {
            new User { Id= 1, Forename = "John", Surname = "Smith", Email = "johnsmith@example.com", IsActive = true },
            new User { Id = 2, Forename = "Hannah", Surname = "Smith", Email = "hannahsmith@example.com", IsActive = false }
        };

        mockDataContext.Setup(x => x.GetAllAsync<User>()).ReturnsAsync(users);
        var repository = new UserRepository(mockDataContext.Object);

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Forename.Should().Be(users[0].Forename);
        result.Surname.Should().Be(users[0].Surname);
        result.Email.Should().Be(users[0].Email);
        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task GetByIdAsync_WhenUserDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var mockDataContext = new Mock<IDataContext>();
        var users = new List<User>
         {
             new User { Forename = "John", Surname = "Smith", Email = "johnsmith@example.com", IsActive = true },
             new User { Forename = "Hannah", Surname = "Smith", Email = "hannahsmith@example.com", IsActive = false }
         };

        mockDataContext.Setup(x => x.GetAllAsync<User>()).ReturnsAsync(users);
        var repository = new UserRepository(mockDataContext.Object);

        // Act
        var result = await repository.GetByIdAsync(99);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_WhenCalled_ShouldCallDataContext()
    {
        // Arrange
        var mockDataContext = new Mock<IDataContext>();
        var repository = new UserRepository(mockDataContext.Object);

        var user = new User
        {
            Forename = "Forename",
            Surname = "Surname",
            Email = "ForenameSurname@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(2000, 1, 1)
        };

        // Act
        var result = await repository.CreateAsync(user);

        // Assert
        mockDataContext.Verify(x => x.CreateAsync(user), Times.AtMostOnce);
    }

    [Fact]
    public async Task CreateAsync_WhenValidUserIsGiven_ShouldCreateUser()
    {
        // Arrange
        var mockDataContext = new Mock<IDataContext>();
        var repository = new UserRepository(mockDataContext.Object);

        var user = new User
        {
            Forename = "Forename",
            Surname = "Surname",
            Email = "ForenameSurname@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(2000, 1, 1)
        };

        mockDataContext
            .Setup(x => x.CreateAsync(user))
            .Callback<User>(u => u.Id = 1)
            .ReturnsAsync(user);

        // Act
        var result = await repository.CreateAsync(user);

        // Assert
        mockDataContext.Verify(x => x.CreateAsync(user), Times.AtMostOnce);
        result.Should().NotBeNull();
        result.Should().BeSameAs(user);
        result.Id.Should().Be(1);
    }


    [Fact]
    public async Task UpdateAsync_WhenCalled_ShouldCallDataContext()
    {
        // Arrange
        var mockDataContext = new Mock<IDataContext>();
        var repository = new UserRepository(mockDataContext.Object);

        var updatedUser = new User
        {
            Id = 1,
            Forename = "Surname",
            Surname = "Forename",
            Email = "ForenameSurname@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(2000, 1, 1)
        };

        // Act
        var result = await repository.UpdateAsync(updatedUser);

        // Assert
        mockDataContext.Verify(x => x.UpdateAsync(updatedUser), Times.AtMostOnce);
    }

    [Fact]
    public async Task UpdateAsync_WhenValidUser_ShouldReturnUpdatedUser()
    {
        // Arrange
        var mockDataContext = new Mock<IDataContext>();
        var repository = new UserRepository(mockDataContext.Object);

        var user = new User
        {
            Forename = "Forename",
            Surname = "Surname",
            Email = "ForenameSurname@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(2000, 1, 1)
        };

        var updatedUser = new User
        {
            Id = 1,
            Forename = "Surname",
            Surname = "Forename",
            Email = "ForenameSurname@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(2000, 1, 1)
        };

        mockDataContext
            .Setup(x => x.UpdateAsync(updatedUser))
            .ReturnsAsync(updatedUser);

        // Act
        var result = await repository.UpdateAsync(updatedUser);

        // Assert
        mockDataContext.Verify(x => x.UpdateAsync(updatedUser), Times.AtMostOnce);
        result.Should().NotBeNull();
        result.Should().BeSameAs(updatedUser);
        result.Id.Should().Be(1);
    }

    [Fact]
    public async Task DeleteAsync_WhenUserExists_ShouldDeleteAndReturnUser()
    {
        // Arrange
        var mockDataContext = new Mock<IDataContext>();
        var repository = new UserRepository(mockDataContext.Object);

        var users = new List<User>
        {
            new User
            {
                Id = 1,
                Forename = "testName1",
                Surname = "testSurname1",
                Email = "testemail1@example.com",
                IsActive = true
            },
            new User
            {
                Id = 2,
                Forename = "testName2",
                Surname = "testSurname2",
                Email = "testemail2@example.com",
                IsActive = true
            },
        };

        mockDataContext.Setup(x => x.GetAllAsync<User>()).ReturnsAsync(users);

        // Act
        var result = await repository.DeleteAsync(1);

        // Assert
        mockDataContext.Verify(x => x.DeleteAsync(users.Where(u => u.Id == 1)), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
    }

    [Fact]
    public async Task DeleteAsync_WhenUserDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var mockDataContext = new Mock<IDataContext>();
        var repository = new UserRepository(mockDataContext.Object);

        var users = new List<User>
        {
            new User
            {
                Id = 1,
                Forename = "testName1",
                Surname = "testSurname1",
                Email = "testemail1@example.com",
                IsActive = true
            },
            new User
            {
                Id = 2,
                Forename = "testName2",
                Surname = "testSurname2",
                Email = "testemail2@example.com",
                IsActive = true
            },
        };

        mockDataContext.Setup(x => x.GetAllAsync<User>()).ReturnsAsync(users);

        // Act
        var result = await repository.DeleteAsync(9999999);

        // Assert
        mockDataContext.Verify(x => x.DeleteAsync(It.IsAny<User>()), Times.Never);
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WhenUserActiveUserDeleted_ShouldDeleteAndReturnUser()
    {
        // Arrange
        var mockDataContext = new Mock<IDataContext>();
        var repository = new UserRepository(mockDataContext.Object);

        var users = new List<User>
        {
            new User
            {
                Id = 1,
                Forename = "testName1",
                Surname = "testSurname1",
                Email = "testemail1@example.com",
                IsActive = true
            },
        };

        mockDataContext.Setup(x => x.GetAllAsync<User>()).ReturnsAsync(users);

        // Act
        var result = await repository.DeleteAsync(1);

        // Assert
        mockDataContext.Verify(x => x.DeleteAsync(users.Where(u => u.Id == 1)), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_WhenUserInactiveUserDeleted_ShouldDeleteAndReturnUser()
    {
        // Arrange
        var mockDataContext = new Mock<IDataContext>();
        var repository = new UserRepository(mockDataContext.Object);

        var users = new List<User>
        {
            new User
            {
                Id = 1,
                Forename = "testName1",
                Surname = "testSurname1",
                Email = "testemail1@example.com",
                IsActive = false
            },
        };

        mockDataContext.Setup(x => x.GetAllAsync<User>()).ReturnsAsync(users);

        // Act
        var result = await repository.DeleteAsync(1);

        // Assert
        mockDataContext.Verify(x => x.DeleteAsync(users.Where(u => u.Id == 1)), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_WhenSpecificUserDeleted_ShouldOnlyDeleteAndReturnSpecifiedUser()
    {
        // Arrange
        var mockDataContext = new Mock<IDataContext>();
        var repository = new UserRepository(mockDataContext.Object);

        var users = new List<User>
        {
            new User
            {
                Id = 1,
                Forename = "testName1",
                Surname = "testSurname1",
                Email = "testemail1@example.com",
                IsActive = false
            },
            new User
            {
                Id = 2,
                Forename = "testName2",
                Surname = "testSurname2",
                Email = "testemail2@example.com",
                IsActive = false
            },
            new User
            {
                Id = 3,
                Forename = "testName3",
                Surname = "testSurname3",
                Email = "testemail3@example.com",
                IsActive = true
            },
        };

        mockDataContext.Setup(x => x.GetAllAsync<User>()).ReturnsAsync(users);

        // Act
        var result = await repository.DeleteAsync(1);

        //Assert
        mockDataContext.Verify(x => x.DeleteAsync(It.Is<User>(u => u.Id == 1)), Times.AtMostOnce);
        mockDataContext.Verify(x => x.DeleteAsync(It.Is<User>(u => u.Id == 2)), Times.Never);
        mockDataContext.Verify(x => x.DeleteAsync(It.Is<User>(u => u.Id == 3)), Times.Never);

        result.Should().NotBeNull();
    }

    private readonly Mock<IDataContext> _dataContext = new();
}
