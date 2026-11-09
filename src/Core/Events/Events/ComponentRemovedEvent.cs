using System;

using Core.ECS;
using Core.Events;

namespace Core.Events.Events;

// Событие удаления компонента у сущности
public readonly record struct ComponentRemovedEvent(Entity Entity, Type ComponentType) : IEvent;
