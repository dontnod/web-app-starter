namespace WebAppStarter.Application.TodoItems.Commands.CreateTodoItem;

using FluentValidation;

public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    public CreateTodoItemCommandValidator()
    {
        RuleFor(v => v.Description)
            .MaximumLength(255)
            .NotEmpty();
    }
}
