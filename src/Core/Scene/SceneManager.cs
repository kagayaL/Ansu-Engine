using Core.Events;
using Core.Events.Events;

namespace Core.Scene;

public class SceneManager
{
    private readonly EventBus? _eventBus;

    public Scene? CurrentScene { get; private set; }

    public SceneManager(EventBus? eventBus = null)
    {
        _eventBus = eventBus;
    }

    public void ChangeScene(Scene newScene)
    {
        CurrentScene?.Unload();
        CurrentScene = newScene;
        CurrentScene.Load();

        _eventBus?.Publish(new SceneLoadedEvent(CurrentScene));
    }

    public void Update(float deltaTime)
    {
        CurrentScene?.Update(deltaTime);
    }
}
