using Core.ECS;
using Editor.Selection;
using Core.Math;
using Core.GameData;
using Core.Physics.Components;
using Runtime;
using Microsoft.Xna.Framework.Graphics;
using Render.Graphics;
using System.Reflection.Metadata;

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

        var sprite = new Sprite
        {
            TextureId = 0,          
            Layer = 0,
            SourceRect = new Core.Math.Rect(0, 0, 32, 32)   // ← кусок текстуры
        };
        scene.World.AddComponent(entity, sprite);
        
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
            if(compName.Name == name)
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

    public ActionResult LoadLevel(string path, GraphicsDevice device, Renderer renderer, ResourceManager resourceManager)
    {
        if (string.IsNullOrEmpty(path))
        {
            return ActionResult.Fail("The path is empty");
        }
        try
        {
            var world = _document.Scene.World;
            LevelLoader.LoadLevel(path, world, device, renderer, resourceManager);
            _document.MarkDirty();
            return ActionResult.Ok();
        }
        catch (Exception exc)
        {
            return ActionResult.Fail($"Loading error: {exc.Message}");
        }
    }
}