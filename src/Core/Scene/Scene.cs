using Core.ECS;

namespace Core.Scene;

public abstract class Scene
{
    public World World { get; } = new();

    public abstract void Load();
    public abstract void Unload();

    public virtual void Update(float deltaTime)
    {
        World.Update(deltaTime);
    }
}
