using Editor.Model;
namespace Editor.Panels;
// любой, кто реализует этот интерфейс, обязан уметь рисовать
public interface IPanel
{
    void Draw(EditorContext context); // вызывается каждый кадр
}