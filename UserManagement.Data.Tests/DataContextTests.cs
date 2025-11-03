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
        context.Delete(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = context.GetAll<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotContain(s => s.Email == entity.Email);
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
            Forename = "James",
            Surname = "Peach",
            Email = "AGiantPeach@example.com",
            DateOfBirth = dateOfBirth
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
            Forename = "James",
            Surname = "Aurthur",
            Email = "WillNotLetGo@example.com",
            DateOfBirth = null
        };

        context.Create(entity);
        // Act
        var result = context.GetAll<User>();

        // Assert
        result
            .Should().Contain(u => u.Id == entity.Id)
            .Which.DateOfBirth.Should().Be(entity.DateOfBirth);
    }

    private DataContext CreateContext() => new();
}
