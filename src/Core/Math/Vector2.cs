using System;

namespace Core.Math;

public struct Vector2
{
    public float X { get; set; }
    public float Y { get; set; }

    public Vector2(float x, float y)
    {
        X = x;
        Y = y;
    }

    public static Vector2 Zero => new(0f, 0f);
    public static Vector2 Half => new(0.5f, 0.5f);
    public static Vector2 One => new(1f, 1f);

    // Длина вектора
    public float Length => MathF.Sqrt(X * X + Y * Y);

    // Квадрат длины, чтобы не брать корень и быстрее стравнивать
    public float LengthSquared => X * X + Y * Y;

    // Нормализация
    public void Normalize()
    {
        float len = Length;
        if (len > 1e-6f)
        {
            X /= len;
            Y /= len;
        }
        else
        {
            X = 0f;
            Y = 0f;
        }
    }

    // Расстояние между двумя точками
    public static float Distance(Vector2 a, Vector2 b) => (a - b).Length;


    public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);
    public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.X - b.X, a.Y - b.Y);
    public static Vector2 operator *(Vector2 a, float scalar) => new(a.X * scalar, a.Y * scalar);
    public static Vector2 operator *(float scalar, Vector2 a) => new(a.X * scalar, a.Y * scalar);
    public static Vector2 operator /(Vector2 a, float scalar) => new(a.X / scalar, a.Y / scalar);

    public override string ToString() => $"({X}, {Y})";
}