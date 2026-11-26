using ImGuiNET;
using Editor.Model;
using Microsoft.Xna.Framework;  

namespace Editor.Panels;
// центральная панель
public sealed class ViewportPanel : IPanel
{
    public void Draw(EditorContext context)
    {
        var io = ImGui.GetIO(); // глобальный объект ImGui с информацией о состоянии
        float windowHeight = io.DisplaySize.Y;
        float windowWidth  = io.DisplaySize.X;

        var pos = new System.Numerics.Vector2(
        Constants.HierarchyWidth, 
        Constants.ToolbarHeight);
    
        var size = new System.Numerics.Vector2(
            windowWidth - Constants.HierarchyWidth - Constants.InspectorWidth,
            windowHeight - Constants.ToolbarHeight - Constants.StatusBarHeight);

        ImGui.SetNextWindowPos(pos, ImGuiCond.Always);
        ImGui.SetNextWindowSize(size, ImGuiCond.Always);
        
        var flags = Constants.PanelsFlags | ImGuiWindowFlags.NoBackground;

            ImGui.Begin("Viewport", flags);
            context.ViewPort.ScreenBounds = new Rectangle(
                (int)pos.X, (int)pos.Y,
                (int)size.X, (int)size.Y
            );
            ImGui.End();
    }
}