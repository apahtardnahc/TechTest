using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Repository.Interfaces;
using UserManagement.Services.Domain.Implementations;
using UserManagement.Web.Models.Logs;

namespace UserManagement.Data.Tests;

public class LogServiceTests
{
    private readonly Mock<ILogRepository> _logRepository = new();
    private LogService CreateService() => new(_logRepository.Object);

    #region GetAllAsync Tests

    [Fact]
    public async Task GetAllAsync_WhenCalled_ShouldCallRepository()
    {
        // Arrange
        var service = CreateService();
        var logs = CreateTestLogs(10);
        _logRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(logs);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        _logRepository.Verify(x => x.GetAllAsync(), Times.AtMostOnce);
    }

    [Fact]
    public async Task GetAllAsync_WhenNoLogs_ShouldReturnEmptyLogs()
    {
        // Arrange
        var service = CreateService();
        _logRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Log>());

        // Act
        var result = await service.GetAllAsync();

        // Assert
        _logRepository.Verify(x => x.GetAllAsync(), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_WhenCalledAndDatabaseHasLogs_ShouldReturnLogs()
    {
        // Arrange
        var service = CreateService();
        var logCount = 10;
        var logs = CreateTestLogs(logCount);
        _logRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(logs);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        _logRepository.Verify(x => x.GetAllAsync(), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().HaveCount(logCount);
    }

    #endregion

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldCallRepository()
    {
        // Arrange
        var service = CreateService();
        var log = CreateTestLogs(1).First();

        _logRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(log);

        // Act
        var result = await service.GetByIdAsync(1);

        // Assert
        _logRepository.Verify(x => x.GetByIdAsync(1), Times.AtMostOnce);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnLog()
    {
        // Arrange
        var service = CreateService();
        var log = CreateTestLogs(1).First();

        _logRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(log);

        // Act
        var result = await service.GetByIdAsync(1);

        // Assert
        _logRepository.Verify(x => x.GetByIdAsync(1), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.Id.Should().Be(log.Id);
        result.Action.Should().Be(log.Action);
        result.Details.Should().Be(log.Details);
        result.TimeStamp.Should().Be(log.TimeStamp);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalid_ShouldReturnNull()
    {
        // Arrange
        var service = CreateService();

        _logRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((Log?)null);

        // Act
        var result = await service.GetByIdAsync(1);

        // Assert
        _logRepository.Verify(x => x.GetByIdAsync(1), Times.AtMostOnce);

        result.Should().BeNull();
    }

    #endregion

    #region GetPaginatedLogsAsync Tests

    [Fact]
    public async Task GetPaginatedLogsAsync_WhenCalledWithValidParameters_ShouldCallRepository()
    {
        // Arrange
        var service = CreateService();
        var log = CreateTestLogs(1).First();

        _logRepository.Setup(x => x.GetPaginatedLogsAsync(1, 5));
        _logRepository.Setup(x => x.GetTotalLogCountAsync()).ReturnsAsync(1);

        // Act
        await service.GetPaginatedLogsAsync(1, 5);

        // Assert
        _logRepository.Verify(x => x.GetPaginatedLogsAsync(1, 5), Times.AtMostOnce);
        _logRepository.Verify(x => x.GetTotalLogCountAsync(), Times.AtMostOnce);
    }

    [Fact]
    public async Task GetPaginatedLogsAsync_WhenCalledWithValidParameters_ShouldReturnPaginatedLogs()
    {
        // Arrange
        var service = CreateService();
        var logCount = 30;
        var logs = CreateTestLogs(logCount);

        _logRepository.Setup(x => x.GetPaginatedLogsAsync(1, 5)).ReturnsAsync(logs);
        _logRepository.Setup(x => x.GetTotalLogCountAsync()).ReturnsAsync(logCount);

        // Act
        var result = await service.GetPaginatedLogsAsync(1, 5);

        // Assert
        _logRepository.Verify(x => x.GetPaginatedLogsAsync(1, 5), Times.AtMostOnce);
        _logRepository.Verify(x => x.GetTotalLogCountAsync(), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.Items.Should().HaveCount(logCount);
        result.CurrentPageIndex.Should().Be(1);
        result.PageSize.Should().Be(5);
        result.TotalPages.Should().Be(6);
        result.TotalCount.Should().Be(logCount);
        result.HasPreviousPage.Should().BeFalse();
        result.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public async Task GetPaginatedLogsAsync_WhenOnSecondPage_ShouldHavePreviousAndNextPages()
    {
        // Arrange
        var service = CreateService();
        var logCount = 30;
        var logs = CreateTestLogs(logCount);

        _logRepository.Setup(x => x.GetPaginatedLogsAsync(2, 5)).ReturnsAsync(logs);
        _logRepository.Setup(x => x.GetTotalLogCountAsync()).ReturnsAsync(logCount);

        // Act
        var result = await service.GetPaginatedLogsAsync(2, 5);

        // Assert
        _logRepository.Verify(x => x.GetPaginatedLogsAsync(2, 5), Times.AtMostOnce);
        _logRepository.Verify(x => x.GetTotalLogCountAsync(), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.Items.Should().HaveCount(logCount);
        result.CurrentPageIndex.Should().Be(2);
        result.HasPreviousPage.Should().BeTrue();
        result.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public async Task GetPaginatedLogsAsync_WhenOnLastPage_ShouldHavePreviousPageButNotNextPage()
    {
        // Arrange
        var service = CreateService();
        var logCount = 30;
        var logs = CreateTestLogs(logCount);

        _logRepository.Setup(x => x.GetPaginatedLogsAsync(6, 5)).ReturnsAsync(logs);
        _logRepository.Setup(x => x.GetTotalLogCountAsync()).ReturnsAsync(logCount);

        // Act
        var result = await service.GetPaginatedLogsAsync(6, 5);

        // Assert
        _logRepository.Verify(x => x.GetPaginatedLogsAsync(6, 5), Times.AtMostOnce);
        _logRepository.Verify(x => x.GetTotalLogCountAsync(), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.Items.Should().HaveCount(logCount);
        result.CurrentPageIndex.Should().Be(6);
        result.HasPreviousPage.Should().BeTrue();
        result.HasNextPage.Should().BeFalse();
    }

    [Fact]
    public async Task GetPaginatedLogsAsync_WhenOnLastPageWithExactLogCountMultiple_ShouldHaveCorrectTotalPageCount()
    {
        // Arrange
        var service = CreateService();
        var logCount = 30;
        var logs = CreateTestLogs(logCount);

        _logRepository.Setup(x => x.GetPaginatedLogsAsync(6, 5)).ReturnsAsync(logs);
        _logRepository.Setup(x => x.GetTotalLogCountAsync()).ReturnsAsync(logCount);

        // Act
        var result = await service.GetPaginatedLogsAsync(6, 5);

        // Assert
        _logRepository.Verify(x => x.GetPaginatedLogsAsync(6, 5), Times.AtMostOnce);
        _logRepository.Verify(x => x.GetTotalLogCountAsync(), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.TotalPages.Should().Be(6);
    }

    [Fact]
    public async Task GetPaginatedLogsAsync_WhenOnLastPageWithNotExactLogCountMultiple_ShouldHaveCorrectTotalPageCount()
    {
        // Arrange
        var service = CreateService();
        var logCount = 31;
        var logs = CreateTestLogs(logCount);

        _logRepository.Setup(x => x.GetPaginatedLogsAsync(7, 5)).ReturnsAsync(logs);
        _logRepository.Setup(x => x.GetTotalLogCountAsync()).ReturnsAsync(logCount);

        // Act
        var result = await service.GetPaginatedLogsAsync(7, 5);

        // Assert
        _logRepository.Verify(x => x.GetPaginatedLogsAsync(7, 5), Times.AtMostOnce);
        _logRepository.Verify(x => x.GetTotalLogCountAsync(), Times.AtMostOnce);

        result.Should().NotBeNull();
        result.TotalPages.Should().Be(7);
    }

    #endregion

    #region CreateAsync Tests

    [Fact]
    public async Task CreateAsync_WhenCalled_ShouldCallRepository()
    {
        // Arrange
        var service = CreateService();
        var log = CreateTestLogs(1).First();

        _logRepository.Setup(x => x.CreateAsync(log));

        // Act
        await service.CreateAsync(log.Action, log.UserId, log.Details);

        // Assert
        _logRepository.Verify(x => x.CreateAsync(log), Times.AtMostOnce);
    }

    [Fact]
    public async Task CreateAsync_WhenCalledWithAllValidParameters_ShouldCreateLog()
    {
        // Arrange
        var service = CreateService();
        Log? createdLog = null;

        _logRepository.Setup(x => x.CreateAsync(It.IsAny<Log>()))
            .Callback<Log>(log => createdLog = log)
            .Returns(Task.CompletedTask);

        const string action = "CREATE";
        const long userId = 1;
        const string details = "DETAILS";
        // Act
        await service.CreateAsync(action, userId, details);

        // Assert
        _logRepository.Verify(x => x.CreateAsync(It.IsAny<Log>()), Times.AtMostOnce);

        createdLog.Should().NotBeNull();
        createdLog.Action.Should().Be(action);
        createdLog.UserId.Should().Be(userId);
        createdLog.Details.Should().Be(details);
        createdLog.TimeStamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(10));
    }

    [Fact]
    public async Task CreateAsync_WhenCalledWithPartialButValidParameters_ShouldCreateLog()
    {
        // Arrange
        var service = CreateService();
        Log? createdLog = null;

        _logRepository.Setup(x => x.CreateAsync(It.IsAny<Log>()))
            .Callback<Log>(log => createdLog = log)
            .Returns(Task.CompletedTask);

        const string action = "CREATE";
        long? userId = null;
        const string details = "DETAILS";
        // Act
        await service.CreateAsync(action, userId, details);

        // Assert
        _logRepository.Verify(x => x.CreateAsync(It.IsAny<Log>()), Times.AtMostOnce);

        createdLog.Should().NotBeNull();
        createdLog.Action.Should().Be(action);
        createdLog.UserId.Should().Be(userId);
        createdLog.Details.Should().Be(details);
        createdLog.TimeStamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(10));
    }

    #endregion

    #region Helper functions

    private List<Log> CreateTestLogs(int logCount, long userId = 1)
    {
        var logs = new List<Log>();

        for (var i = 0; i < logCount; i++)
        {
            logs.Add(
                new Log
                {
                    Id = i + 1,
                    UserId = userId,
                    Action = i % 2 == 0 ? "CREATE" : "UPDATE",
                    Details = $"LOG DETAILS {i}",
                    TimeStamp = DateTime.Now.AddHours(-i)
                });
        }

        return logs;
    }

    #endregion
}
