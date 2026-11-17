using Core.Events;

namespace Core.Events.Events;

// Событие загрузки сцены по имени
public readonly record struct SceneLoadedEvent(Scene.Scene Scene) : IEvent;
