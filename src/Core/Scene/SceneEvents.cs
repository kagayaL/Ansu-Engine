namespace Core.Scene;

public readonly record struct SceneLoadedEvent(Scene.Scene LoadedScene) : IEvent;
