using ImGuiNET;
using Editor.Model;

namespace Editor.Panels;

public sealed class HierarchyPanel : IPanel
{
    private const float StatusBarHeight = 24f;
    private const float ToolbarHeight = 20f;
    private const float HierarchyWidth = 220f;

    public void Draw(EditorContext context)
    {
        var io = ImGui.GetIO(); // глобальный объект ImGui с информацией о состоянии
        float windowHeight = io.DisplaySize.Y;

         ImGui.SetNextWindowPos(
            new System.Numerics.Vector2(0, ToolbarHeight),
            ImGuiCond.Always);
        ImGui.SetNextWindowSize(
            new System.Numerics.Vector2(HierarchyWidth, windowHeight - StatusBarHeight - ToolbarHeight),
            ImGuiCond.Always);

        ImGuiWindowFlags flags =
            ImGuiWindowFlags.NoResize |      // нельзя тянуть за углы и менять размер
            ImGuiWindowFlags.NoMove |        // нельзя перетащить мышью за заголовок
            ImGuiWindowFlags.NoSavedSettings; // не сохранять положение и размер в imgui.ini

            ImGui.Begin("Hierarchy", flags);

            ImGui.TextUnformatted("The scene is not loaded"); // заглушка, сцены пока нет
            ImGui.End();
    }
}