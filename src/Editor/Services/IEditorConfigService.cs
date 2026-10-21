using Editor.Model;

namespace Editor.Services;
// сохранить/изменить настроки редактора, вызывается из Program.cs
public interface IEditorConfigService
{
    EditorConfig Load();
    void Save(EditorConfig config);
}