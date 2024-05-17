namespace WebAppStarter.Application.TodoItems.Commands.UpdateTodoItem;

using Ardalis.Result;
using MediatR;
using WebAppStarter.Domain.Entities;

public record UpdateTodoItemCommand : IRequest<Result<TodoItem>>
{
    public required int Id { get; init; }

    public required string Description { get; init; }
}