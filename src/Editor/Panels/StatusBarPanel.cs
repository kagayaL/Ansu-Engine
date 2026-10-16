using ImGuiNET;
using Editor.Model;

namespace Editor.Panels;
// полоса внизу окна редактора, прилипает снизу
public sealed class StatusBarPanel : IPanel
{
    private const float StatusBarHeight = 24f;
    // вызывается каждый кадр EditorPanels
    public void Draw(EditorContext context)
    {
        var io = ImGui.GetIO(); // глобальный объект ImGui с информацией о состоянии
        float windowHeight = io.DisplaySize.Y;
        float windowWidth = io.DisplaySize.X;
        // прибить к низу окна, растянуть на всю ширину
        ImGui.SetNextWindowPos(new System.Numerics.Vector2(0, windowHeight - StatusBarHeight),
            ImGuiCond.Always);
        ImGui.SetNextWindowSize(new System.Numerics.Vector2(windowWidth, StatusBarHeight), ImGuiCond.Always);
            
        ImGuiWindowFlags flags =
            ImGuiWindowFlags.NoTitleBar | // убирает серую полосу заголовка
            ImGuiWindowFlags.NoResize |      // нельзя тянуть за углы и менять размер
            ImGuiWindowFlags.NoMove |        // нельзя перетащить мышью за заголовок
            ImGuiWindowFlags.NoSavedSettings; // не сохранять положение и размер в imgui.ini

            ImGui.Begin("##StatusBar", flags); // открывает окно с названием статусбар, но в заголовке не показывает
            
            // цвет красный при ошибке, зелёный иначе
            var color = context.Status.IsError
                ? new System.Numerics.Vector4(1f, 0.4f, 0.4f, 1f)   
                : new System.Numerics.Vector4(0.7f, 1f, 0.7f, 1f);  

            ImGui.PushStyleColor(ImGuiCol.Text, color);
            ImGui.TextUnformatted(context.Status.Message); // выводит строку без разбора %
            ImGui.PopStyleColor(); // пара к push, возвращает текст какого цвета был

            ImGui.End();
        }
}
