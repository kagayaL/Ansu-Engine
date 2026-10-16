using Core.Scene;
namespace Editor.Model;
// хранит открытую сцену и состояние файла
public sealed class EditorDocument
{
    public EditorDocument(Scene scene)
    {
        scene = scene ?? throw new ArgumentNullException(nameof(scene));
    }
}