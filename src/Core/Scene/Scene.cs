using Core.ECS;

namespace Core.Scene;

public abstract class Scene
{
    public string Name { get; set; } = string.Empty;
    public string? SourceLevelPath { get; set; }
    public World World { get; } = new();

    public virtual void Load() { }
    public virtual void Unload() { }

    public virtual void Update(float deltaTime)
    {
        World.Update(deltaTime);
    }
}
