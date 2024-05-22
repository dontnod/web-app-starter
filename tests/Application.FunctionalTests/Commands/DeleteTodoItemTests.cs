namespace WebAppStarter.Application.FunctionalTests.TodoItems.Commands;

using Ardalis.Result;
using FluentAssertions;
using WebAppStarter.Application.TodoItems.Commands.CreateTodoItem;
using WebAppStarter.Application.TodoItems.Commands.DeleteTodoItem;
using WebAppStarter.Domain.Entities;
using static WebAppStarter.Application.FunctionalTests.Testing;

public class DeleteTodoItemTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidTodoItemId()
    {
        var command = new DeleteTodoItemCommand(99);
        var result = await SendAsync(command);

        result.Status.Should().Be(ResultStatus.NotFound);
    }

    [Test]
    public async Task ShouldDeleteTodoItem()
    {
        var userId = await RunAsDefaultUserAsync();

        var result = await SendAsync(new CreateTodoItemCommand { Description = "New Item", });

        result.IsSuccess.Should().BeTrue();

        var createdItem = result.Value;

        await SendAsync(new DeleteTodoItemCommand(createdItem.Id));

        var deletedItem = await FindAsync<TodoItem>(createdItem.Id);

        deletedItem.Should().BeNull();
    }
}
