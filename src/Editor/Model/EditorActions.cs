using Core.ECS;
using Editor.Selection;
using Core.Math;
using Core.GameData;
using Core.Physics.Components;
using System.Net.Mail;
using Core.HierarchyComponents;

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
        scene.World.AddComponent(entity, new RootEntity());

        _selection.Select(entity);
        _document.MarkDirty();
        return ActionResult.Ok();
    }

    public ActionResult MakeRoot(Entity entity)
    {
        var world = _document.Scene.World;

        if (world.HasComponent<RootEntity>(entity)) return ActionResult.Fail("Entity is already root"); 

        var parentComponent = world.GetComponent<Parent>(entity);
        var parent = parentComponent.GetParent();

        var folderComponent = world.GetComponent<Folder>(parent);
        folderComponent.RemoveChild(entity);

        world.RemoveComponent<Parent>(entity);
        world.AddComponent(entity, new RootEntity());
        return ActionResult.Ok();

    }
    //перемещаем в папку (удаляем у старого родителя, добавляем новому
    public ActionResult MoveToFolder(Entity folder, Entity entity)
    {
        if (IsRelatives(folder, entity)) return ActionResult.Fail("Objects are relatives");

        var world = _document.Scene.World;
        var folderComponent = world.GetComponent<Folder>(folder);

        //если наша сущность была корневой, то надо удалить компонент
        if (world.HasComponent<RootEntity>(entity))
        {
            world.RemoveComponent<RootEntity>(entity);
            world.AddComponent(entity, new Parent(folder));

        } //если сущность не корневая, то у нее обязан быть родитель
        else if (world.HasComponent<Parent>(entity))
        {
            ref var parentComponent = ref world.GetComponent<Parent>(entity);
            //переназначаем родителя
            var oldParent = parentComponent.GetParent();
            parentComponent.SetParent(folder);

            //старый родитель - папка, удаляем из нее
            if (world.HasComponent<Folder>(oldParent))
            {

                var oldFolderComponent = world.GetComponent<Folder>(oldParent);
                //удаляем у старого родителя ребенка
                oldFolderComponent.RemoveChild(entity);
            }

        }

        folderComponent.AddChild(entity);
        return ActionResult.Ok();

    }
    public ActionResult CreateFolder(string name)
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
        scene.World.AddComponent(entity, new RootEntity());
        scene.World.AddComponent(entity, new Folder());

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

    //проверка что у родителя ребенок не является предком
    private bool IsRelatives(Entity parent, Entity child)
    {
        var world = _document.Scene.World;
        //если родитель корневой, то точно ложь
        if (world.HasComponent<RootEntity>(parent)) return false;

        var ansector = world.GetComponent<Parent>(parent).GetParent();

        //идем по предкам родитяля, пока не дойдем до корня
        while (!world.HasComponent<RootEntity>(ansector))
        {
            if (ansector == child) return true;
            //переход к некст предку
            ansector = world.GetComponent<Parent>(ansector).GetParent();
        }
        //проверка корня
        if (ansector == child) return true;
        return false;
    }
}