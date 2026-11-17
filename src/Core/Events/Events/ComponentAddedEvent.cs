using System;

using Core.ECS;
using Core.Events;

namespace Core.Events.Events;

// Событие добавления компонента к сущности
public readonly record struct ComponentAddedEvent(Entity Entity, Type ComponentType) : IEvent;
