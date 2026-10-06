using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    // Обертка для идентификатора сущности
    public record struct Entity(int Id);

    // Интерфейс для пула компонентов, чтобы можно было хранить их в одном dictionary
    internal interface IComponentPool
    {
        bool Has(int entityId);
        void Remove(int entityId);
    }

    // Пул компонентов для конкретного типа компонента
    internal sealed class ComponentPool<T> : IComponentPool where T : struct
    {
        private readonly Dictionary<int, T> _components = new();
        public void Set(int entityId, T component) => _components[entityId] = component;

        public ref T Get(int entityId) => ref System.Runtime.InteropServices.CollectionsMarshal.GetValueRefOrNullRef(_components, entityId);

        public bool Has(int entityId) => _components.ContainsKey(entityId);

        public void Remove(int entityId) => _components.Remove(entityId);

        public Dictionary<int, T> GetAll() => _components;


    }

    public interface ISystem
    {
        void Update(World world, float deltaTime);
    }


    public class World
    {
        private int _nextEntityId = 1;
        private readonly HashSet<int> _entities = new();

        // Dictionary для хранения пулов компонентов по типу компонента
        private readonly Dictionary<Type, IComponentPool> _pools = new();

        // Список систем, которые будут обновляться каждый кадр
        private readonly List<ISystem> _systems = new();

        public Entity CreateEntity()
        {
            int id = _nextEntityId++;
            _entities.Add(id);
            return new Entity(id);
        }

        public void DestroyEntity(Entity entity)
        {
            _entities.Remove(entity.Id);
            // Зачищаем все компоненты удаленной сущности
            foreach (var pool in _pools.Values)
            {
                pool.Remove(entity.Id);
            }
        }

        // Возвращает все сущности с конкретным компонентом (нужно системам для итерации)
        public Dictionary<int, T> GetPool<T>() where T : struct
        {
            return GetOrCreatePool<T>().GetAll();
        }

        // Добавление системы в логику
        public void AddSystem(ISystem system)
        {
            _systems.Add(system);
        }

        // Обновление всех систем с учетом deltaTime
        public void Update(float deltaTime)
        {
            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].Update(this, deltaTime);
            }
        }

        // --- Управление компонентами ---

        // Защита от ещё не созданного компонента
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

        public void AddComponent<T>(Entity entity, T component) where T : struct
        {
            GetOrCreatePool<T>().Set(entity.Id, component);
        }

        public ref T GetComponent<T>(Entity entity) where T : struct
        {
            return ref GetOrCreatePool<T>().Get(entity.Id);
        }

        public bool HasComponent<T>(Entity entity) where T : struct
        {
            return GetOrCreatePool<T>().Has(entity.Id);
        }
    }
}
