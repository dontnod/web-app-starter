namespace WebAppStarter.Domain.Events;

using WebAppStarter.Domain.Common;
using WebAppStarter.Domain.Entities;

public class TodoItemCompletedEvent : BaseEvent
{
    public TodoItemCompletedEvent(TodoItem item)
    {
        Item = item;
    }

    public TodoItem Item { get; }
}
