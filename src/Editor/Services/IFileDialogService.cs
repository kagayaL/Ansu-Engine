namespace Editor.Services;

// умеет спрашивать у пользователя путь к файлу, 
// открывать существующий и создавать новый
public interface IFileDialogService
{
    string? OpenScene();
    string? SaveScene();
}