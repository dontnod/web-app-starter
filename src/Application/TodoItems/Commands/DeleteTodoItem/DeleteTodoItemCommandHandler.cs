namespace WebAppStarter.Application.TodoItems.Commands.DeleteTodoItem;

using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebAppStarter.Application.Common.Interfaces;
using WebAppStarter.Domain.Entities;
using WebAppStarter.Domain.Events;

public class DeleteTodoItemCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<DeleteTodoItemCommand, Result<TodoItem>>
{
    public async Task<Result<TodoItem>> Handle(
        DeleteTodoItemCommand request,
        CancellationToken cancellationToken
    )
    {
        var toDoToDelete = await context.TodoItems.FirstOrDefaultAsync(todoItem =>
            todoItem.Id == request.Id
        );

        if (toDoToDelete is null)
        {
            return Result.NotFound();
        }

        if (toDoToDelete.Owner != currentUser.GetId() || currentUser.IsApplication())
        {
            return Result.Unauthorized();
        }

        var deletedTodo = context.TodoItems!.Remove(toDoToDelete);
        await context.SaveChangesAsync(cancellationToken);

        toDoToDelete.AddDomainEvent(new TodoItemDeletedEvent(toDoToDelete));

        return Result.Success(deletedTodo.Entity);
    }
}
