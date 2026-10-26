using Core.ECS;
using Core.Math;

namespace Core.Physics.Components;

public struct Transform : IComponent
{
    public Vector2 Position;
    public float Rotation; // Угол в радианах
    public Vector2 Scale;


    public Transform()
    {
        Position = Vector2.Zero;
        Rotation = 0f;
        Scale = Vector2.One;
    }
    public Transform(float xPosition, float yPosition)
    {
        Position = new Vector2(xPosition, yPosition);
        Rotation = 0f;
        Scale = Vector2.One;
    }

    public Transform(Vector2 position, float rotation, Vector2 scale)
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }

    public static Transform Default => new(Vector2.Zero, 0f, Vector2.One);
}