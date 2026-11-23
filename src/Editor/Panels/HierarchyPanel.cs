using ImGuiNET;
using Editor.Model;
using Core.Physics.Components;
using Core.ECS;
using Core.HierarchyComponents;
using Core.GameData;

namespace Editor.Panels;

public sealed class HierarchyPanel : IPanel
{
    public void Draw(EditorContext context)
    {
        var io = ImGui.GetIO(); // глобальный объект ImGui с информацией о состоянии
        float windowHeight = io.DisplaySize.Y;

         ImGui.SetNextWindowPos(
            new System.Numerics.Vector2(0, Constants.ToolbarHeight),
            ImGuiCond.Always);
        ImGui.SetNextWindowSize(
            new System.Numerics.Vector2(Constants.HierarchyWidth, windowHeight - Constants.StatusBarHeight - Constants.ToolbarHeight),
            ImGuiCond.Always);

        var flags = Constants.PanelsFlags;
        
        ImGui.Begin("Hierarchy", flags);

        var scene = context.Document.Scene;
        //тут будет не позиция, а RootEntity
        var positions = scene.World.GetPool<RootEntity>(); // берем всех на конкретной позиции

        if (positions.Count == 0)
        {
            ImGui.TextUnformatted("The scene is empty");
        }
        else
        {
            DrawRoot(context, positions.Keys.ToList());
        }
        var avail = ImGui.GetContentRegionAvail();
        if (avail.X > 0 && avail.Y > 0)
        {
            ImGui.InvisibleButton("empty", avail);
            RootTarget(context);
        }

        ImGui.End();
    }

    private void DrawRoot(EditorContext context, List <int> entities)
    {
        List <int> nonFolders = new List <int>();

        //Рисую папки
        for (int i = 0; i < entities.Count; i++)
        {
            var entityID = entities[i];
            var entity = new Entity(entityID);
            //RootEntity могут быть как папками, так и просто сущностями. По папкам надо проходиться внутрь
            if (context.Document.Scene.World.HasComponent<Folder>(entity))
            {
                DrawFolder(context, entity);
            }
            else
            {
                //сохраняю сущности не папки, чтобы потом быстро пройтись по ним
                nonFolders.Add(entityID);
            }
        }
        foreach (var entityId in nonFolders)
        {
            var entity = new Entity(entityId);
            DrawNonFolder(context, entity);
        }

    }
    
    //Тут рисуем папку со всеми ее детьми.
    private void DrawFolder(EditorContext context, Entity entity)
    {
        ImGuiTreeNodeFlags flags = ImGuiTreeNodeFlags.OpenOnArrow | ImGuiTreeNodeFlags.SpanAvailWidth;

        bool isSelected = context.Selection.IsSelected(entity);
        if (isSelected) flags |= ImGuiTreeNodeFlags.Selected;

        string name = context.Actions.GetEntityName(entity);
        string label = name + "##entity_" + entity.Id;
        var world = context.Document.Scene.World;

        //открываю папку
        bool isOpen = ImGui.TreeNodeEx(label, flags);

        if (ImGui.IsItemClicked(ImGuiMouseButton.Left))
        {
            context.Selection.Select(entity);
        }

        FolderTarget(context, entity);
        DragDropSource(context, entity, name);

        if (isOpen)
        {
            var folder = world.GetComponent<Folder>(entity);
            var children = new List<Entity>(folder.GetChildren());

            foreach (var child in children)
            {
                //если ребенок папка - идем вглубь
                if (world.HasComponent<Folder>(child))
                {
                    DrawFolder(context, child);
                }
                else
                {
                    DrawNonFolder(context, child);
                }
            }
            ImGui.TreePop();
        }
    }

    private void DrawNonFolder(EditorContext context, Entity entity)
    {

        bool isSelected = context.Selection.IsSelected(entity);

        string name = context.Actions.GetEntityName(entity);
        string label = name + "##entity_" + entity.Id;

        if (ImGui.Selectable(label, isSelected))
        {
            context.Selection.Select(entity);
        }
        DragDropSource(context, entity, name);
    }

    private void DragDropSource(EditorContext context, Entity entity, string name)
    {
        if (ImGui.BeginDragDropSource())
        {
            var id = entity.Id;
            unsafe
            {
                ImGui.SetDragDropPayload("ENTITY_ID", (IntPtr)(&id), sizeof(int));
            }
            ImGui.Text($"{name}");
            ImGui.EndDragDropSource();
        } 

    }

    private bool HasDroppedEntity(out Entity droppedEntity)
    {
        droppedEntity = default;
        var payload = ImGui.AcceptDragDropPayload("ENTITY_ID");
        unsafe
        {
            if (payload.NativePtr == null) return false;

            //id сброшенной сущности
            int droppedEntityId = *(int*)payload.Data;
            //сброшенная сущность
            droppedEntity = new Entity(droppedEntityId);
            return true;

        }
    }

    //что будет если таргетом при драг дропе будет папка (сущность в папку положить)
    private void FolderTarget(EditorContext context, Entity folder)
    {
        if (ImGui.BeginDragDropTarget())
        {
            if (HasDroppedEntity(out var droppedEntity)) 
            {
                context.Actions.MoveToFolder(folder, droppedEntity);
            }
        }
        ImGui.EndDragDropTarget();
        
    }
    //Если таргет - пустота в конце, то делаю источник корневым
    private void RootTarget(EditorContext context)
    {
        if (ImGui.BeginDragDropTarget())
        {
            if (HasDroppedEntity(out var droppedEntity))
            {
                context.Actions.MakeRoot(droppedEntity);
            }
        }
        ImGui.EndDragDropTarget();
    }

         
}