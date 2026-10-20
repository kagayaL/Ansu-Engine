using Core.Math;

namespace Core.Input;

public interface IInputProvider
{
    bool IsActionPressed(string action);
    bool IsActionDown(string action);
    Vector2 GetMousePosition();
    bool IsMouseLeftButtonPressed();
    bool IsMouseLeftButtonDown();
    bool IsMouseRightButtonPressed();
    bool IsMouseRightButtonDown();
}