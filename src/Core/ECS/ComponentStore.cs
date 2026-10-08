using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Core.ECS;

internal interface IComponentPool
{
    bool Has(int entityId);
    void Remove(int entityId);
}

internal sealed class ComponentPool<T> : IComponentPool where T : struct
{
    private readonly Dictionary<int, T> _components = new();

    public void Set(int entityId, T component) => _components[entityId] = component;

    public ref T Get(int entityId) =>
        ref CollectionsMarshal.GetValueRefOrNullRef(_components, entityId);

    public bool Has(int entityId) => _components.ContainsKey(entityId);

    public void Remove(int entityId) => _components.Remove(entityId);

    public Dictionary<int, T> GetAll() => _components;
}

public class ComponentStore
{
    private readonly Dictionary<Type, IComponentPool> _pools = new();

    private ComponentPool<T> GetOrCreatePool<T>() where T : struct
    {
        var type = typeof(T);
        if (!_pools.TryGetValue(type, out var pool))
        {
            pool = new ComponentPool<T>();
            _pools[type] = pool;
        }
        return (ComponentPool<T>)pool;
    }

    public void Add<T>(int entityId, T component) where T : struct
    {
        GetOrCreatePool<T>().Set(entityId, component);
    }

    public ref T Get<T>(int entityId) where T : struct
    {
        return ref GetOrCreatePool<T>().Get(entityId);
    }

    public bool Has<T>(int entityId) where T : struct
    {
        if (!_pools.TryGetValue(typeof(T), out var pool))
            return false;

        return pool.Has(entityId);
    }

    public void Remove<T>(int entityId) where T : struct
    {
        if (_pools.TryGetValue(typeof(T), out var pool))
        {
            pool.Remove(entityId);
        }
    }

    public void RemoveAll(int entityId)
    {
        foreach (var pool in _pools.Values)
        {
            pool.Remove(entityId);
        }
    }

    public Dictionary<int, T> GetPool<T>() where T : struct
    {
        return GetOrCreatePool<T>().GetAll();
    }
}