using Core.ECS;
using Core.Events;

namespace Core.Events.Events;

// Событие уничтожения сущности
public readonly record struct EntityDestroyedEvent(Entity Entity) : IEvent;
