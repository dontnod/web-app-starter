namespace WebAppStarter.Application.TodoItems.Queries.GetUserTodoItems;

using Ardalis.Result;
using MediatR;
using WebAppStarter.Domain.Entities;

public record GetUserTodoItemsQuery : IRequest<Result<List<TodoItem>>> { }
