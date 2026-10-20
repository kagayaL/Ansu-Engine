using Core.ECS;
using Core.Math;

namespace Core.Input;

public struct InputComponent : IComponent
{
    public Vector2 Direction;
    public float Speed;

    public InputComponent(float speed = 200f)
    {
        Direction.X = 0;
        Direction.Y = 0;
        Speed = speed;
    }
}