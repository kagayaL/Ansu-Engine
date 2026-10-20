using ImGuiNET;
using Editor.Model;
using Core.Physics.Components;
using Core.ECS;
using System.Linq;
using System.Reflection.Metadata;
using Microsoft.VisualBasic;

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
            new System.Numerics.Vector2(Constants.HierarchyWidth, Constants.ToolbarHeight - (windowHeight - Constants.StatusBarHeight)),
            ImGuiCond.Always);

        ImGuiWindowFlags flags =
            ImGuiWindowFlags.NoResize |      // нельзя тянуть за углы и менять размер
            ImGuiWindowFlags.NoMove |        // нельзя перетащить мышью за заголовок
            ImGuiWindowFlags.NoSavedSettings; // не сохранять положение и размер в imgui.ini

            ImGui.Begin("Hierarchy", flags);

            var scene = context.Document.Scene;
            var positions = scene.World.GetPool<Position>(); // берем всех на конкретной позиции

            if (positions.Count == 0)
        {
            ImGui.TextUnformatted("The scene is empty");
        }
        else
        {
            var entitiesList = positions.Keys.ToList();

            for (int i = 0; i < entitiesList.Count; i++)
            {
                int entityID = entitiesList[i];
                var entity = new Entity(entityID);

                bool isSelected = context.Selection.IsSelected(entity);

                string label = "Entity " + entityID + "##" + i;

                if (ImGui.Selectable(label, isSelected))
                {
                    context.Selection.Select(entity);
                }
            }
        }
        ImGui.End();
    }
}