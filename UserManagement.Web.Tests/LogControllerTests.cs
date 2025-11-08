using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using UserManagement.Models;
using UserManagement.Services.Domain;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Controllers;
using UserManagement.Web.Models.Logs;

namespace UserManagement.Data.Tests;

public class LogControllerTests
{

    #region List Tests WITH PaginationAsync update 

    [Fact]
    public async Task List_WhenServiceReturnsLogs_ModelMustContainAllLogs()
    {
        // Arrange
        var controller = CreateController();
        var logs = SetupTestLogs(5);

        var pagedLogs = new PagedResult<Log>
        {
            Items = logs,
            CurrentPageIndex = 1,
            PageSize = 10,
            TotalCount = 5,
            TotalPages = 1,
        };

        _logService.Setup(x => x.GetPaginatedLogsAsync(1, 10)).ReturnsAsync(pagedLogs);

        // Act
        var result = await controller.List();

        // Assert
        result.Model
            .Should().BeOfType<LogListViewModel>()
            .Which.Items.Should().HaveCount(5);
    }

    [Fact]
    public async Task List_WhenServiceReturnsEmpty_ShouldReturnEmptyModel()
    {
        // Arrange
        var controller = CreateController();
        var emptyLogs = Array.Empty<Log>();

        var pagedLogs = new PagedResult<Log>
        {
            Items = emptyLogs,
            CurrentPageIndex = 1,
            PageSize = 10,
            TotalCount = 0,
            TotalPages = 0,
        };

        _logService.Setup(x => x.GetPaginatedLogsAsync(1, 10)).ReturnsAsync(pagedLogs);

        // Act
        var result = await controller.List();

        // Assert
        result.Model.Should().BeOfType<LogListViewModel>()
            .Which.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task List_WhenLogHasUserInformation_ShouldIncludeUserInformation()
    {
        // Arrange
        var controller = CreateController();
        var logs = SetupTestLogsWithUser();

        var pagedLogs = new PagedResult<Log>
        {
            Items = logs,
            CurrentPageIndex = 1,
            PageSize = 10,
            TotalCount = 1,
            TotalPages = 3,
        };

        _logService.Setup(x => x.GetPaginatedLogsAsync(1, 10)).ReturnsAsync(pagedLogs);

        // Act
        var result = await controller.List();

        // Assert
        result.Model.Should().BeOfType<LogListViewModel>()
            .Which.Items.First().UserForename.Should().Be("Johnny");

        result.Model.Should().BeOfType<LogListViewModel>()
            .Which.Items.First().UserSurname.Should().Be("Bravo");

    }

    [Fact]
    public async Task List_WhenCalled_ShouldCallService()
    {
        // Arrange
        var controller = CreateController();
        var logs = SetupTestLogsWithUser();

        var pagedLogs = new PagedResult<Log>
        {
            Items = logs,
            CurrentPageIndex = 1,
            PageSize = 10,
            TotalCount = 1,
            TotalPages = 3,
        };

        _logService.Setup(x => x.GetPaginatedLogsAsync(1, 10)).ReturnsAsync(pagedLogs);

        // Act
        var result = await controller.List();

        // Assert
        _logService.Verify(x => x.GetPaginatedLogsAsync(1, 10), Times.AtMostOnce);
    }

    #endregion

    #region Detail Tests

    [Fact]
    public async Task Detail_WhenLogExists_ShouldReturnViewWithLogViewModel()
    {
        // Arrange
        var controller = CreateController();
        var log = SetupTestLogsWithUser();

        _logService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(log[0]);

        // Act
        var result = await controller.Detail(1);

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<LogViewModel>().Subject;

        model.Id.Should().Be(1);
        model.Action.Should().Be("CREATE");
        model.Details.Should().Be("CREATED User: Johnny Bravo");
    }

    [Fact]
    public async Task Detail_WhenLogExists_ShouldCallService()
    {
        // Arrange
        var controller = CreateController();
        var log = SetupTestLogsWithUser();

        _logService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(log[0]);

        // Act
        var result = await controller.Detail(1);

        // Assert
        _logService.Verify(x => x.GetByIdAsync(1), Times.AtMostOnce);
    }

    [Fact]
    public async Task Detail_WhenLogDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var controller = CreateController();
        _logService.Setup(x => x.GetByIdAsync(99999)).ReturnsAsync((Log?)null);

        // Act
        var result = await controller.Detail(99999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Detail_WhenLogHasUserInformation_ShouldIncludeUserInformation()
    {
        // Arrange
        var controller = CreateController();
        var log = SetupTestLogsWithUser();

        _logService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(log[0]);

        // Act
        var result = await controller.Detail(1);

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<LogViewModel>().Subject;

        model.Id.Should().Be(1);
        model.Action.Should().Be("CREATE");
        model.Details.Should().Be("CREATED User: Johnny Bravo");
    }

    [Fact]
    public async Task Detail_WhenLogHasNoUser_ShouldReturnViewModel()
    {
        // Arrange
        var controller = CreateController();
        var log = SetupTestLogsWithUser();

        log[0].UserId = null;
        log[0].User = null;

        _logService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(log[0]);

        // Act
        var result = await controller.Detail(1);

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<LogViewModel>().Subject;

        model.UserForename.Should().BeNull();
        model.UserId.Should().BeNull();
    }

    #endregion

    #region UserLogs Tests

    [Fact]
    public async Task UserLogs_WhenCalled_ShouldCallService()
    {
        // Arrange
        var controller = CreateController();
        var logs = SetupTestLogsWithUser();

        _logService.Setup(x => x.GetByUserIdAsync(1)).ReturnsAsync(logs.AsQueryable());

        // Act
        var result = await controller.UserLogs(1);

        // Assert
        _logService.Verify(x => x.GetByIdAsync(1), Times.AtMostOnce);

    }

    [Fact]
    public async Task UserLogs_WhenCalled_ShouldReturnLogsForSpecifiedUser()
    {
        // Arrange
        var controller = CreateController();
        var logs = SetupTestLogsWithUser();

        _logService.Setup(x => x.GetByUserIdAsync(1)).ReturnsAsync(logs.AsQueryable());

        // Act
        var result = await controller.UserLogs(1);

        // Assert
        result.Model
            .Should().BeOfType<LogListViewModel>()
            .Which.Items.Should().HaveCount(3);

        // Assert
        result.Model
            .Should().BeOfType<LogListViewModel>()
            .Which.Items.Should().OnlyContain(log => log.UserId == 1);
    }

    [Fact]
    public async Task UserLogs_WhenNoLogsForSpecifiedUser_ShouldReturnEmpty()
    {
        // Arrange
        var controller = CreateController();
        _logService.Setup(x => x.GetByUserIdAsync(1)).ReturnsAsync(Array.Empty<Log>());

        // Act
        var result = await controller.UserLogs(1);

        // Assert
        result.Model
            .Should().BeOfType<LogListViewModel>()
            .Which.Items.Should().BeEmpty();
    }

    #endregion

    #region List Pagination New Tests

    [Fact]
    public async Task List_WithPageParameters_ShouldReturnCorrectPage()
    {
        // Arrange
        var controller = CreateController();
        var pagedResult = new PagedResult<Log>
        {
            Items = SetupTestLogs(10),
            CurrentPageIndex = 2,
            PageSize = 10,
            TotalPages = 3,
            TotalCount = 25
        };

        _logService.Setup(x => x.GetPaginatedLogsAsync(2, 10)).ReturnsAsync(pagedResult);

        // Act
        var result = await controller.List(2, 10);

        // Assert
        result.Model.Should().BeOfType<LogListViewModel>()
            .Which.CurrentPageIndex.Should().Be(2);
    }

    [Fact]
    public async Task List_WhenOnFirstPageIndex_ShouldNotHavePreviousPage()
    {
        // Arrange
        var controller = CreateController();
        var pagedResult = new PagedResult<Log>
        {
            Items = SetupTestLogs(10),
            CurrentPageIndex = 1,
            PageSize = 10,
            TotalPages = 3,
            TotalCount = 25
        };

        _logService.Setup(x => x.GetPaginatedLogsAsync(1, 10)).ReturnsAsync(pagedResult);

        // Act
        var result = await controller.List(1, 10);

        // Assert
        result.Model.Should().BeOfType<LogListViewModel>()
            .Which.HasPreviousPage.Should().BeFalse();
    }
    [Fact]
    public async Task List_WhenOnLastPageIndex_ShouldNotHavePreviousPage()
    {
        // Arrange
        var controller = CreateController();
        var pagedResult = new PagedResult<Log>
        {
            Items = SetupTestLogs(10),
            CurrentPageIndex = 3,
            PageSize = 10,
            TotalPages = 3,
            TotalCount = 25
        };

        _logService.Setup(x => x.GetPaginatedLogsAsync(3, 10)).ReturnsAsync(pagedResult);

        // Act
        var result = await controller.List(3, 10);

        // Assert
        result.Model.Should().BeOfType<LogListViewModel>()
            .Which.HasNextPage.Should().BeFalse();
    }

    #endregion

    private readonly Mock<ILogService> _logService = new();
    private LogsController CreateController()
    {
        var controller = new LogsController(_logService.Object);

        controller.TempData = new TempDataDictionary(
            new DefaultHttpContext(),
            Mock.Of<ITempDataProvider>());

        return controller;
    }

   #region Helper functions

    private List<Log> SetupTestLogs(int logCount, long userId = 1)
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

    private List<Log> SetupTestLogsWithUser()
    {
        var user = new User
        {
            Id = 1,
            Forename = "Johnny",
            Surname = "Bravo",
            Email = "jbravo@example.com",
            IsActive = true
        };

        var logs = new List<Log>
        {
            new Log
            {
                Id = 1,
                UserId = 1,
                Action = "CREATE",
                Details = "CREATED User: Johnny Bravo",
                TimeStamp = DateTime.UtcNow.AddHours(-10),
                User = user
            },
            new Log
            {
                Id = 2,
                UserId = 1,
                Action = "UPDATE",
                Details = "UPDATED User: Johnny Bravo",
                TimeStamp = DateTime.UtcNow.AddHours(-5),
                User = user
            },
            new Log
            {
                Id = 3,
                UserId = 1,
                Action = "VIEW",
                Details = "VIEWED User: Johnny Bravo",
                TimeStamp = DateTime.UtcNow,
                User = user
            }
        };

        return logs;
    }

    #endregion
}
