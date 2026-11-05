using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ImGuiNET;
using MonoGame.ImGuiNet;
using Editor.Model;
using Editor.Panels;
using Render.Graphics;
using Runtime;
using Render.Camera;

namespace Editor.App; 

// Game - готовая заготовка от MonoGame, позволяет
// открывать окно, рисовать кадры 60/с 
public class EditorApp : Game
{
     private GraphicsDeviceManager _graphics; // создает экран, следит за размером окна+переключение в полноэкр. режим
     private ImGuiRenderer? _imgui;

     private readonly EditorContext _context;
     private readonly EditorPanels _panels;

     private Renderer _renderer;
     private ResourceManager _resourceManager;
     private RenderSystem _renderSystem;
     private Camera2d _camera2d;

     public EditorApp(EditorContext context, EditorPanels panels)
    {
        _context = context;
        _panels = panels;
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
        _renderer = new Renderer(GraphicsDevice);
        _resourceManager = new ResourceManager(GraphicsDevice);
        _renderSystem = new RenderSystem(_renderer, _resourceManager);
        _camera2d = new Camera2d(1280, 720);
        // загрузка сцены из файла
        _context.LoadLevelAction = (path) =>
        {
            try
        {
            Runtime.LevelLoader.LoadLevel(path, _context.Document.Scene.World, GraphicsDevice, _renderer, _resourceManager);
            _context.Status.Success("Level loaded: " + System.IO.Path.GetFileName(path));
        }
        catch (Exception exc)
        {
            _context.Status.Error("Load failed: " + exc.Message);
        }
            };
        
        base.Initialize(); // вызов метода класса 

    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    // метод, который MonoGame вызывает сам, чтобы нарисовать кадр (60/с)
    protected override void Draw(GameTime gameTime)
    {
    _imgui.BeforeLayout(gameTime); // ImGui готовится: обновляет ввод, начинает новый frame
    GraphicsDevice.Clear(new Color(35, 37, 42));

    // отрисовка сцены
    Rectangle bounds = _context.ViewPort.ScreenBounds;
    GraphicsDevice.ScissorRectangle = bounds;
    _renderer.Begin(_camera2d.GetViewMatrix());
    _renderSystem.DrawRenderList(_context.Document.Scene.World);
    _renderer.End();
    GraphicsDevice.ScissorRectangle = GraphicsDevice.Viewport.Bounds;

    _panels.Draw(_context);        
    _imgui.AfterLayout(); //  ImGui завершает frame и выводит всё на экран
    base.Draw(gameTime);
    }

    // вызов LevelLoader
    public void LoadLevel(string path)
    {
        try
        {
            LevelLoader.LoadLevel(path, _context.Document.Scene.World, GraphicsDevice, _renderer, _resourceManager);
            _context.Status.Success("Level loaded");
        }
        catch (Exception exc)
        {
            _context.Status.Error($"Load failed: {exc.Message}");
        }
    }
}
