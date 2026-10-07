using System.Collections.Generic;

namespace Core.ECS;

public class World
{
    private int _nextEntityId = 1;
    private readonly HashSet<int> _entities = new();

    private readonly ComponentStore _store = new();
    private readonly SystemScheduler _scheduler = new();

    public Entity CreateEntity()
    {
        int id = _nextEntityId++;
        _entities.Add(id);
        return new Entity(id);
    }

    public void DestroyEntity(Entity entity)
    {
        if (_entities.Remove(entity.Id))
        {
            _store.RemoveAll(entity.Id);
        }
    }

    public bool IsAlive(Entity entity) => _entities.Contains(entity.Id);


    public void AddComponent<T>(Entity entity, T component) where T : struct
    {
        _store.Add(entity.Id, component);
    }

    public ref T GetComponent<T>(Entity entity) where T : struct
    {
        return ref _store.Get<T>(entity.Id);
    }

    public bool HasComponent<T>(Entity entity) where T : struct
    {
        return _store.Has<T>(entity.Id);
    }

    public void RemoveComponent<T>(Entity entity) where T : struct
    {
        _store.Remove<T>(entity.Id);
    }

    public Dictionary<int, T> GetPool<T>() where T : struct
    {
        return _store.GetPool<T>();
    }


    public void AddSystem(ISystem system)
    {
        _scheduler.AddSystem(system);
    }

    public void Update(float deltaTime)
    {
        _scheduler.Update(this, deltaTime);
    }
}