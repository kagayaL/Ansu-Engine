using Core.ECS;

namespace Core.HierarchyComponents
{
    public struct Folder : IComponent
    {
        private List<Entity> _children;

        public Folder()
        {
            _children = new List<Entity>();
        }

        public void AddChild(Entity entity)
        {
            _children.Add(entity);
        }

        public void RemoveChild(Entity entity)
        {
            _children.Remove(entity);
        }

        public List<Entity> GetChildren()
        {
            return _children;
        }
    }

    public struct Parent : IComponent
    {
        private Entity _parent;

        public Parent(Entity parent)
        {
            _parent = parent;
        }

        public Entity GetParent()
        {
            return _parent;
        }

        public void SetParent(Entity parent)
        {
            _parent = parent;
        }
    }
}
