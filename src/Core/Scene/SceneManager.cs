using Core.Scene;

// 
public class SceneManager
{
    public Scene? CurrentScene { get; private set; }

    // Переключение сцены
    public void ChangeScene(Scene newScene)
    {
        CurrentScene?.Unload();
        CurrentScene = newScene;
        CurrentScene.Load();
    }

    public void Update(float deltaTime)
    {
        CurrentScene?.Update(deltaTime);
    }
}