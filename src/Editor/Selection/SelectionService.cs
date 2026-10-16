using Core.ECS;  

namespace Editor.Selection;

// хранит сущность, выбранную в редакторе

public sealed class SelectionService
{
    public Entity? Selected { get; private set; } // меняется тока через Select/Clear
    public void Select(Entity entity)
    {
        Selected = entity; // выбранная сущность заменяет пред. выбор
    }
    public void Clear()
    {
        Selected = null;
    }
    public bool IsSelected(Entity entity)
    {
        if (Selected == null)
            return false;
        return Selected.Value == entity; 
    }

}