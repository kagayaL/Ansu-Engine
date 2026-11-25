using Core.ECS;
using Editor.Selection;
using Core.Math;
using Core.GameData;
using Core.Physics.Components;
using Runtime;

// логика редактора - что можно делать со сценой
// панели вызывают эти методы, не трогая World
namespace Editor.Model;
public sealed class EditorActions
{
    private readonly EditorDocument _document;
    private readonly SelectionService _selection;
    private readonly EditorServices _services;
    private readonly EditorStatus _status;
    public EditorActions(
        EditorDocument document,
        SelectionService selection,
        EditorServices services,
        EditorStatus status)
    {
        _document = document;
        _selection = selection;
        _services = services;
        _status = status;
    }

    public ActionResult CreateEntity(string name)
    {
        name = name.Trim();

        if (name.Length == 0)
        {
            return ActionResult.Fail("Name can't be empty");
        }
        var scene = _document.Scene;
        var entity = scene.World.CreateEntity();

        scene.World.AddComponent(entity, new Transform());
        scene.World.AddComponent(entity, new NameComponent(name));

        _selection.Select(entity);
        _document.MarkDirty();
        return ActionResult.Ok();
    }

    public string GetEntityName(Entity entity)
    {
        var world = _document.Scene.World;
        if (world.HasComponent<NameComponent>(entity))
        {
            var compName = world.GetComponent<NameComponent>(entity);
            return compName.Name;
        }
        return "Entity " + entity.Id;
    }

    public bool HasEntityName(string name)
    {
        var names = _document.Scene.World.GetPool<NameComponent>();

        foreach (var compName in names.Values)
        {
            if (compName.Name == name)
            {
                return true;
            } 
        }
        return false;
    }

    public ActionResult DeleteEntity(Entity entity)
    {
        var world = _document.Scene.World;

        if (!world.IsAlive(entity))
        {
            return ActionResult.Fail("The entity was not found");
        }

        world.DestroyEntity(entity);
        
        if (_selection.IsSelected(entity))
        {
            _selection.Clear();
        }

        _document.MarkDirty();
        return ActionResult.Ok();
    }

    public ActionResult SetPosition(Entity entity, float x, float y)
    {
        var world = _document.Scene.World;

        if (!world.IsAlive(entity))
        {
            return ActionResult.Fail("The entity was not found");
        }
        if (!world.HasComponent<Transform>(entity))
        {
            return ActionResult.Fail("The entity has not Position");
        }

        ref var pos = ref world.GetComponent<Transform>(entity);
        pos.Position = new Vector2(x, y);

        _document.MarkDirty();
        return ActionResult.Ok();
    }

    public void SelectEntity(EditorContext context, int localX, int localY)
    {   
        var xnaVector = new Microsoft.Xna.Framework.Vector2(localX, localY);
        var worldPos = context.ViewPort.Camera.ScreenToWorld(xnaVector);
        _status.Success($"World pos: {worldPos.X:F1}, {worldPos.Y:F1}");

        var wrld = _document.Scene.World;
        var sprites = wrld.GetPool<Sprite>();

        foreach (var k in sprites)
        {
            var entity = new Entity(k.Key);

            if (!wrld.HasComponent<Transform>(entity))
            {
                continue;
            }
            var sprite = k.Value;
            var transform = wrld.GetComponent<Transform>(entity);

            float width = 32f; // по умолчанию
            float height = 32f;

            var rect = Culling.CalculateAABB(context.ResourceManager, sprite, transform);

            if (rect.HasValue)
            {
                width = rect.Value.Width;
                height = rect.Value.Height;
            }

            
            // Position - левый верхний угол
            float left = transform.Position.X;
            float right = transform.Position.X + width;
            float top = transform.Position.Y;
            float bottom = transform.Position.Y + height;

            if (worldPos.X >= left && worldPos.X <= right && worldPos.Y >= top && worldPos.Y <= bottom)
            {
                _selection.Select(entity);
                return;
            }
        }
        _selection.Clear();
    }
}