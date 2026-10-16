using Editor.App;
using Editor.Model;
using Editor.Panels;
using Editor.Selection;

var scene = new EmptyScene();
var document = new EditorDocument(scene);

var services = new EditorServices(
    Dialogs: new NullFileDialogService(),
    SceneFiles: new NullSceneFileService(),
    GameLauncher: new NullGameLauncher(),
    Config: new NullEditorConfigService(),
    Logger: new NullLogger());

var status = new EditorStatus();
status.Success("StatusBar");
var selection = new SelectionService();
var viewport = new ViewportState();
var actions = new EditorActions(document, selection, services, status);

var context = new EditorContext(
    document,
    selection,
    actions,
    services,
    status,
    viewport);

var panels = new EditorPanels(new IPanel[]
{
    new ToolBarPanel(),
    new HierarchyPanel(),
    new InspectorPanel(),
    new StatusBarPanel(),
    new ViewportPanel(),
});

using var game = new EditorApp(context, panels);
game.Run();