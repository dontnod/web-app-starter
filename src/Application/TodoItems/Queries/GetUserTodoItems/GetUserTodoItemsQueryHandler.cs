namespace WebAppStarter.Application.TodoItems.Queries.GetUserTodoItems;

using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebAppStarter.Application.Common.Interfaces;
using WebAppStarter.Domain.Entities;

public class GetUserTodoItemsQueryHandler(IApplicationDbContext context, ICurrentUser currentUser) : IRequestHandler<GetUserTodoItemsQuery, Result<List<TodoItem>>>
{
    public async Task<Result<List<TodoItem>>> Handle(GetUserTodoItemsQuery request, CancellationToken cancellationToken)
    {
        var todoItems = currentUser.IsApplication() ? await context.TodoItems
            .ToListAsync() : await context.TodoItems
            .Where(todoItem => todoItem.Owner == currentUser.GetId())
            .ToListAsync();

        return Result.Success(todoItems);
    }
}
