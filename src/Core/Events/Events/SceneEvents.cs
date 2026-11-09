namespace Core.Events.Events;

public readonly record struct SceneLoadedEvent(Scene LoadedScene) : IEvent;
