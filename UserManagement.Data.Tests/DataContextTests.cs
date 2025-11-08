using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;
using UserManagement.Web.Models.Logs;

namespace UserManagement.Data.Tests;

public class DataContextTests
{

    #region Users Tests

    [Fact]
    public async Task GetAllAsync_WhenNewEntityAdded_MustIncludeNewEntity()
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

        await context.CreateAsync(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await context.GetAllAsync<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result
            .Should().Contain(s => s.Email == entity.Email)
            .Which.Should().BeEquivalentTo(entity);
    }

    [Fact]
    public async Task GetAllAsync_WhenDeleted_MustNotIncludeDeletedEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();
        var entity = (await context.GetAllAsync<User>()).First();
        var deletedUserId = entity.Id;
        await context.DeleteAsync(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await context.GetAllAsync<User>();

        // Assert: Verifies that the action of the method under test behaves as expected
        result.Should().NotContain(u => u.Id == deletedUserId);
    }

    // Tests for Date of Birth
    [Fact]
    public async Task GetAllAsync_WhenUserHasDateOfBirth_MustReturnUserWithDateOfBirth()
    {
        // Arrange
        var context = CreateContext();
        var dateOfBirth = new DateTime(1990, 5, 15);

        var entity = new User
        {
            Forename = "James", Surname = "Peach", Email = "AGiantPeach@example.com", DateOfBirth = dateOfBirth
        };

        await context.CreateAsync(entity);
        // Act
        var result = await context.GetAllAsync<User>();

        // Assert
        result
            .Should().Contain(u => u.Id == entity.Id)
            .Which.DateOfBirth.Should().Be(entity.DateOfBirth);
    }

    [Fact]
    public async Task GetAllAsync_WhenUserHasNullDateOfBirth_MustReturnUserWithNullDateOfBirth()
    {
        // Arrange
        var context = CreateContext();

        var entity = new User
        {
            Forename = "James", Surname = "Aurthur", Email = "WillNotLetGo@example.com", DateOfBirth = null
        };

        await context.CreateAsync(entity);
        // Act
        var result = await context.GetAllAsync<User>();    

        // Assert
        result
            .Should().Contain(u => u.Id == entity.Id)
            .Which.DateOfBirth.Should().Be(entity.DateOfBirth);
    }

    [Fact]
    public async Task CreateAsync_WhenValidUserProvided_ShouldReturnCreatedUser()
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
        var result = await context.CreateAsync(user);

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
    public async Task CreateAsync_WhenCalled_ShouldContainAndRetainUserInDataContext()
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
        var result = await context.CreateAsync(user);

        // Assert
        var allUsersInDataContext = await context.GetAllAsync<User>();
        var createdUsersId = result.Id;
        result.Should().NotBeNull();
        allUsersInDataContext.Should().Contain(u => u.Id == createdUsersId);
    }

    [Fact]
    public async Task CreateAsync_WhenMultipleUsersCreated_ShouldAssignDifferentIdsToNewCreatedUsers()
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
        var userCreated1 = await context.CreateAsync(newUser1);
        var userCreated2 = await context.CreateAsync(newUser2);

        // Assert
        userCreated1.Id.Should().BeGreaterThan(0);
        userCreated1.Id.Should().BeGreaterThan(0);
        userCreated2.Should().NotBe(userCreated1.Id);
    }

    [Fact]
    public async Task UpdateAsync_WhenValidUserProvided_ShouldReturnUpdatedUser()
    {
        // Arrange
        var context = CreateContext();
        var existingUser = (await context.GetAllAsync<User>()).First();

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
        var result = await context.UpdateAsync(existingUser);

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
    public async Task UpdateAsync_WhenCalled_ShouldContainAndRetainUsersInDataContext()
    {
        // Arrange
        var context = CreateContext();
        var existingUser = (await context.GetAllAsync<User>()).First();
        var userId = existingUser.Id;

        existingUser.Forename = "UpdatedForename";
        existingUser.Surname = "UpdatedSurname";

        // Act
        await context.UpdateAsync(existingUser);

        // Assert
        var updatedUser = (await context.GetAllAsync<User>()).First(u => u.Id == userId);
        updatedUser.Forename.Should().Be("UpdatedForename");
        updatedUser.Surname.Should().Be("UpdatedSurname");
    }

    [Fact]
    public async Task UpdateAsync_WhenUserForenameChanged_ShouldUpdateUser()
    {
        // Arrange
        var context = CreateContext();
        var existingUser = (await context.GetAllAsync<User>()).First();

        var previousForeName = existingUser.Forename;

        existingUser.Forename = "UpdatedForename123";

        // Act
        var result = await context.UpdateAsync(existingUser);

        // Assert
        result.Should().NotBeNull();
        result.Forename.Should().Be("UpdatedForename123");
        result.Forename.Should().NotBe(previousForeName);
    }

    [Fact]
    public async Task Update_WhenUserSurnameChanged_ShouldUpdateUser()
    {
        // Arrange
        var context = CreateContext();
        var existingUser = (await context.GetAllAsync<User>()).First();

        var previousSurname = existingUser.Surname;

        existingUser.Surname = "UpdatedSurname";

        // Act
        var result = await context.UpdateAsync(existingUser);

        // Assert
        result.Should().NotBeNull();
        result.Surname.Should().Be("UpdatedSurname");
        result.Surname.Should().NotBe(previousSurname);
    }

    [Fact]
    public async Task Update_WhenUserActiveStatusChanged_ShouldUpdateUser()
    {
        // Arrange
        var context = CreateContext();
        var existingUser = (await context.GetAllAsync<User>()).First();

        var previousActiveStatus = existingUser.IsActive;

        existingUser.IsActive = !existingUser.IsActive;

        // Act
        var result = await context.UpdateAsync(existingUser);

        // Assert
        result.Should().NotBeNull();
        result.IsActive.Should().Be(!previousActiveStatus);
        result.IsActive.Should().NotBe(previousActiveStatus);
    }

    [Fact]
    public async Task Update_WhenUserDateOfBirthUpdated_ShouldUpdateUser()
    {
        // Arrange
        var context = CreateContext();
        var existingUser = (await context.GetAllAsync<User>()).First();

        var previousDateOfBirth = existingUser.DateOfBirth;
        existingUser.DateOfBirth = new DateTime(2000, 11, 10);

        // Act
        var result = await context.UpdateAsync(existingUser);

        // Assert
        result.Should().NotBeNull();
        result.DateOfBirth.Should().Be(new DateTime(2000, 11, 10));
        result.DateOfBirth.Should().NotBe(previousDateOfBirth);
    }

    [Fact]
    public async Task Update_WhenUserEmailUpdated_ShouldReturnUpdateUser()
    {
        // Arrange
        var context = CreateContext();
        var existingUser = (await context.GetAllAsync<User>()).First();

        var previousEmail = existingUser.Email;
        existingUser.Email = "updatedEmail@example.com";


        // Act
        var result = await context.UpdateAsync(existingUser);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be("updatedEmail@example.com");
        result.Email.Should().NotBe(previousEmail);
    }

    [Fact]
    public async Task Update_WhenUserDateOfBirthUpdatedToNull_ShouldUpdateUser()
    {
        // Arrange
        var context = CreateContext();
        var existingUser = (await context.GetAllAsync<User>()).First();

        var previousDateOfBirth = existingUser.DateOfBirth;
        existingUser.DateOfBirth = null;

        // Act
        var result = await context.UpdateAsync(existingUser);

        // Assert
        result.Should().NotBeNull();
        result.DateOfBirth.Should().Be(null);
        result.DateOfBirth.Should().NotBe(previousDateOfBirth);
    }

    [Fact]
    public async Task DeleteAsync_WhenValidUserProvided_ShouldDeleteUserFromContext()
    {
        // Arrange
        var context = CreateContext();
        var userToDelete = (await context.GetAllAsync<User>()).First();
        var userId = userToDelete.Id;

        // Act
        await context.DeleteAsync(userToDelete);

        // Assert
        var allUsers = await context.GetAllAsync<User>();
        allUsers.Should().NotContain(u => u.Id == userId);
    }

    [Fact]
    public async Task Delete_WhenExistingUserDeleted_ShouldOnlyDeleteSpecifiedUser()
    {
        // Arrange
        var context = CreateContext();
        var initialCount = (await context.GetAllAsync<User>()).Count();
        var userToDelete = (await context.GetAllAsync<User>()).First();

        // Act
        await context.DeleteAsync(userToDelete);

        // Assert
        var remainingCount = (await context.GetAllAsync<User>()).Count();
        remainingCount.Should().Be(initialCount - 1);
    }


    #endregion

    #region Logs

    [Fact]
    public async Task Logs_ShouldExist()
    {
        // Arrange
        var context = CreateContext();

        // Act
        var logs = await context.Logs.ToListAsync();

        // Assert
        logs.Should().NotBeNull();
        logs.Should().BeEmpty();
    }

    [Fact]
    public async Task Logs_WhenLogCreated_ShouldAddToDatabase()
    {
        // Arrange
        var context = CreateContext();
        var log = new Log
        {
            UserId = 1,
            Action = "Action",
            Details = "Details",
            TimeStamp = new DateTime(2025, 10, 10)
        };

        // Act
        await context.Logs.AddAsync(log);
        await context.SaveChangesAsync();
        var savedLog = await context.Logs.FirstOrDefaultAsync(x => x.Id == 1);

        // Assert
        savedLog.Should().NotBeNull();
        savedLog.Id.Should().BeGreaterThan(0);
        savedLog.Action.Should().Be(log.Action);
        savedLog.Details.Should().Be(log.Details);
        savedLog.TimeStamp.Should().Be(log.TimeStamp);
    }

    [Fact]
    public async Task Log_WithUser_ShouldGetUserFromDatabaseAsWell()
    {
        // Arrange
        var context = CreateContext();

        var user = new User
        {
            Forename = "Forename",
            Surname = "Surname",
            Email = "Email@example.com",
            IsActive = true,
        };

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var log = new Log
        {
            UserId = user.Id,
            Action = "ACTION",
            Details = "ACTION user",
            TimeStamp = new DateTime(2025, 10, 10)
        };

        await context.Logs.AddAsync(log);
        await context.SaveChangesAsync();

        // Act
        var savedLog = await context.Logs.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == log.Id);

        // Assert
        savedLog.Should().NotBeNull();


        // User is populated from the Users database into the Log database
        savedLog.User.Should().NotBeNull();
        savedLog.User.Forename.Should().Be(user.Forename);
        savedLog.User.Surname.Should().Be(user.Surname);
        savedLog.User.Email.Should().Be(user.Email);
        savedLog.User.IsActive.Should().Be(user.IsActive);
    }

    //Scenario, user is deleted but has logs, should not get rid of the logs
    [Fact]
    public async Task Log_WhenUserIsDeleted_ShouldAllowNullUserIdInLogs()
    {
        // Arrange
        var context = CreateContext();

        var log = new Log
        {
            UserId = null,
            Action = "ACTION",
            Details = "ACTION NO USER",
            TimeStamp = new DateTime(2025, 10, 10)
        };

        // Act
        await context.Logs.AddAsync(log);
        await context.SaveChangesAsync();

        // Assert
        var savedLog = await context.Logs.FirstOrDefaultAsync((x => x.Action.Equals(log.Action)));
        savedLog.Should().NotBeNull();
        savedLog.UserId.Should().BeNull();
    }

    #endregion

    #region Helper functions

    //// <summary>
    //// Trying to prevent database from persisting between tests
    // </summary>
    private DataContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TEST_DATABASE" + Guid.NewGuid())
            .Options;

        var context = new DataContext(options);
        // This ensures data is created and should only be used in testing
        context.Database.EnsureCreated();
        return context;
    }

    #endregion
}
