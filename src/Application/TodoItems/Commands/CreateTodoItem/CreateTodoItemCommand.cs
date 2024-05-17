namespace WebAppStarter.Application.TodoItems.Commands.CreateTodoItem;

using Ardalis.Result;
using MediatR;
using WebAppStarter.Domain.Entities;

public record CreateTodoItemCommand : IRequest<Result<TodoItem>>
{
    public required string Description { get; init; }

    public Guid? Owner { get; init; }
}
