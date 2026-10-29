using Core.ECS;
using Core.Math;

namespace Core.Physics.Components;

public struct Transform : IComponent
{
    public Vector2 Position;
    public float Rotation; // Угол в радианах
    public Vector2 Scale;
    public Vector2 Origin;

    public Transform()
    {
        Position = Vector2.Zero;
        Rotation = 0f;
        Scale = Vector2.One;
        Origin = Vector2.Half;
    }
    public Transform(float xPosition, float yPosition, float rotation = 0f, Vector2? origin = null, Vector2? scale = null)
    {
        Position = new Vector2(xPosition, yPosition);
        Rotation = rotation;
        Scale = scale ?? Vector2.One;
        Origin = origin ?? Vector2.Half;

    }

    public Transform(Vector2 position, float rotation, Vector2? scale= null, Vector2? origin = null)
    {
        Position = position;
        Rotation = rotation;
        Scale = scale ?? Vector2.One;
        Origin = origin ?? Vector2.Half;
    }

    public static Transform Default => new(Vector2.Zero, 0f, Vector2.One, Vector2.Half);
}