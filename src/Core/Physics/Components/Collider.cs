namespace Core.Physics.Components;

using Core.ECS;
using Core.Math;

public struct Collider : IComponent
{
    public Rect Bounds;
    public Vector2 Offset;
    public bool IsOneWay;
    public float Bounciness;

    public Collider(Rect bounds, Vector2 offset = default, bool isOneWay = false, float bounciness = 0f)
    {
        Bounds = bounds;
        Offset = offset;
        IsOneWay = isOneWay;
        Bounciness = bounciness;
    }

    public Rect GetWorldBounds(Vector2 position)
    {
        return new Rect(position.X + Offset.X + Bounds.X, position.Y + Offset.Y + Bounds.Y, Bounds.Width, Bounds.Height);
    }
}
