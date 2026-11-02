using Microsoft.Xna.Framework;

namespace Render.Camera;

public class Camera2d
{
    // позиция камеры в мировых координатах
    public Vector2 Position { get; set;}

    //масштаб камеры, 1 = обычный масштаб, 2 = приближение в 2 раза, 0,5 - отдаление в 2 раза
    public float Zoom { get; set; } = 1f;

    //размеры экрана в пикселях
    private readonly int _screenWidth;
    private readonly int _screenHeight;

    // конструктор. камера стартует в точке (0, 0) мира
    public Camera2d(int screenWidth, int screenHeight)
    {
        _screenHeight = screenHeight;
        _screenWidth = screenWidth;
        Position = Vector2.Zero;
    }

    // 1.сдвигаем весь мир относительно камеры (на -позишн)
    // 2.создаем матрицу, которая масштабирует весь мир на зум
    // 3.после того как сдвинули мир и растянули карту так, как нам нужно,
    // ставим камеру в центр монитора
    // в моногейм матрицы применяются справа налево, поэтому пишем в обратном порядке
    public Matrix GetViewMatrix()
    {
       Matrix ViewMatrix = Matrix.CreateTranslation(_screenWidth/2f, _screenHeight/2f, 0) 
       * Matrix.CreateTranslation(-Position.X, -Position.Y, 0)  
       * Matrix.CreateScale(Zoom, Zoom, 1);
       return ViewMatrix;
    }

    // создаем обратную матрицу, чтобы узнавать реальные мировые координаты screenPos,
    public Vector2 ScreenToWorld (Vector2 screenPos)
    {
        Matrix ViewMatrix = GetViewMatrix();
        Matrix invertedViewMatrix = Matrix.Invert(ViewMatrix);
        Vector2 tarnsformedPosition = Vector2.Transform(screenPos, invertedViewMatrix);
        return tarnsformedPosition;
    }
}