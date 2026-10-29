using Core.ECS;
using Core.Input;
using Core.Physics.Components;

namespace Core.Physics.Systems;

public class PlayerMovementSystem : ISystem
{
    public void Update(World world, float dt)
    {
        var inputPool = world.GetPool<InputComponent>();

        foreach (int entityId in inputPool.Keys)
        {
            var entity = new Entity(entityId);

            if (!world.HasComponent<Velocity>(entity))
                continue;

            ref var input = ref world.GetComponent<InputComponent>(entity);
            ref var velocity = ref world.GetComponent<Velocity>(entity);

            velocity.Direction = input.Direction * input.Speed;
            
        }
    }
}