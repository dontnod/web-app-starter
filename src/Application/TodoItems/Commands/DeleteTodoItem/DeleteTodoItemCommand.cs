namespace WebAppStarter.Application.TodoItems.Commands.DeleteTodoItem;

using Ardalis.Result;
using MediatR;
using WebAppStarter.Domain.Entities;

public record DeleteTodoItemCommand(int Id) : IRequest<Result<TodoItem>>;
