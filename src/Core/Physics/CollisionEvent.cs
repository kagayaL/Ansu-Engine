using Core.ECS;
using Core.Events;

namespace Core.Physics;

// Сторона столкновения
public enum Side
{
    Top,
    Bottom,
    Left,
    Right
}

// Событие столкновения сущностей
public readonly record struct CollisionEvent(Entity Entity, Side Side, Entity? Other) : IEvent;
