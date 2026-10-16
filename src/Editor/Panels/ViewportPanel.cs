using ImGuiNET;
using Editor.Model;

namespace Editor.Panels;
// центральная панель
public sealed class ViewportPanel : IPanel
{
    private const float HierarchyWidth = 220f;
    private const float InspectorWidth = 300f;
    private const float ToolbarHeight = 20f;
    private const float StatusBarHeight = 24f;

    public void Draw(EditorContext context)
    {
        var io = ImGui.GetIO(); // глобальный объект ImGui с информацией о состоянии
        float windowHeight = io.DisplaySize.Y;
        float windowWidth  = io.DisplaySize.X;

         ImGui.SetNextWindowPos(
            new System.Numerics.Vector2(HierarchyWidth, ToolbarHeight),
            ImGuiCond.Always);
        ImGui.SetNextWindowSize(
            new System.Numerics.Vector2(windowWidth - HierarchyWidth - InspectorWidth,
                windowHeight - ToolbarHeight - StatusBarHeight),
            ImGuiCond.Always);

        ImGuiWindowFlags flags =
            ImGuiWindowFlags.NoResize |      // нельзя тянуть за углы и менять размер
            ImGuiWindowFlags.NoMove |        // нельзя перетащить мышью за заголовок
            ImGuiWindowFlags.NoSavedSettings; // не сохранять положение и размер в imgui.ini

            ImGui.Begin("Viewport", flags);
            
            // заглушка
            ImGui.TextUnformatted("Viewport");

            ImGui.End();
    }
}