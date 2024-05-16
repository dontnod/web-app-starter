namespace WebAppStarter.Application.TodoItems.Queries.GetTodoItemById;

using Ardalis.Result;
using MediatR;
using WebAppStarter.Domain.Entities;

public record GetTodoItemByIdQuery(int Id) : IRequest<Result<TodoItem>>;
