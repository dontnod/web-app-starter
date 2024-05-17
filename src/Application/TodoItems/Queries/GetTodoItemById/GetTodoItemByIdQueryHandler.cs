namespace WebAppStarter.Application.TodoItems.Queries.GetTodoItemById;

using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebAppStarter.Application.Common.Interfaces;
using WebAppStarter.Domain.Entities;

public class GetTodoItemByIdQueryHandler(IApplicationDbContext context, ICurrentUser currentUser) : IRequestHandler<GetTodoItemByIdQuery, Result<TodoItem>>
{
    public async Task<Result<TodoItem>> Handle(GetTodoItemByIdQuery request, CancellationToken cancellationToken)
    {
        var todoItem = await context.TodoItems
            .FirstOrDefaultAsync(td => td.Id == request.Id);

        if (todoItem == null)
        {
            return Result.NotFound($"No todo item found by id: {request.Id}");
        }

        if (todoItem.Owner != currentUser.GetId() || currentUser.IsApplication())
        {
            return Result.Unauthorized();
        }

        return Result.Success(todoItem);
    }
}
