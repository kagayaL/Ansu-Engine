using Editor.Services;

namespace Editor.Model;

// контейнер, в котором лежат все сервисы редактора
// крч чтобы меньше параметров методам передавать
public record EditorServices(IFileDialogService Dialogs,
    ISceneFileService SceneFiles,
    IGameLauncher GameLauncher,
    IEditorConfigService Config,
    ILogger Logger)
{
    
}