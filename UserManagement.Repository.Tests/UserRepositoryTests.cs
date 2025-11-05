using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Repository.Implementations;

namespace UserManagement.Repository.Tests;

public class UserRepositoryTests
{
    [Fact]
    public void GetAll_ShouldReturnAnEmptyCollection_WhenNoUsers()
    {
        // Arrange
        //var mockDataContext = new Mock<IDataContext>();
        //mockDataContext.Setup(x => x.GetAll<User>())
        //        .Returns(new List<User>().AsQueryable());

        //var repository = new UserRepository(mockDataContext.Object);
        _dataContext.Setup(x => x.GetAll<User>())
                .Returns(new List<User>().AsQueryable());

        var repository = new UserRepository(_dataContext.Object);
        // Act
        var result = repository.GetAll();

        // Assert
        result.AsEnumerable().Should().NotBeNull();
        result.AsEnumerable().Should().BeEmpty();
    }

    [Fact]
    public void GetAll_WhenCalled_ShouldReturnAllUsers()
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
        
        mockDataContext.Setup(x => x.GetAll<User>())
            .Returns(users.AsQueryable);

        var repository = new UserRepository(mockDataContext.Object);

        // Act
        var result = repository.GetAll();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(users);
    }

    [Fact]
    public void GetByActiveStatus_WhenCalledWithTrue_ShouldReturnActiveUsers()
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

        mockDataContext.Setup(x => x.GetAll<User>()).Returns(users.AsQueryable());
        var repository = new UserRepository(mockDataContext.Object);

        // Act
        var result = repository.GetByActiveStatus(true);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().IsActive.Should().BeTrue();
    }

    [Fact]
    public void GetByActiveStatus_WhenCalledWithFalse_ShouldReturnInactiveUsers()
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

        mockDataContext.Setup(x => x.GetAll<User>()).Returns(users.AsQueryable());
        var repository = new UserRepository(mockDataContext.Object);

        // Act
        var result = repository.GetByActiveStatus(false);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().IsActive.Should().BeFalse();
    }

    [Fact]
    public void GetByActiveStatus_WhenCalledWithTrueAndMixedUsers_ShouldReturnOnlyActiveUsers()
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

        mockDataContext.Setup(x => x.GetAll<User>()).Returns(users.AsQueryable());
        var repository = new UserRepository(mockDataContext.Object);

        // Act
        var result = repository.GetByActiveStatus(true);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().IsActive.Should().BeTrue();
    }

    [Fact]
    public void GetByActiveStatus_WhenCalledWithFalseAndMixedUsers_ShouldReturnOnlyInactiveUsers()
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

        mockDataContext.Setup(x => x.GetAll<User>()).Returns(users.AsQueryable());
        var repository = new UserRepository(mockDataContext.Object);

        // Act
        var result = repository.GetByActiveStatus(false);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().IsActive.Should().BeFalse();
    }

    // TODO adding Task 3 tests

    [Fact]
    public void GetById_WhenUserListEmpty_ShouldReturnNull()
    {
        // Arrange
        var mockDataContext = new Mock<IDataContext>();
        var users = new List<User>();

        mockDataContext.Setup(x => x.GetAll<User>()).Returns(users.AsQueryable());
        var repository = new UserRepository(mockDataContext.Object);

        // Act
        var result = repository.GetById(1);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetById_WhenUserExists_ShouldReturnUser()
    {
        // Arrange
        var mockDataContext = new Mock<IDataContext>();
        var users = new List<User>
        {
            new User { Id= 1, Forename = "John", Surname = "Smith", Email = "johnsmith@example.com", IsActive = true },
            new User { Id = 2, Forename = "Hannah", Surname = "Smith", Email = "hannahsmith@example.com", IsActive = false }
        };

        mockDataContext.Setup(x => x.GetAll<User>()).Returns(users.AsQueryable());
        var repository = new UserRepository(mockDataContext.Object);

        // Act
        var result = repository.GetById(1);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Forename.Should().Be(users[0].Forename);
        result.Surname.Should().Be(users[0].Surname);
        result.Email.Should().Be(users[0].Email);
        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public void GetById_WhenUserDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var mockDataContext = new Mock<IDataContext>();
        var users = new List<User>
         {
             new User { Forename = "John", Surname = "Smith", Email = "johnsmith@example.com", IsActive = true },
             new User { Forename = "Hannah", Surname = "Smith", Email = "hannahsmith@example.com", IsActive = false }
         };

        mockDataContext.Setup(x => x.GetAll<User>()).Returns(users.AsQueryable());
        var repository = new UserRepository(mockDataContext.Object);

        // Act
        var result = repository.GetById(99);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Create_WhenCalled_ShouldCallDataContext()
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
        var result = repository.Create(user);

        // Assert
        mockDataContext.Verify(x => x.Create(user), Times.AtMostOnce);
    }

    [Fact]
    public void Create_WhenValidUserIsGiven_ShouldCreateUser()
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
            .Setup(x => x.Create(user))
            .Callback<User>(u => u.Id = 1)
            .Returns<User>(u => u);

        // Act
        var result = repository.Create(user);

        // Assert
        mockDataContext.Verify(x => x.Create(user), Times.AtMostOnce);
        result.Should().NotBeNull();
        result.Should().BeSameAs(user);
        result.Id.Should().Be(1);
    }

    // TODO update
    [Fact]
    public void Update_WhenCalled_ShouldCallDataContext()
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
        var result = repository.Update(updatedUser);

        // Assert
        mockDataContext.Verify(x => x.Update(updatedUser), Times.AtMostOnce);
    }

    [Fact]
    public void Update_WhenValidUser_ShouldReturnUpdatedUser()
    {
        // Arrange
        var mockDataContext = new Mock<IDataContext>();
        var repository = new UserRepository(mockDataContext.Object);

        // TODO remove if you can't find solution to integrate the change 
        var user = new User
        {
            Forename = "Forename",
            Surname = "Surname",
            Email = "ForenameSurname@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(2000, 1, 1)
        };

        // Can we add an old user then update and then expect it to update?

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
            .Setup(x => x.Update(updatedUser))
            .Returns<User>(u => u);

        // Act
        var result = repository.Update(updatedUser);

        // Assert
        mockDataContext.Verify(x => x.Update(updatedUser), Times.AtMostOnce);
        result.Should().NotBeNull();
        result.Should().BeSameAs(updatedUser);
        result.Id.Should().Be(1);
    }

    [Fact]
    public void Delete_WhenUserExists_ShouldDeleteAndReturnUser()
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

        mockDataContext.Setup(x => x.GetAll<User>()).Returns(users.AsQueryable());

        // Act
        var result = repository.Delete(1);

        // Assert
        mockDataContext.Verify(x => x.Delete(users.Where(u => u.Id == 1)), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
    }

    [Fact]
    public void Delete_WhenUserDoesNotExist_ShouldReturnNull()
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

        mockDataContext.Setup(x => x.GetAll<User>()).Returns(users.AsQueryable());

        // Act
        var result = repository.Delete(9999999);

        // Assert
        mockDataContext.Verify(x => x.Delete(It.IsAny<User>()), Times.Never);
        result.Should().BeNull();
    }

    [Fact]
    public void Delete_WhenUserActiveUserDeleted_ShouldDeleteAndReturnUser()
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

        mockDataContext.Setup(x => x.GetAll<User>()).Returns(users.AsQueryable());

        // Act
        var result = repository.Delete(1);

        // Assert
        mockDataContext.Verify(x => x.Delete(users.Where(u => u.Id == 1)), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Delete_WhenUserInactiveUserDeleted_ShouldDeleteAndReturnUser()
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

        mockDataContext.Setup(x => x.GetAll<User>()).Returns(users.AsQueryable());

        // Act
        var result = repository.Delete(1);

        // Assert
        mockDataContext.Verify(x => x.Delete(users.Where(u => u.Id == 1)), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Delete_WhenSpecificUserDeleted_ShouldOnlyDeleteAndReturnSpecifiedUser()
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

        mockDataContext.Setup(x => x.GetAll<User>()).Returns(users.AsQueryable());

        // Act
        var result = repository.Delete(1);

        //Assert
        mockDataContext.Verify(x => x.Delete(It.Is<User>(u => u.Id == 1)), Times.AtMostOnce);
        mockDataContext.Verify(x => x.Delete(It.Is<User>(u => u.Id == 2)), Times.Never);
        mockDataContext.Verify(x => x.Delete(It.Is<User>(u => u.Id == 3)), Times.Never);
        
        result.Should().NotBeNull();
    }

    private readonly Mock<IDataContext> _dataContext = new();
}
