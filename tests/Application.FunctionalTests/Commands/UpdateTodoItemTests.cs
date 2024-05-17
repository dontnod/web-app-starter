namespace WebAppStarter.Application.FunctionalTests.TodoItems.Commands;

using Ardalis.Result;
using FluentAssertions;
using WebAppStarter.Application.TodoItems.Commands.CreateTodoItem;
using WebAppStarter.Application.TodoItems.Commands.UpdateTodoItem;
using WebAppStarter.Domain.Entities;
using static WebAppStarter.Application.FunctionalTests.Testing;

public class UpdateTodoItemTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidTodoItemId()
    {
        var command = new UpdateTodoItemCommand { Id = 99, Description = "New Title" };

        var result = await SendAsync(command);

        result.Status.Should().Be(ResultStatus.NotFound);
    }

    [Test]
    public async Task ShouldUpdateTodoItem()
    {
        var userId = await RunAsDefaultUserAsync();

        var createResult = await SendAsync(new CreateTodoItemCommand
        {
            Description = "New Item",
        });

        var command = new UpdateTodoItemCommand
        {
            Id = createResult.Value.Id,
            Description = "Updated Item Title",
        };

        var updateResult = await SendAsync(command);

        updateResult.IsSuccess.Should().BeTrue();

        var item = await FindAsync<TodoItem>(createResult.Value.Id);

        item.Should().NotBeNull();
        item!.Description.Should().Be(command.Description);

        item.LastModifiedBy.Should().NotBeNull();
        item.LastModifiedBy.Should().Be(userId);
        item.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
