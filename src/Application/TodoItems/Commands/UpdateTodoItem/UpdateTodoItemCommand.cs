namespace WebAppStarter.Application.TodoItems.Commands.UpdateTodoItem;

using Ardalis.Result;
using MediatR;
using WebAppStarter.Domain.Entities;

// Could have a base class in common with CreateToDoItemCommand,
// which would have the Validatior for Desc?
public record UpdateTodoItemCommand : IRequest<Result<TodoItem>>
{
    public required int Id { get; init; }

    public required string Description { get; init; }
}
