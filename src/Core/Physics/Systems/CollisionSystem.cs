namespace Core.Physics.Systems;

using System.Collections.Generic;
using Core.ECS;
using Core.Events;
using Core.Math;
using Core.Physics.Components;

public class CollisionSystem : ISystem
{
    private readonly EventBus _eventBus;

    public CollisionSystem(EventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public void Update(World world, float deltaTime)
    {
        var entities = new List<Entity>();
        foreach (var entity in world.Query<Transform, Collider>())
        {
            entities.Add(entity);
        }

        for (int i = 0; i < entities.Count; i++)
        {
            Entity e1 = entities[i];
            ref var transform1 = ref world.GetComponent<Transform>(e1);
            ref var collider1 = ref world.GetComponent<Collider>(e1);
            Rect bounds1 = collider1.GetWorldBounds(transform1.Position);

            for (int j = i + 1; j < entities.Count; j++)
            {
                Entity e2 = entities[j];
                ref var transform2 = ref world.GetComponent<Transform>(e2);
                ref var collider2 = ref world.GetComponent<Collider>(e2);
                Rect bounds2 = collider2.GetWorldBounds(transform2.Position);

                if (bounds1.Intersects(bounds2))
                {
                    ResolveCollision(e1, bounds1, collider1, e2, bounds2, collider2);
                }
            }
        }
    }

    private void ResolveCollision(
        Entity e1, Rect bounds1, Collider collider1,
        Entity e2, Rect bounds2, Collider collider2)
    {
        Rect overlap = bounds1.Overlap(bounds2);

        Side side1;
        Side side2;

        if (overlap.Width < overlap.Height)
        {
            if (bounds1.X < bounds2.X)
            {
                side1 = Side.Right;
                side2 = Side.Left;
            }
            else
            {
                side1 = Side.Left;
                side2 = Side.Right;
            }
        }
        else
        {
            if (bounds1.Y < bounds2.Y)
            {
                side1 = Side.Bottom;
                side2 = Side.Top;
            }
            else
            {
                side1 = Side.Top;
                side2 = Side.Bottom;
            }
        }

        // Односторонняя платформа пропускает объект, если столкновение происходит не сверху
        if (collider2.IsOneWay && side1 != Side.Bottom)
        {
            return;
        }

        if (collider1.IsOneWay && side2 != Side.Bottom)
        {
            return;
        }

        _eventBus.Publish(new CollisionEvent(e1, side1, e2));
        _eventBus.Publish(new CollisionEvent(e2, side2, e1));
    }
}
