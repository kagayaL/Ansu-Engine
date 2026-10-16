using ImGuiNET;
using Editor.Model;

namespace Editor.Panels;

// Св-ва выбранной сущности
public sealed class InspectorPanel : IPanel
{
    private const float StatusBarHeight = 24f;
    private const float ToolbarHeight = 20f;
    private const float InspectorWidth = 300f;

    public void Draw(EditorContext context)
    {
        var io = ImGui.GetIO(); // глобальный объект ImGui с информацией о состоянии
        float windowWidth  = io.DisplaySize.X;
        float windowHeight = io.DisplaySize.Y;

         ImGui.SetNextWindowPos(
            new System.Numerics.Vector2(windowWidth - InspectorWidth, ToolbarHeight),
            ImGuiCond.Always);
        ImGui.SetNextWindowSize(
            new System.Numerics.Vector2(InspectorWidth, windowHeight - StatusBarHeight),
            ImGuiCond.Always);

        ImGuiWindowFlags flags =
            ImGuiWindowFlags.NoResize |      // нельзя тянуть за углы и менять размер
            ImGuiWindowFlags.NoMove |        // нельзя перетащить мышью за заголовок
            ImGuiWindowFlags.NoSavedSettings; // не сохранять положение и размер в imgui.ini

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