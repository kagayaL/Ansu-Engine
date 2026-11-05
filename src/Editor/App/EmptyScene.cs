using Core.Scene;

namespace Editor.App;

// временная пустая сцена, чтобы редактор мог запуститься
public sealed class EmptyScene : Scene
{
    public override void Load() { }
    public override void Unload() { }
}