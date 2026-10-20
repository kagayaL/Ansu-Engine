using Core.Scene;
namespace Editor.Model;
// хранит открытую сцену и состояние файла
public sealed class EditorDocument
{
    public Scene Scene { get; set; } 
    public string? FilePath { get; set; }
    public bool IsDirty { get; set; }

    public EditorDocument(Scene scene)
    {
        Scene = scene ?? throw new ArgumentNullException(nameof(scene));
    } 

    // при загрузке файла заменить открытую сцену целиком
    public void ReplaceScene(Scene NewScene, string? NewFilePath, bool NewIsDirty)
    {
        Scene = NewScene ?? throw new ArgumentNullException(nameof(NewScene));
        NewFilePath = FilePath;
        NewIsDirty = IsDirty;
    }

    // пометить как измененный
    public void MarkDirty()
    {
        IsDirty = true;
    }

    // пометить как сохраненный
    public void MarkSaved(string path)
    {
        FilePath = path;
        IsDirty = true;
    }
}