using ImGuiNET;
using Editor.Model;

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

        ImGui.EndMainMenuBar();
    }
}