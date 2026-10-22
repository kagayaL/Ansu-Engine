using System.Collections.Generic;

using Core.GameData;
using Core.Physics.Components;
using Core.GameData.TileMap;

namespace Core.ECS;

public class World
{
    private int _nextEntityId = 1;
    private readonly HashSet<int> _entities = new();

    // ID сущностей, у которых есть Position и Sprite
    private readonly HashSet<int> _renderableEntities = new();
    private readonly List<Entity> _renderList = new();

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
            _renderableEntities.Remove(entity.Id);
            _store.RemoveAll(entity.Id);
        }
    }

    public bool IsAlive(Entity entity) => _entities.Contains(entity.Id);

    public void AddComponent<T>(Entity entity, T component) where T : struct
    {
        _store.Add(entity.Id, component);

        // Добавляем в список отрисовки, если собралась пара Position + Sprite
        if ((typeof(T) == typeof(Sprite) || typeof(T) == typeof(TileMapComponent)) &&
            _store.Has<Position>(entity.Id))
        {
            _renderList.Add(entity);
        }
        else if (typeof(T) == typeof(Position) &&
            (_store.Has<Sprite>(entity.Id) || _store.Has<TileMapComponent>(entity.Id)))
        {
            _renderList.Add(entity);
        }
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
        // Убираем из отрисовки при удалении любого из трех компонентов
        if (typeof(T) == typeof(Sprite) || typeof(T) == typeof(Position) ||
           typeof(T) == typeof(TileMapComponent))
        {
            _renderList.Remove(entity);
        }

        _store.Remove<T>(entity.Id);
    }

    public Dictionary<int, T> GetPool<T>() where T : struct
    {
        return _store.GetPool<T>();
    }

    // Возвращает актуальный список (Position, Sprite) для рендера
    public List<Entity> GetRenderList()
    {
        return _renderList;
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