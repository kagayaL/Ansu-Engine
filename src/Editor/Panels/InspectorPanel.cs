using ImGuiNET;
using Editor.Model;
using Microsoft.VisualBasic;
using Core.Physics.Components;
using System.Xml.Serialization;

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

        var selected = context.Selection.Selected;
        if (selected == null)
        {
            ImGui.TextUnformatted("Nothing has been chosen");
        }
        else
        {
            string name = context.Actions.GetEntityName(selected.Value);
            ImGui.Text($"Name: {name}");
            ImGui.Separator();

            var world = context.Document.Scene.World;
            if (world.HasComponent<Transform>(selected.Value)) {
                ref var transform = ref world.GetComponent<Transform>(selected.Value);

                float x = transform.Position.X;
                float y = transform.Position.Y;

                if (ImGui.DragFloat("X", ref x, 0.5f))
                {
                    context.Actions.SetPosition(selected.Value, x, y);
                }
                if (ImGui.DragFloat("Y", ref y, 0.5f))
                {
                    context.Actions.SetPosition(selected.Value, x, y);
                }

                ImGui.Separator();
                ImGui.Text($"Rotation: {transform.Rotation}");
                ImGui.Text($"Scale: {transform.Scale}");

            }
            else
            {
                ImGui.TextUnformatted("No transform component");
            }
        }
        
        ImGui.End();
    }
}