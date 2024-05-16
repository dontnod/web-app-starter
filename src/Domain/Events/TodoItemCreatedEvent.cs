namespace WebAppStarter.Domain.Events;

using WebAppStarter.Domain.Common;
using WebAppStarter.Domain.Entities;

public class TodoItemCreatedEvent : BaseEvent
{
    public TodoItemCreatedEvent(TodoItem item)
    {
        Item = item;
    }

    public TodoItem Item { get; }
}
