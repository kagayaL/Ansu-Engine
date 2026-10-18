using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Render.Input;

public class InputManager
{
    //храним состояние клавиатуры и мыши для текущего и прошлого кадра
    private KeyboardState _currentKeyboard;
    private KeyboardState _previousKeyboard;
    
    private MouseState _currentMouse;
    private MouseState _previousMouse;

    //cловарь для маппинга: строковое имя действия -> конкретная клавиша
    private readonly Dictionary<string, Keys> _keyBindings;

    public InputManager()
    {
        _keyBindings = new Dictionary<string, Keys>
        {
            {"Jump", Keys.Space},
            {"MoveLeft", Keys.A},
            {"MoveRight", Keys.D},
            {"MoveStraight", Keys.W},
            {"MoveBack", Keys.S},
            {"Interact", Keys.E },
            {"Pause", Keys.Escape},
        };
    }

    public void Update()
    {
        _previousKeyboard = _currentKeyboard;
        _previousMouse = _currentMouse;

        _currentKeyboard = Keyboard.GetState();
        _currentMouse = Mouse.GetState();
    }
    
    //кнопка нажата 1 раз
    public bool IsActionPressed(string action)
    {
        if (!_keyBindings.TryGetValue(action, out Keys key)) return false;
        return _currentKeyboard.IsKeyDown(key) && _previousKeyboard.IsKeyUp(key);
    }

    //кнопка зажата
    public bool IsActionDown(string action)
    {
        if (!_keyBindings.TryGetValue(action, out Keys key)) 
        {
            return false;
        }
        return _currentKeyboard.IsKeyDown(key);
    }

    public Vector2 GetMousePosition()
    {
        return new Vector2(_currentMouse.X, _currentMouse.Y);
    }

    public bool IsMouseLeftButtonPressed()
    {
        return _currentMouse.LeftButton == ButtonState.Pressed && _previousMouse.LeftButton == ButtonState.Released;
    }

    public bool IsMouseLeftButtonDown()
    {
        return _currentMouse.LeftButton == ButtonState.Pressed;
    }

    public bool IsMouseRightButtonPressed()
    {
        return _currentMouse.RightButton == ButtonState.Pressed && _previousMouse.RightButton == ButtonState.Released;
    }

    public bool IsMouseRightButtonDown()
    {
        return _currentMouse.RightButton == ButtonState.Pressed;
    }
}