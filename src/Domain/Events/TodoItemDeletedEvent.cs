namespace WebAppStarter.Domain.Events;

using WebAppStarter.Domain.Common;
using WebAppStarter.Domain.Entities;

public class TodoItemDeletedEvent : BaseEvent
{
    public TodoItemDeletedEvent(TodoItem item)
    {
        Item = item;
    }

    public TodoItem Item { get; }
}
