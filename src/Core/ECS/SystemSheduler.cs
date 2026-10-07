using System.Collections.Generic;

namespace Core.ECS;

public class SystemScheduler
{
    private readonly List<ISystem> _systems = new();

    public void AddSystem(ISystem system)
    {
        _systems.Add(system);
    }

    public void Update(World world, float deltaTime)
    {
        for (int i = 0; i < _systems.Count; i++)
        {
            _systems[i].Update(world, deltaTime);
        }
    }
}