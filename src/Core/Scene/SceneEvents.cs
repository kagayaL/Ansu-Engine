namespace Core.Scene;

public readonly record struct SceneLoadedEvent(Scene LoadedScene) : IEvent;
