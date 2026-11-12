using ImGuiNET;
using Editor.Model;
using Microsoft.VisualBasic;

namespace Editor.Panels;

// Св-ва выбранной сущности
public sealed class InspectorPanel : IPanel
{
    public void Draw(EditorContext context)
    {
        var io = ImGui.GetIO(); // глобальный объект ImGui с информацией о состоянии
        float windowWidth  = io.DisplaySize.X;
        float windowHeight = io.DisplaySize.Y;

         ImGui.SetNextWindowPos(
            new System.Numerics.Vector2(windowWidth - Constants.InspectorWidth, Constants.ToolbarHeight),
            ImGuiCond.Always);
        ImGui.SetNextWindowSize(
            new System.Numerics.Vector2(Constants.InspectorWidth, windowHeight - Constants.StatusBarHeight),
            ImGuiCond.Always);

        var flags = Constants.PanelsFlags;
        
            ImGui.Begin("Inspector", flags);

        // заглушка
        if (context.Selection.Selected == null)
        {
            ImGui.TextUnformatted("Nothing has been chosen");
        }
        else
        {
            // когда появятся сущности, тут будет имя и св-ва
            ImGui.TextUnformatted("Entity has been chosen");
        }

        ImGui.End();
    }
}