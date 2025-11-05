using System;
using System.Linq;
using UserManagement.Models;

namespace UserManagement.Data.Tests;

public class DataContextTests
{
    [Fact]
    public void GetAll_WhenNewEntityAdded_MustIncludeNewEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();

        var entity = new User
        {
            Forename = "James",
            Surname = "Bond",
            Email = "Double07@example.com",
            DateOfBirth = new DateTime(2000, 1, 1),
        };
        context.Create(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = context.GetAll<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result
            .Should().Contain(s => s.Email == entity.Email)
            .Which.Should().BeEquivalentTo(entity);
    }

    [Fact]
    public void GetAll_WhenDeleted_MustNotIncludeDeletedEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();
        var entity = context.GetAll<User>().First();
        var deletedUserId = entity.Id;
        context.Delete(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = context.GetAll<User>();

        // Assert: Verifies that the action of the method under test behaves as expected
        result.Should().NotContain(u => u.Id == deletedUserId);
    }

    // Tests for Date of Birth
    [Fact]
    public void GetAll_WhenUserHasDateOfBirth_MustReturnUserWithDateOfBirth()
    {
        // Arrange
        var context = CreateContext();
        var dateOfBirth = new DateTime(1990, 5, 15);

        var entity = new User
        {
            Forename = "James", Surname = "Peach", Email = "AGiantPeach@example.com", DateOfBirth = dateOfBirth
        };

        context.Create(entity);
        // Act
        var result = context.GetAll<User>();

        // Assert
        result
            .Should().Contain(u => u.Id == entity.Id)
            .Which.DateOfBirth.Should().Be(entity.DateOfBirth);
    }

    [Fact]
    public void GetAll_WhenUserHasNullDateOfBirth_MustReturnUserWithNullDateOfBirth()
    {
        // Arrange
        var context = CreateContext();

        var entity = new User
        {
            Forename = "James", Surname = "Aurthur", Email = "WillNotLetGo@example.com", DateOfBirth = null
        };

        context.Create(entity);
        // Act
        var result = context.GetAll<User>();

        // Assert
        result
            .Should().Contain(u => u.Id == entity.Id)
            .Which.DateOfBirth.Should().Be(entity.DateOfBirth);
    }

    // TODO add tests for Create Update
    [Fact]
    public void Create_WhenValidUserProvided_ShouldReturnCreatedUser()
    {
        // Arrange
        var context = CreateContext();
        var user = new User
        {
            Forename = "John",
            Surname = "Travolta",
            Email = "JTravolta@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(1989, 8, 8)
        };

        // Act
        var result = context.Create(user);

        // Assert
        result.Should().NotBeNull();
        // The id is 12 after what's in the DataContext already;
        result.Id.Should().BeGreaterThan(0);
        result.Forename.Should().Be(user.Forename);
        result.Surname.Should().Be(user.Surname);
        result.Email.Should().Be(user.Email);
        result.IsActive.Should().Be(user.IsActive);
        result.DateOfBirth.Should().Be(user.DateOfBirth);
    }

    [Fact]
    public void Create_WhenCalled_ShouldContainAndRetainUserInDataContext()
    {
        // Arrange
        var context = CreateContext();
        var user = new User
        {
            Forename = "John",
            Surname = "Travolta",
            Email = "JTravolta@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(1989, 8, 8)
        };

        // Act
        var result = context.Create(user);

        // Assert
        var allUsersInDataContext = context.GetAll<User>();
        var createdUsersId = result.Id;
        result.Should().NotBeNull();
        allUsersInDataContext.Should().Contain(u => u.Id == createdUsersId);
    }

    [Fact]
    public void Create_WhenMultipleUsersCreated_ShouldAssignDifferentIdsToNewCreatedUsers()
    {
        // Arrange
        var context = CreateContext();
        var newUser1 = new User
        {
            Forename = "John",
            Surname = "Travolta",
            Email = "JTravolta@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(1989, 8, 8)
        };

        var newUser2 = new User
        {
            Forename = "Mr",
            Surname = "T",
            Email = "MrT@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(1989, 8, 8)
        };

        // Act
        var userCreated1 = context.Create(newUser1);
        var userCreated2 = context.Create(newUser2);

        // Assert
        userCreated1.Id.Should().BeGreaterThan(0);
        userCreated1.Id.Should().BeGreaterThan(0);
        userCreated2.Should().NotBe(userCreated1.Id);

    }

    // TODO tests for update
    [Fact]
    public void Update_WhenValidUserProvided_ShouldReturnUpdatedUser()
    {
        // Arrange
        var context = CreateContext();
        var existingUser = context.GetAll<User>().First();

        var previousEmail = existingUser.Email;
        var previousForeName = existingUser.Forename;
        var previousSurname = existingUser.Surname;
        var previousActiveStatus = existingUser.IsActive;
        var previousDateOfBirth = existingUser.DateOfBirth;

        existingUser.Email = "updatedEmail@example.com";
        existingUser.IsActive = !existingUser.IsActive;
        existingUser.Forename = "UpdatedForename";
        existingUser.Surname = "UpdatedSurname";
        existingUser.DateOfBirth = new DateTime(2000, 10, 10);

        // Act
        var result = context.Update(existingUser);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be("updatedEmail@example.com");
        result.IsActive.Should().Be(!previousActiveStatus);
        result.Forename.Should().Be("UpdatedForename");
        result.Surname.Should().Be("UpdatedSurname");
        result.DateOfBirth.Should().Be(new DateTime(2000, 10, 10));

        result.Email.Should().NotBe(previousEmail);
        result.IsActive.Should().NotBe(previousActiveStatus);
        result.Forename.Should().NotBe(previousForeName);
        result.Surname.Should().NotBe(previousSurname);
        result.DateOfBirth.Should().NotBe(previousDateOfBirth);
    }

    [Fact]
    public void Update_WhenCalled_ShouldContainAndRetainUsersInDataContext()
    {
        // Arrange
        var context = CreateContext();
        var existingUser = context.GetAll<User>().First();
        var userId = existingUser.Id;

        existingUser.Forename = "UpdatedForename";
        existingUser.Surname = "UpdatedSurname";

        // Act
        context.Update(existingUser);

        // Assert
        var updatedUser = context.GetAll<User>().First(u => u.Id == userId);
        updatedUser.Forename.Should().Be("UpdatedForename");
        updatedUser.Surname.Should().Be("UpdatedSurname");
    }

    [Fact]
    public void Update_WhenUserForenameChanged_ShouldUpdateUser()
    {
        // Arrange
        var context = CreateContext();
        var existingUser = context.GetAll<User>().First();

        var previousForeName = existingUser.Forename;

        existingUser.Forename = "UpdatedForename123";

        // Act
        var result = context.Update(existingUser);

        // Assert
        result.Should().NotBeNull();
        result.Forename.Should().Be("UpdatedForename123");
        result.Forename.Should().NotBe(previousForeName);
    }

    [Fact]
    public void Update_WhenUserSurnameChanged_ShouldUpdateUser()
    {
        // Arrange
        var context = CreateContext();
        var existingUser = context.GetAll<User>().First();

        var previousSurname = existingUser.Surname;

        existingUser.Surname = "UpdatedSurname";

        // Act
        var result = context.Update(existingUser);

        // Assert
        result.Should().NotBeNull();
        result.Surname.Should().Be("UpdatedSurname");
        result.Surname.Should().NotBe(previousSurname);
    }

    [Fact]
    public void Update_WhenUserActiveStatusChanged_ShouldUpdateUser()
    {
        // Arrange
        var context = CreateContext();
        var existingUser = context.GetAll<User>().First();

        var previousActiveStatus = existingUser.IsActive;

        existingUser.IsActive = !existingUser.IsActive;

        // Act
        var result = context.Update(existingUser);

        // Assert
        result.Should().NotBeNull();
        result.IsActive.Should().Be(!previousActiveStatus);
        result.IsActive.Should().NotBe(previousActiveStatus);
    }

    [Fact]
    public void Update_WhenUserDateOfBirthUpdated_ShouldUpdateUser()
    {
        // Arrange
        var context = CreateContext();
        var existingUser = context.GetAll<User>().First();

        var previousDateOfBirth = existingUser.DateOfBirth;
        existingUser.DateOfBirth = new DateTime(2000, 11, 10);

        // Act
        var result = context.Update(existingUser);

        // Assert
        result.Should().NotBeNull();
        result.DateOfBirth.Should().Be(new DateTime(2000, 11, 10));
        result.DateOfBirth.Should().NotBe(previousDateOfBirth);
    }

    [Fact] public void Update_WhenUserEmailUpdated_ShouldReturnUpdateUser()
    {
        // Arrange
        var context = CreateContext();
        var existingUser = context.GetAll<User>().First();

        var previousEmail = existingUser.Email;
        existingUser.Email = "updatedEmail@example.com";


        // Act
        var result = context.Update(existingUser);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be("updatedEmail@example.com");
        result.Email.Should().NotBe(previousEmail);
    }

    [Fact]
    public void Update_WhenUserDateOfBirthUpdatedToNull_ShouldUpdateUser()
    {
        // Arrange
        var context = CreateContext();
        var existingUser = context.GetAll<User>().First();

        var previousDateOfBirth = existingUser.DateOfBirth;
        existingUser.DateOfBirth = null;

        // Act
        var result = context.Update(existingUser);

        // Assert
        result.Should().NotBeNull();
        result.DateOfBirth.Should().Be(null);
        result.DateOfBirth.Should().NotBe(previousDateOfBirth);
    }

    // TODO tests for delete

    //[Fact]
    //public void Delete_WhenValidUserProvided
    [Fact]
    public void Delete_WhenValidUserProvided_ShouldDeleteUserFromContext()
    {
        // Arrange
        var context = CreateContext();
        var userToDelete = context.GetAll<User>().First();
        var userId = userToDelete.Id;

        // Act
        context.Delete(userToDelete);

        // Assert
        var allUsers = context.GetAll<User>();
        allUsers.Should().NotContain(u => u.Id == userId);
    }

    [Fact]
    public void Delete_WhenExistingUserDeleted_ShouldOnlyDeleteSpecifiedUser()
    {
        // Arrange
        var context = CreateContext();
        var initialCount = context.GetAll<User>().Count();
        var userToDelete = context.GetAll<User>().First();

        // Act
        context.Delete(userToDelete);

        // Assert
        var remainingCount = context.GetAll<User>().Count();
        remainingCount.Should().Be(initialCount - 1);
    }

    private DataContext CreateContext() => new();
}
