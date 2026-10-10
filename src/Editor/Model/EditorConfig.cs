namespace Editor.Model;

public sealed class EditorConfig
{
    // путь к последней открытой сцене, null — сцена ещё не открывалась.
    public string? LastScenePath { get; set; }
}