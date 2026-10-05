using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ImGuiNET;
using MonoGame.ImGuiNet;

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

        _imgui = new ImGuiRenderer(GraphicsDevice);
        
    }

    // Метод, который MonoGame вызывает сам, чтобы нарисовать кадр (60/с)
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(255, 183, 255));
        base.Draw(gameTime);
    }
}
