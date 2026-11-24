using Core.ECS;
using Core.Physics.Components;
using Core.Math;

public class GravitySystem : ISystem
{
    public void Update(World world, float deltaTime)
    {
        foreach (var entity in world.Query<Velocity, Gravity>())
        {
            ref Gravity gravity = ref world.GetComponent<Gravity>(entity);
            if (gravity.IsAffectedByGravity)
            {
                ref Velocity velocity = ref world.GetComponent<Velocity>(entity);
                var direction = velocity.Direction;
                direction.Y += gravity.Force * deltaTime;
                velocity.Direction = direction;
            }
        }
    }
}
