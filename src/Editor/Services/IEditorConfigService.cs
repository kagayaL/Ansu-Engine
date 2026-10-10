using Editor.Model;

namespace Editor.Services;

public interface IEditorConfigService
{
    EditorConfig Load();
    void Save(EditorConfig config);
}