using System.Collections.Generic; // коллекции из стандартной либы .NET
using Editor.Model;

namespace Editor.Panels;
// менеджер панелей
public sealed class EditorPanels
{
    private readonly List<IPanel> _panels;
    public EditorPanels(IEnumerable<IPanel> panels)
    {
        _panels = new List<IPanel>(panels);
    }

    // вызываем Draw у всех панелей по порядку
    public void Draw(EditorContext context)
    {
        for (int i = 0; i< _panels.Count; i++)
        {
            _panels[i].Draw(context);
        }
    }
}