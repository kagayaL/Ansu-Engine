using Xunit;

using Core.Math;
using Core.ECS;
using Core.Physics.Systems;
using Core.Input;

namespace Tests.Core;

// Фейковый класс для теста InputSystem, эмулирующий нажатия игрока.
// В нем зафиксированно состояние когда игрок зажал кнопку W,
// обе кнопки мыши и установил курсор на позицию (0,0)
public class FakeInput : IInputProvider
{
    public bool IsActionDown(string action) => action == "MoveStraight";
    public bool IsActionPressed(string action) => action == "MoveStraight";
    public Vector2 GetMousePosition() => new Vector2(0, 0);
    public bool IsMouseLeftButtonPressed() => true;
    public bool IsMouseLeftButtonDown() => true;
    public bool IsMouseRightButtonPressed() => true;
    public bool IsMouseRightButtonDown() => true;
}
public class InputSystemTest
{
    [Fact]
    public void InputComponentTest()
    {
        World world = new World();
        Entity deniska = world.CreateEntity();
        InputSystem inputSystem = new InputSystem(new FakeInput()); // вместо настоящего IInputProvider вставляем фейковый
        world.AddSystem(inputSystem);
        world.AddComponent<InputComponent>(deniska, new InputComponent(33));
        world.Update(1f);
        InputComponent res = new InputComponent(33);
        res.Direction = new Vector2(0, -1); // так как в фейковом классе нажата кнопка вперед, то направление в InputComponent будет смотреть вперед
        Assert.Equal(res, world.GetComponent<InputComponent>(deniska));

    }
}

