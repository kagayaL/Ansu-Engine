using ImGuiNET;

namespace Editor.Panels;

public class Constants
{
    public const float ToolbarHeight = 20f;

    public const float StatusBarHeight = 24f;

    public const float HierarchyWidth = 220f;

    public const float InspectorWidth = 300f;

    public const ImGuiWindowFlags PanelsFlags = 
        ImGuiWindowFlags.NoResize |      // нельзя тянуть за углы и менять размер
        ImGuiWindowFlags.NoMove |        // нельзя перетащить мышью за заголовок
        ImGuiWindowFlags.NoSavedSettings; // не сохранять положение и размер в imgui.ini
}