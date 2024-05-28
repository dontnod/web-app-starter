namespace WebAppStarter.Application.TodoItems.Commands.CreateTodoItem;

using Ardalis.Result;
using MediatR;
using WebAppStarter.Application.Common.Interfaces;
using WebAppStarter.Domain.Entities;
using WebAppStarter.Domain.Events;

public class CreateTodoItemCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<CreateTodoItemCommand, Result<TodoItem>>
{
    public async Task<Result<TodoItem>> Handle(
        CreateTodoItemCommand request,
        CancellationToken cancellationToken
    )
    {
        Guid? ownerIdOfTodo = currentUser.IsApplication() ? request.Owner : currentUser.GetId();

        if (!ownerIdOfTodo.HasValue)
        {
            // Error list?
            return Result.Error(new ErrorList(["Failed to determine id of the owner"], null));
        }

        var entity = new TodoItem
        {
            Owner = ownerIdOfTodo.Value,
            Description = request.Description,
        };

        await context.TodoItems.AddAsync(entity, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        entity.AddDomainEvent(new TodoItemCreatedEvent(entity));

        return Result<TodoItem>.Success(entity);
    }
}
