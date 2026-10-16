using Core.ECS;
using Editor.Selection;
// логика редактора - что можно делать со сценой
// панели вызывают эти методы, не трогая World
namespace Editor.Model;
public sealed class EditorActions
{
    private readonly EditorDocument _document;
    private readonly SelectionService _selection;
    private readonly EditorServices _services;
    private readonly EditorStatus _status;
    public EditorActions(
        EditorDocument document,
        SelectionService selection,
        EditorServices services,
        EditorStatus status)
    {
        _document = document;
        _selection = selection;
        _services = services;
        _status = status;
    }
}