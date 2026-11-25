using Core.ECS;
using Core.GameData;
namespace Editor.Selection;

// хранит сущность, выбранную в редакторе

public sealed class SelectionService
{
    private World _world;
    public SelectionService(World world)
    {
        _world = world;
    }
    public Entity? Selected { get; private set; } // меняется тока через Select/Clear
    public void Select(Entity entity)
    {
        if (Selected != null) //значит надо снять флаг у старой сущности
        {
            if (_world.HasComponent<Sprite>(Selected.Value))
            {
                _world.GetComponent<Sprite>(Selected.Value).isSelected = false;
            }
        }
        Selected = entity; // выбранная сущность заменяет пред. выбор

        if (Selected != null) //помечает флаг у новой сущности
        {
            if (_world.HasComponent<Sprite>(Selected.Value))
            {
                _world.GetComponent<Sprite>(Selected.Value).isSelected = true;
            }
        }
    }
    public void Clear()
    {
        if (Selected != null) //значит надо снять флаг у старой сущности
        {
            if (_world.HasComponent<Sprite>(Selected.Value))
            {
                _world.GetComponent<Sprite>(Selected.Value).isSelected = false;
            }
        }
        Selected = null;
    }
    public bool IsSelected(Entity entity)
    {
        if (Selected == null)
            return false;
        return Selected.Value == entity; 
    }

}