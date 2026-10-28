using ImGuiNET;
using Editor.Model;
using Core.ECS;

namespace Editor.Panels;

public sealed class ToolBarPanel : IPanel
{
    public void Draw(EditorContext context)
    {
        if (!ImGui.BeginMainMenuBar())
        {
            return;
        }
        // Меню File
        if (ImGui.BeginMenu("File"))
        {
            if (ImGui.MenuItem("New scene"))
            {
                context.Status.Success("New scene created");
            }
            if (ImGui.MenuItem("Open scene"))
            {
                context.Status.Success("Scene opened");
            } 
            if (ImGui.MenuItem("Save scene"))
            {
                context.Status.Success("Scene saved");
            }
            if (ImGui.MenuItem("Save scene as..."))
            {
                context.Status.Success("Save scene as..."); 
            }
            ImGui.EndMenu();
        }
        // Меню Edit
        if (ImGui.BeginMenu("Edit"))
        {
            if (ImGui.MenuItem("Undo"))
            {
                context.Status.Success("Undo");
            }
            if (ImGui.MenuItem("Redo"))
            {
                context.Status.Success("Redo");
            }
            ImGui.EndMenu();
        }
        // Меню View
        if (ImGui.BeginMenu("View"))
        {
            if (ImGui.MenuItem("Reset Layout"))
            {
                context.Status.Success("Reset Layout");
            }
            ImGui.EndMenu();
        }
        // Entity
        if (ImGui.BeginMenu("Entity"))
        {            
            if (ImGui.MenuItem("Create Entity"))
            {
                string name = "Entity 1";
                int cnt = 1;
                while (context.Actions.HasEntityName(name))
                {
                    cnt++;
                    name = "Entity " + cnt;
                }
                var result = context.Actions.CreateEntity(name);
                if (result.Succeeded)
                    context.Status.Success("Entity created");
                else
                    context.Status.Error(result.Error ?? "Failed");
            }

            var selected = context.Selection.Selected;
            bool canDelete = selected != null;
            if (!canDelete) ImGui.BeginDisabled();

            if (ImGui.MenuItem("Delete Entity"))
            {   
                var result = context.Actions.DeleteEntity(selected!.Value);
                if (result.Succeeded)
                    context.Status.Success("Entity deleted");
                else
                    context.Status.Error(result.Error ?? "Failed");
            }   
            if (!canDelete) ImGui.EndDisabled();

            ImGui.EndMenu();  
        }
        ImGui.EndMainMenuBar();
    }
}