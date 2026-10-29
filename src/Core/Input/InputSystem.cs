using Core.ECS;
using Core.Math;

namespace Core.Input;

public class InputSystem : ISystem
{
    private readonly IInputProvider _inputProvider;

    public InputSystem(IInputProvider inputProvider)
    {
        _inputProvider = inputProvider;
    }

    public void Update(World world, float dt)
    {
        var pool = world.GetPool<InputComponent>();

        foreach (int entityId in pool.Keys)
        {
            var entity = new Entity(entityId);
            ref var input = ref world.GetComponent<InputComponent>(entity);

            float x = 0f;
            float y = 0f;

            // Ключи из словаря в InputManager
            if (_inputProvider.IsActionDown("MoveLeft"))
                x -= 1f;
            if (_inputProvider.IsActionDown("MoveRight"))
                x += 1f;
            if (_inputProvider.IsActionDown("MoveStraight"))
                y -= 1f;
            if (_inputProvider.IsActionDown("MoveBack"))
                y += 1f;

            var dir = new Vector2(x, y);


            if (dir.X != 0f || dir.Y != 0f)
            {
                dir.Normalize();
            }

            input.Direction = dir;
        }
    }
}