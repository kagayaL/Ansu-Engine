using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ImGuiNET;
using MonoGame.ImGuiNet;

namespace PacEngine.Editor;

// Game - готовая заготовка от MonoGame, позволяет
// открывать окно, рисовать кадры 60/с 
public class EditorApp : Game
{
     private GraphicsDeviceManager _graphics; // создает экран, следит за размером окна+переключение в полноэкр. режим
     private ImGuiRenderer _imgui;
     public EditorApp()
    {
        _graphics = new GraphicsDeviceManager(this);  
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        IsMouseVisible = true;
        Window.Title = "Ansu-Engine";        
    }

    protected override void Initialize()
    {
        _imgui = new ImGuiRenderer(this);
        _imgui.RebuildFontAtlas();
        base.Initialize(); // вызов метода класса 
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
    // Метод, который MonoGame вызывает сам, чтобы нарисовать кадр (60/с)
    protected override void Draw(GameTime gameTime)
    {
        _imgui.BeforeLayout(gameTime);

        GraphicsDevice.Clear(new Color(255, 183, 255));

        ImGui.Begin("Editor");
        ImGui.End();
        _imgui.AfterLayout();
        base.Draw(gameTime);
    }
}
