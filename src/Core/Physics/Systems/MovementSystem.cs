using Core.ECS;
using Core.Physics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Physics.Systems;

public class MovementSystem : ECS.ISystem
{
    public void Update(World world, float deltaTime)
    {
        foreach (var (entityId, velocity) in world.GetPool<Velocity>())
        {
            var entity = new Entity(entityId);

            if (world.HasComponent<Transform>(entity))
            {
                ref var transform = ref world.GetComponent<Transform>(entity);
                transform.Position += velocity.Direction * deltaTime;
            }
        }
    }
}

