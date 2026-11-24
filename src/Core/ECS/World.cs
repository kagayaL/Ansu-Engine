using System.Collections.Generic;

using Core.GameData;
using Core.Physics.Components;
using Core.GameData.TileMap;
using Core.Events;

namespace Core.ECS;

public class World
{
    private int _nextEntityId = 1;
    private readonly HashSet<int> _entities = new();

    // ID сущностей, у которых есть Position и Sprite
    private readonly List<Entity> _renderList = new();

    private readonly ComponentStore _store = new();
    private readonly SystemScheduler _scheduler = new();
    private readonly EventBus _eventBus;

    // Перечисление ID всех сущностей для первичной загрузки в UI
    public IEnumerable<int> EntityIds => _entities;

    public World(EventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public World() : this(new Events.EventBus()) { }

    public Entity CreateEntity()
    {
        int id = _nextEntityId++;
        _entities.Add(id);
        var entity = new Entity(id);
        _eventBus.Publish(new Events.Events.EntityCreatedEvent(entity)); // Уведомление о создании сущности
        return entity;
    }

    public void DestroyEntity(Entity entity)
    {
        if (_entities.Remove(entity.Id))
        {
            _renderList.Remove(entity);
            _store.RemoveAll(entity.Id);
            _eventBus.Publish(new Events.Events.EntityDestroyedEvent(entity)); // Уведомление об удалении сущности
        }
    }

    public bool IsAlive(Entity entity) => _entities.Contains(entity.Id);

    public void AddComponent<T>(Entity entity, T component) where T : struct
    {
        _store.Add(entity.Id, component);

        // Добавляем в список отрисовки, если собралась пара Position + Sprite
        if ((typeof(T) == typeof(Sprite) || typeof(T) == typeof(TileMapComponent)) &&
            _store.Has<Transform>(entity.Id))
        {
            _renderList.Add(entity);
        }
        else if (typeof(T) == typeof(Transform) &&
            (_store.Has<Sprite>(entity.Id) || _store.Has<TileMapComponent>(entity.Id)))
        {
            _renderList.Add(entity);
        }

        _eventBus.Publish(new Events.Events.ComponentAddedEvent(entity, typeof(T))); // Уведомление о добавлении компонента
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
        if (typeof(T) == typeof(Sprite) || typeof(T) == typeof(Transform) ||
           typeof(T) == typeof(TileMapComponent))
        {
            _renderList.Remove(entity);
        }

        _store.Remove<T>(entity.Id);
        _eventBus.Publish(new Events.Events.ComponentRemovedEvent(entity, typeof(T))); // Уведомление об удалении компонента
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

    public IEnumerable<Entity> Query<T1, T2>()
    where T1 : struct
    where T2 : struct
    {
        // Берем пул T1 (Dictionary<int, T1>)
        var pool1 = _store.GetPool<T1>();

        foreach (int entityId in pool1.Keys)
        {
            if (_store.Has<T2>(entityId))
            {
                yield return new Entity(entityId);
            }
        }
    }

}
