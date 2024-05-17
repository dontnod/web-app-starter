namespace WebAppStarter.Application.FunctionalTests.TodoItems.Commands;

using Ardalis.Result;
using FluentAssertions;
using WebAppStarter.Application.Common.Exceptions;
using WebAppStarter.Application.TodoItems.Commands.CreateTodoItem;
using WebAppStarter.Domain.Entities;
using static WebAppStarter.Application.FunctionalTests.Testing;

public class CreateTodoItemTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateTodoItemCommand() { Description = string.Empty };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateTodoItem()
    {
        var userId = await RunAsDefaultUserAsync();

        var command = new CreateTodoItemCommand
        {
            Description = "Tasks",
        };

        Result<TodoItem> result = await SendAsync(command);

        result.IsSuccess.Should().BeTrue();

        var createdItem = result.Value;

        var item = await FindAsync<TodoItem>(createdItem.Id);

        item.Should().NotBeNull();

        item!.Description.Should().Be(command.Description);

        // item.CreatedBy.Should().Be(userId);
        // item.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
        // item.LastModifiedBy.Should().Be(userId);
        // item.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
