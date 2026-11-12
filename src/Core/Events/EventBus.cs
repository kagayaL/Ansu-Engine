namespace Core.Events;

// Шина событий для подписки и публикации
public class EventBus
{
    private readonly Dictionary<Type, List<Delegate>> _subscribers = new();

    // Подписаться на событие определенного типа
    public void Subscribe<T>(Action<T> handler) where T : IEvent
    {
        var type = typeof(T);
        if (!_subscribers.TryGetValue(type, out var handlers))
        {
            handlers = new List<Delegate>();
            _subscribers[type] = handlers;
        }
        handlers.Add(handler);
    }

    // Отписаться от события
    public void Unsubscribe<T>(Action<T> handler) where T : IEvent
    {
        var type = typeof(T);
        if (_subscribers.TryGetValue(type, out var handlers))
        {
            handlers.Remove(handler);
        }
    }

    // Опубликовать событие для всех подписчиков
    public void Publish<T>(T eventData) where T : IEvent
    {
        var type = typeof(T);
        if (_subscribers.TryGetValue(type, out var handlers))
        {
            foreach (var handler in handlers.ToArray())
            {
                ((Action<T>)handler)(eventData);
            }
        }
    }
}
