using Editor.Model;
using Editor.Services;

namespace Editor.App;

// временные пустые реализации сервисов,
// чтобы EditorServices создался без настоящих

internal sealed class NullLogger : ILogger
{
    public void Info(string message) { }
    public void Error(string message) { }
}

internal sealed class NullFileDialogService : IFileDialogService
{
    public string? OpenScene()
    {
        return null;
    } 
    public string? SaveScene(){
        return null;
    }
}

internal sealed class NullSceneFileService : ISceneFileService { }

internal sealed class NullGameLauncher : IGameLauncher { }

internal sealed class NullEditorConfigService : IEditorConfigService
{
    public EditorConfig Load() {
        return new EditorConfig();
    }
    public void Save(EditorConfig config) { }
}