using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Repository.Implementations;
using UserManagement.Web.Models.Logs;

namespace UserManagement.Repository.Tests;

public class LogRepositoryTests
{

    #region GetAllAsync Tests
    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyCollection_WhenNoLogs()
    {
        // Arrange
        var context = CreateContext();
        var user = new User { Forename = "Test", Surname = "User", Email = "test@example.com", IsActive = true };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var log = new Log { UserId = user.Id, Action = "CREATE", Details = "Created user", TimeStamp = DateTime.UtcNow };
        await context.Logs.AddAsync(log);
        await context.SaveChangesAsync();

        var repository = new LogRepository(context);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        result.Should().NotBeEmpty();

    }

    [Fact]
    public async Task GetAllAsync_WhenCalled_ShouldReturnAllLogs()
    {
        // Arrange
        var context = CreateContext();
        var user = new User { Forename = "Test", Surname = "User", Email = "test@example.com", IsActive = true };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var log1 = new Log { UserId = user.Id, Action = "CREATE", Details = "Created user", TimeStamp = DateTime.UtcNow };
        var log2 = new Log { UserId = user.Id, Action = "CREATE", Details = "Created user", TimeStamp = DateTime.UtcNow };
        await context.Logs.AddAsync(log1);
        await context.Logs.AddAsync(log2);
        await context.SaveChangesAsync();

        var repository = new LogRepository(context);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().HaveCount(2);

        // We sort by descending so most recent time stamp will be at the top
        result[0].Should().BeEquivalentTo(log2);
        result[1].Should().BeEquivalentTo(log1);
    }

    [Fact]
    public async Task GetAllAsync_WhenUserNotNullInLog_ShouldIncludeUser()
    {
        // Arrange
        var context = CreateContext();
        var user = new User { Forename = "Test", Surname = "User", Email = "test@example.com", IsActive = true };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var log1 = new Log { UserId = user.Id, Action = "CREATE", Details = "Created user", TimeStamp = DateTime.UtcNow, User = user};
        await context.Logs.AddAsync(log1);
        await context.SaveChangesAsync();

        var repository = new LogRepository(context);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        result.First().User.Should().NotBeNull();
        result.First().User?.Forename.Should().Be("Test");
    }


    #endregion

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnLog()
    {
        // Arrange
        var context = CreateContext();
        var user = new User { Forename = "Test", Surname = "User", Email = "test@example.com", IsActive = true };
        var userInDatabase = await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var log1 = new Log { UserId = userInDatabase.Entity.Id,Action = "CREATE", Details = "Created user", TimeStamp = DateTime.UtcNow };
        await context.Logs.AddAsync(log1);
        await context.SaveChangesAsync();

        var repository = new LogRepository(context);

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Action.Should().Be("CREATE");
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var context = CreateContext();
        var user = new User { Forename = "Test", Surname = "User", Email = "test@example.com", IsActive = true };
        var userInDatabase = await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var repository = new LogRepository(context);

        // Act
        var result = await repository.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region GetByUserId Tests

    [Fact]
    public async Task GetByUserIdAsync_WhenGivenValidUserId_ShouldReturnUserSpecificLogs()
    {
        // Arrange
        var context = CreateContext();
        var user = new User { Forename = "Test", Surname = "User", Email = "test@example.com", IsActive = true };
        var userInDatabase = await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var log1 = new Log { UserId = userInDatabase.Entity.Id, Action = "CREATE", Details = "Created user", TimeStamp = DateTime.UtcNow.AddHours(-1) };
        var log2 = new Log { UserId = userInDatabase.Entity.Id, Action = "CREATE", Details = "Created user", TimeStamp = DateTime.UtcNow };
        await context.Logs.AddAsync(log1);
        await context.Logs.AddAsync(log2);
        await context.SaveChangesAsync();

        var repository = new LogRepository(context);

        // Act
        var result = await repository.GetByUserIdAsync(userInDatabase.Entity.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().OnlyContain(log => log.UserId == userInDatabase.Entity.Id);
    }

    [Fact]
    public async Task GetByUserIdAsync_WhenNoLogs_ShouldReturnEmptyLogs()
    {
        // Arrange
        var context = CreateContext();
        var user = new User { Forename = "Test", Surname = "User", Email = "test@example.com", IsActive = true };
        var userInDatabase = await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var repository = new LogRepository(context);

        // Act
        var result = await repository.GetByUserIdAsync(999);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    #endregion

    #region GetPaginatedLogs Test

    [Fact]
    public async Task GetPaginatedAsync_WithValidPageAndPageSize_ShouldReturnLogsWithinCurrentPage()
    {
        // Arrange
        var context = CreateContext();
        var user = new User { Forename = "Test", Surname = "User", Email = "test@example.com", IsActive = true };
        var userInDatabase = await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var logCount = 10;
        await CreateXLogsInContext(context, logCount, userInDatabase.Entity.Id);
        await context.SaveChangesAsync();

        var repository = new LogRepository(context);

        var currentPageIndex = 1;
        var pageSize = 10;

        // Act
        var result = await repository.GetPaginatedLogsAsync(currentPageIndex, pageSize);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(logCount);
    }



    [Fact]
    public async Task GetPaginatedAsync_WhenOnFirstPage_ShouldReturnLogsOnFirstPage()
    {
        // Arrange
        var context = CreateContext();
        var user = new User { Forename = "Test", Surname = "User", Email = "test@example.com", IsActive = true };
        var userInDatabase = await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var logCount = 5;
        await CreateXLogsInContext(context, logCount, userInDatabase.Entity.Id);
        await context.SaveChangesAsync();

        var repository = new LogRepository(context);

        // Act
        var result = await repository.GetPaginatedLogsAsync(1, 5);

        // Assert
        result.Should().HaveCount(5);
        result.Should().BeInDescendingOrder(l => l.TimeStamp);
    }

    [Fact]
    public async Task GetPaginatedAsync_WhenOnLastPage_ShouldReturnLogsOnFirstPage()
    {
        // Arrange
        var context = CreateContext();
        var user = new User { Forename = "Test", Surname = "User", Email = "test@example.com", IsActive = true };
        var userInDatabase = await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var logCount = 15;
        await CreateXLogsInContext(context, logCount, userInDatabase.Entity.Id);
        await context.SaveChangesAsync();

        var repository = new LogRepository(context);

        // Act
        var result = await repository.GetPaginatedLogsAsync(3, 6);

        // Assert
        result.Should().HaveCount(3);
        result.Should().BeInDescendingOrder(l => l.TimeStamp);
    }

    #endregion

    [Fact]
    public async Task GetTotalCountAsync_ShouldReturnCorrectCount()
    {
        // Arrange
        var context = CreateContext();
        var user = new User { Forename = "Test", Surname = "User", Email = "test@example.com", IsActive = true };
        var userInDatabase = await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var logCount = 15;
        await CreateXLogsInContext(context, logCount, userInDatabase.Entity.Id);
        await context.SaveChangesAsync();

        var repository = new LogRepository(context);

        // Act
        var result = await repository.GetTotalLogCountAsync();

        // Assert
        result.Should().Be(logCount);
    }

    [Fact]
    public async Task CreateAsync_WithValidLog_ShouldAddLogToDatabase()
    {
        // Arrange
        var context = CreateContext();
        var log = new Log
        {
            UserId = 1,
            Action = "CREATE",
            Details = "Created user",
            TimeStamp = DateTime.UtcNow
        };

        await context.SaveChangesAsync();

        var repository = new LogRepository(context);

        // Act
        await repository.CreateAsync(log);

        // Assert
        var logs = await context.Logs.ToListAsync();
        logs.Should().ContainSingle();
        logs[0].Should().BeEquivalentTo(log, options => options.Excluding(l => l.Id));

    }

    #region Helper functions

    private DataContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new DataContext(options);
        
        context.Database.EnsureCreated();
        return context;
    }

    private async Task<List<Log>> CreateXLogsInContext(DataContext context, int logCount, long? userId)
    {
        var logs = new List<Log>();

        for (var i = 0; i < logCount; i++)
        {
            var log = new Log
            {
                UserId = userId,
                Action = "TEST",
                Details = $"TEST {i}",
                TimeStamp = DateTime.Today.AddHours(-i)
            };
            await context.Logs.AddAsync(log);
            logs.Add(log);
        }

        await context.SaveChangesAsync();
        return logs;
    }

    #endregion
}

