using System;

namespace Core.Math;

public readonly record struct Rect
{
    public float X { get; init; }
    public float Y { get; init; }
    public float Width { get; init; }
    public float Height { get; init; }

    public Rect(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public Rect(Vector2 position, Vector2 size)
    {
        X = position.X;
        Y = position.Y;
        Width = size.X;
        Height = size.Y;
    }

    // Границы прямоугольника
    public float Left => X;
    public float Right => X + Width;
    public float Top => Y;
    public float Bottom => Y + Height;

    // Удобные штуки
    public Vector2 Position => new(X, Y);
    public Vector2 Size => new(Width, Height);
    public Vector2 Center => new(X + Width * 0.5f, Y + Height * 0.5f);

    public static Rect Empty => new(0f, 0f, 0f, 0f);

    // Проверка на попадание точки в прямоугольник
    public bool Contains(Vector2 point)
    {
        return point.X >= Left && point.X <= Right &&
               point.Y >= Top && point.Y <= Bottom;
    }

    public bool Contains(float px, float py)
    {
        return px >= Left && px <= Right &&
               py >= Top && py <= Bottom;
    }

    // Проверка на полное попадание прямоугольника в другой прямоугольник 
    public bool Contains(Rect other)
    {
        return other.Left >= Left && other.Right <= Right &&
               other.Top >= Top && other.Bottom <= Bottom;
    }

    // Проверка на пересечение двух прямоугольников
    public bool Intersects(Rect other)
    {
        return Left < other.Right && Right > other.Left &&
               Top < other.Bottom && Bottom > other.Top;
    }

    // Возвращает прямоугольник, который является пересечением двух прямоугольников
    public Rect Overlap(Rect other)
    {
        if (!Intersects(other))
            return Empty;

        float minX = MathF.Max(Left, other.Left);
        float maxX = MathF.Min(Right, other.Right);
        float minY = MathF.Max(Top, other.Top);
        float maxY = MathF.Min(Bottom, other.Bottom);

        return new Rect(minX, minY, maxX - minX, maxY - minY);
    }
}