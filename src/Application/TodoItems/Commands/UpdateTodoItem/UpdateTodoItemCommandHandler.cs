namespace WebAppStarter.Application.TodoItems.Commands.UpdateTodoItem;

using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebAppStarter.Application.Common.Interfaces;
using WebAppStarter.Domain.Entities;

public class UpdateTodoItemCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<UpdateTodoItemCommand, Result<TodoItem>>
{
    public async Task<Result<TodoItem>> Handle(
        UpdateTodoItemCommand request,
        CancellationToken cancellationToken
    )
    {
        var storedTodoItem = await context.TodoItems.FirstOrDefaultAsync(todoItem =>
            todoItem.Id == request.Id
        );

        if (storedTodoItem is null)
        {
            return Result.NotFound();
        }

        if (storedTodoItem.Owner != currentUser.GetId() || currentUser.IsApplication())
        {
            return Result.Unauthorized();
        }

        storedTodoItem.Description = request.Description;

        context.TodoItems!.Update(storedTodoItem);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(storedTodoItem);
    }
}
