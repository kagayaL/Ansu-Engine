using Core.Math;
using Core.Input;
using Render.Input;
namespace Runtime;
public class GameInputProvider : IInputProvider
{
    private InputManager _manager;

    public GameInputProvider(InputManager inputManager)
    {
        _manager = inputManager;
    }
    public bool IsActionPressed(string action)
    {
        return _manager.IsActionPressed(action);
    }

    public bool IsActionDown(string action)
    {
        return _manager.IsActionDown(action);
    }

    public Vector2 GetMousePosition()
    {
        var mousePosition = _manager.GetMousePosition();
        return new(mousePosition.X, mousePosition.Y);
    }

    public bool IsMouseLeftButtonPressed()
    {
        return _manager.IsMouseLeftButtonPressed();
    }

    public bool IsMouseLeftButtonDown()
    {
        return _manager.IsMouseLeftButtonDown();
    }

    public bool IsMouseRightButtonPressed()
    {
        return _manager.IsMouseRightButtonPressed();
    }

    public bool IsMouseRightButtonDown()
    {
        return _manager.IsMouseRightButtonDown();
    }
}
