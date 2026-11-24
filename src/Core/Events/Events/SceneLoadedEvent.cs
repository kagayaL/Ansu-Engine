using Core.Events;

namespace Core.Events.Events;

// Событие загрузки сцены
public readonly record struct SceneLoadedEvent(Scene.Scene Scene) : IEvent;
