namespace WebAppStarter.Application.TodoItems.Queries.GetUserTodoItems;

using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebAppStarter.Application.Common.Interfaces;
using WebAppStarter.Domain.Entities;

public class GetUserTodoItemsQueryHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<GetUserTodoItemsQuery, Result<List<TodoItem>>>
{
    public async Task<Result<List<TodoItem>>> Handle(
        GetUserTodoItemsQuery request,
        CancellationToken cancellationToken
    )
    {
        // Clearer without ternary IMHO
        var query = context.TodoItems.AsQueryable();

        if (!currentUser.IsApplication())
        {
            query = query.Where(todoItem => todoItem.Owner == currentUser.GetId());
        }

        var todoItems = await query.ToListAsync(cancellationToken);

        return Result.Success(todoItems);
    }
}
