namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi.Controllers;

public class ReadByIdTests
{
    [Fact]
    public async Task Put_UpdateByIdWhenItemUpdated_ReturnsNoContent()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        var request = new ToDoItemUpdateRequestDto(
            Name: "Jmeno",
            Description: "Popis",
            IsCompleted: false,
            Category: "HouseTasks"
        );
        var someId = 1;
        var readToDoItem = new ToDoItem { Name = "Jmeno", Description = "Popis", IsCompleted = false, ToDoItemId = someId };
        repositoryMock.ReadByIdAsync(someId).Returns(readToDoItem);

        // Act
        var result = await controller.UpdateByIdAsync(someId, request);

        // Assert
        Assert.IsType<NoContentResult>(result);
        await repositoryMock.Received(1).ReadByIdAsync(someId);
        await repositoryMock.Received(1).UpdateAsync(Arg.Any<ToDoItem>());
    }

    [Fact]
    public async Task Put_UpdateByIdWhenIdNotFound_ReturnsNotFound()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        var request = new ToDoItemUpdateRequestDto(
            Name: "Jmeno",
            Description: "Popis",
            IsCompleted: false,
            Category: "HouseTasks"
        );
        repositoryMock.ReadByIdAsync(Arg.Any<int>()).ReturnsNull();
        var someId = 1;

        // Act
        var result = await controller.UpdateByIdAsync(someId, request);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        await repositoryMock.Received(1).ReadByIdAsync(someId);
    }

    [Fact]
    public async Task Put_UpdateByIdUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        var request = new ToDoItemUpdateRequestDto(
            Name: "Jmeno",
            Description: "Popis",
            IsCompleted: false,
            Category: "HouseTasks"
        );
        var someId = 1;
        var readToDoItem = new ToDoItem { Name = "Jmeno", Description = "Popis", IsCompleted = false, ToDoItemId = someId };

        repositoryMock.ReadByIdAsync(Arg.Any<int>()).Returns(readToDoItem);
        repositoryMock.When(r => r.UpdateAsync(Arg.Any<ToDoItem>())).Do(r => throw new Exception());

        // Act
        var result = await controller.UpdateByIdAsync(someId, request);

        // Assert
        Assert.IsType<ObjectResult>(result);
        Assert.Equivalent(new StatusCodeResult(StatusCodes.Status500InternalServerError), result);
    }
}
