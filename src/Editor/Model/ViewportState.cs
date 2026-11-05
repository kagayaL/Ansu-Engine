using Microsoft.Xna.Framework;
namespace Editor.Model;

// создание вьюпорта редактора
public sealed class ViewportState
{
    public bool IsMouseInside { get; set; } // курсор внутри вьюпорта 
    public Vector2 MouseWorldPosition { get; set; } // где в мире находится точка под курсором
    public Rectangle ScreenBounds { get; set; } // прямоугольник вьюпорта в экранных коор-х
}