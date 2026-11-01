using Core.ECS;
using Core.Events;

namespace Core.Events.Events;

// Событие создания сущности
public readonly record struct EntityCreatedEvent(Entity Entity) : IEvent;
