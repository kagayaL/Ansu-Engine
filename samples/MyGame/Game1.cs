using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Render.Input;
using Render.Graphics;

namespace Sandbox;

public sealed class Game1 : Game
{
    private readonly GraphicsDeviceManager _gdm;
    private readonly WindowSettings _settings;
    private GraphicsService _graphics;
    private InputManager _input;
    private Vector2 _testPosition = new Vector2(600, 300);


    public Game1()
    {
        _settings = new WindowSettings
        {
            Width = 1280,
            Height = 720,
            Title = "Sandbox",
            ClearColor = Color.CornflowerBlue,
        };

        _gdm = new GraphicsDeviceManager(this)
        {
            //размер окна в пикселях
            PreferredBackBufferWidth = _settings.Width,
            PreferredBackBufferHeight = _settings.Height,
            SynchronizeWithVerticalRetrace = _settings.VSync,
            PreferredDepthStencilFormat = DepthFormat.None,
        };

        //текстуры, шрифты, звуки лежат в контент
        Content.RootDirectory = "Content";
        IsFixedTimeStep = _settings.FixedTimeStep;
        Window.AllowUserResizing = _settings.AllowResize;
        Window.Title = _settings.Title;
    }

    protected override void LoadContent()
    {
        _graphics = new GraphicsService(GraphicsDevice, _settings);
        _input = new InputManager();
    }

    protected override void Update(GameTime gameTime)
    {
        _input.Update();
        
        if (_input.IsActionPressed("Pause")) 
        {
            Exit();
        }

        //формула для движения, независимого от частоты кадров
        float speed = 400f * (float)gameTime.ElapsedGameTime.TotalSeconds; 

        if (_input.IsActionDown("MoveLeft"))
        {
            _testPosition.X -= speed;
        }
        if (_input.IsActionDown("MoveRight"))
        {
            _testPosition.X += speed;
        }
        if (_input.IsActionDown("MoveStraight")) 
        {
            _testPosition.Y -= speed;
        }
        if (_input.IsActionDown("MoveBack")) 
        {
            _testPosition.Y += speed;
        }

        base.Update(gameTime);
    }
    

    protected override void Draw(GameTime gameTime)
    {
        _graphics.Renderer.Clear(_settings.ClearColor);
        _graphics.Renderer.Begin();
        //задаем размеры прямоугольника
        Rectangle testRect = new Rectangle((int)_testPosition.X, (int)_testPosition.Y, 64, 64);
        _graphics.Renderer.DrawRect(testRect, Color.Magenta);
       
        _graphics.Renderer.End();

        base.Draw(gameTime);
    }
}