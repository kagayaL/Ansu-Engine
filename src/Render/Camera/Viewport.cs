using Microsoft.Xna.Framework;

namespace Render.Camera;

// класс вьюпорт отвечает за определение видимой области камеры в мировых координатах
// используется для оптимизации (Culling): 
// чтобы не рисовать объекты, которые находятся за пределами экрана
public class Viewport
{
    private readonly Camera2d _camera;
    private readonly int _screenWidth;
    private readonly int _screenHeight;

    public Viewport(Camera2d camera, int screenWidth, int screenHeight)
    {
        _camera = camera;
        _screenWidth = screenWidth;
        _screenHeight = screenHeight;
    }

    // вычисляет и возвращает прямоугольник видимой области в мировых координатах
    // этот прямоугольник динамически меняется при движении камеры или изменении зума
    public Rectangle GetWorldBounds()
    {
        Vector2 topLeft = _camera.ScreenToWorld(Vector2.Zero);
        Vector2 bottomRight = _camera.ScreenToWorld(new Vector2(_screenWidth, _screenHeight));

        int width = (int)(bottomRight.X - topLeft.X);
        int height = (int)(bottomRight.Y - topLeft.Y);

        return new Rectangle ((int)topLeft.X, (int)topLeft.Y, width, height);
    }

    // проверяем, виден ли объект в наших границах
    // метод возвращает true, если два прямоугольника перекрываются (объект виден полностью), 
    // и false в противном случае
    public bool IsVisible(Rectangle objectBounds)
    {
        return objectBounds.Intersects(GetWorldBounds());
    }
}