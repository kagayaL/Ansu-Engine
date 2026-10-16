using Editor.Selection;
namespace Editor.Model;
//объединение всех частей редактора
public sealed class EditorContext
{
    public EditorDocument Document { get; } // открытая сцена + состояние файла
    public SelectionService Selection { get; } // что выбрано
    public EditorActions Actions { get; } // что можно делать
    public EditorServices Services { get; } // файлы, лог и диалоги
    public EditorStatus Status { get; } // короткое сообщение внизу
    public ViewportState ViewPort { get; } // размер вьюпорта и камера

    public EditorContext(
        EditorDocument document,
        SelectionService selection,
        EditorActions actions,
        EditorServices services,
        EditorStatus status,
        ViewportState viewport)
    {
        Document = document;
        Selection = selection;
        Actions = actions;
        Services = services;
        Status = status;
        ViewPort = viewport;
    }
}