/*using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Render.Input;
using Render.Graphics;
using Render.Camera;

namespace MyGame;

public sealed class Game1 : Game
{
    private readonly GraphicsDeviceManager _gdm;
    private readonly WindowSettings _settings;
    private GraphicsService _graphics;
    private InputManager _input;
    private Camera2d _camera;
    private CameraFollow _cameraFollow;
    private Render.Camera.Viewport _viewport;
    private Vector2 _testPosition = new Vector2(600, 300);


    public Game1()
    {
        _settings = new WindowSettings
        {
            Width = 1280,
            Height = 720,
            Title = "MyGame",
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

        // инициализация камеры и вьюпорта
        _camera = new Camera2d(_settings.Width, _settings.Height);
        _cameraFollow = new CameraFollow(_camera, _settings.Height, _settings.Width);
        _viewport = new Render.Camera.Viewport(_camera, _settings.Width, _settings.Height);

        // пусть границы мира 2000x1000, чтобы проверить RoomLocked
        _cameraFollow.WorldBounds = new Rectangle(0, 0, 2000, 1000);
        // пусть фиксированный X для VerticalFollow
        _cameraFollow.FixedX = 1000f;
    }

    protected override void Update(GameTime gameTime)
    {
        _input.Update();

        if (_input.IsActionPressed("Pause"))
        {
            Exit();
        }

        // переключение режимов камеры по клавише E (Interact)
        if (_input.IsActionPressed("Interact"))
        {
            if (_cameraFollow.Mode == FollowMode.Follow)
            {
                _cameraFollow.Mode = FollowMode.RoomLocked;
            }
            else if (_cameraFollow.Mode == FollowMode.RoomLocked)
            {
                _cameraFollow.Mode = FollowMode.VerticalFollow;
            }
            else
            {
                _cameraFollow.Mode = FollowMode.Follow;
            }

            System.Console.WriteLine($"[Camera] Режим изменен на: {_cameraFollow.Mode}");
        }

        // управление зумом
        float zoomSpeed = 1.5f * (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_input.IsActionDown("ZoomIn"))
        {
            _camera.Zoom += zoomSpeed;
        }
        if (_input.IsActionDown("ZoomOut"))
        {
            _camera.Zoom -= zoomSpeed;
        }

        // защита: не даем зуму уйти в ноль или отрицательные значения
        if (_camera.Zoom < 0.1f) _camera.Zoom = 0.1f;
        if (_camera.Zoom > 3.0f) _camera.Zoom = 3.0f;

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

        // обновление камеры каждый кадр
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _cameraFollow.Update(_testPosition, deltaTime);

        base.Update(gameTime);
    }


    protected override void Draw(GameTime gameTime)
    {
        _graphics.Renderer.Clear(_settings.ClearColor);
        _graphics.Renderer.Begin(_camera.GetViewMatrix());
        //задаем размеры прямоугольника
        Rectangle testRect = new Rectangle((int)_testPosition.X, (int)_testPosition.Y, 64, 64);
        _graphics.Renderer.DrawRect(testRect, Color.Magenta);

        // визуализация границ мира (красная рамка), чтобы видеть RoomLocked
        if (_cameraFollow.WorldBounds.HasValue)
        {
            Rectangle bounds = _cameraFollow.WorldBounds.Value;
            // рисуем 4 линии, образующие рамку границ мира
            _graphics.Renderer.DrawRect(new Rectangle(bounds.Left, bounds.Top, bounds.Width, 4), Color.Red);
            _graphics.Renderer.DrawRect(new Rectangle(bounds.Left, bounds.Bottom - 4, bounds.Width, 4), Color.Red);
            _graphics.Renderer.DrawRect(new Rectangle(bounds.Left, bounds.Top, 4, bounds.Height), Color.Red);
            _graphics.Renderer.DrawRect(new Rectangle(bounds.Right - 4, bounds.Top, 4, bounds.Height), Color.Red);
        }

        _graphics.Renderer.End();

        base.Draw(gameTime);
    }
}



*/

//пыталась совместить гейм из гита с моим, возникают конфликты возможно из-за того, что у меня пнг нет на компе
using Core.ECS;
using Core.GameData;
using Core.Physics.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Render.Graphics;

using Render.Camera;

using Runtime;
using Runtime.TIledParse;

using System;
using System.IO;
using Render.Input;
using Core.Input;
using Core.Physics.Systems;
namespace MyGame;

public class Game1 : Game
{
    private readonly GraphicsDeviceManager _gdm;
    private readonly WindowSettings _settings;
    private GraphicsService _graphics;
    private InputManager _input;
    private Camera2d _camera;
    private CameraFollow _cameraFollow;
    private Render.Camera.Viewport _viewport;
    private Vector2 _testPosition = new Vector2(600, 300);

    Transform transform;
    Entity koza;

    private Renderer _renderer;
    private MapData _mapData;

    private World _world;
    private ResourceManager _manager;
    private RenderSystem _renderSystem;

    public Game1()
    {
        _settings = new WindowSettings
        {
            Width = 1280,
            Height = 720,
            Title = "MyGame",
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

        Content.RootDirectory = "Content";
        IsFixedTimeStep = _settings.FixedTimeStep;
        Window.AllowUserResizing = _settings.AllowResize;
        Window.Title = _settings.Title;
        IsMouseVisible = true;
    }
    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _graphics = new GraphicsService(GraphicsDevice, _settings);
        _input = new InputManager();

        _renderer = _graphics.Renderer;

        // инициализация камеры и вьюпорта
        _camera = new Camera2d(_settings.Width, _settings.Height);
        _cameraFollow = new CameraFollow(_camera, _settings.Height, _settings.Width);
        _viewport = new Render.Camera.Viewport(_camera, _settings.Width, _settings.Height);

        // пусть границы мира 2000x1000, чтобы проверить RoomLocked
        _cameraFollow.WorldBounds = new Rectangle(0, 0, 2000, 1000);
        // пусть фиксированный X для VerticalFollow
        _cameraFollow.FixedX = 640f;
        _manager = new ResourceManager(GraphicsDevice);
        _renderSystem = new RenderSystem(_renderer, _manager);



        _world = new World();
        koza = _world.CreateEntity();

        transform = new Transform(100, 100);
        Velocity velocity = new Velocity(100, 100);
        string path = Path.Combine(AppContext.BaseDirectory, "Data", "32tile.png");
        InputComponent inputComponent = new InputComponent(200f);
        Sprite sprite1 = new Sprite(_manager.LoadTexture(path), 4);

        _world.AddComponent(koza, transform);
        _world.AddComponent(koza, velocity);
        _world.AddComponent(koza, sprite1);
        _world.AddComponent(koza, inputComponent);

        _world.AddSystem(new InputSystem(new GameInputProvider(_input)));
        _world.AddSystem(new PlayerMovementSystem());
        _world.AddSystem(new MovementSystem());

        path = Path.Combine(AppContext.BaseDirectory, "Data", "sample.tmj");
        LevelLoader.LoadLevel(path, _world, GraphicsDevice, _renderer, _manager);

    }

    protected override void Update(GameTime gameTime)
    {
        _input.Update();

        if (_input.IsActionPressed("Pause"))
        {
            Exit();
        }

        // переключение режимов камеры по клавише E (Interact)
        if (_input.IsActionPressed("Interact"))
        {
            if (_cameraFollow.Mode == FollowMode.Follow)
                _cameraFollow.Mode = FollowMode.RoomLocked;
            else if (_cameraFollow.Mode == FollowMode.RoomLocked)
                _cameraFollow.Mode = FollowMode.VerticalFollow;
            else
                _cameraFollow.Mode = FollowMode.Follow;

            System.Console.WriteLine($"[Camera] Режим изменен на: {_cameraFollow.Mode}");
        }

        // управление зумом
        float zoomSpeed = 1.5f * (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_input.IsActionDown("ZoomIn")) _camera.Zoom += zoomSpeed;
        if (_input.IsActionDown("ZoomOut")) _camera.Zoom -= zoomSpeed;

        // защита: не даем зуму уйти в ноль или отрицательные значения
        _camera.Zoom = MathHelper.Clamp(_camera.Zoom, 0.1f, 3.0f);


        //формула для движения, независимого от частоты кадров
        float speed = 400f * (float)gameTime.ElapsedGameTime.TotalSeconds;

        

        // обновление камеры каждый кадр
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        transform = _world.GetComponent<Transform>(koza);

        _cameraFollow.Update(CoreToMonogame.ConvertVector2(transform.Position), deltaTime);

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        
        _world.Update(dt);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _graphics.Renderer.Clear(_settings.ClearColor);
        _graphics.Renderer.Begin(_camera.GetViewMatrix());
        //задаем размеры прямоугольника
        Rectangle testRect = new Rectangle((int)_testPosition.X, (int)_testPosition.Y, 64, 64);
        _graphics.Renderer.DrawRect(testRect, Color.Magenta);

        // визуализация границ мира (красная рамка), чтобы видеть RoomLocked
        if (_cameraFollow.WorldBounds.HasValue)
        {
            Rectangle bounds = _cameraFollow.WorldBounds.Value;
            // рисуем 4 линии, образующие рамку границ мира
            _graphics.Renderer.DrawRect(new Rectangle(bounds.Left, bounds.Top, bounds.Width, 4), Color.Red);
            _graphics.Renderer.DrawRect(new Rectangle(bounds.Left, bounds.Bottom - 4, bounds.Width, 4), Color.Red);
            _graphics.Renderer.DrawRect(new Rectangle(bounds.Left, bounds.Top, 4, bounds.Height), Color.Red);
            _graphics.Renderer.DrawRect(new Rectangle(bounds.Right - 4, bounds.Top, 4, bounds.Height), Color.Red);
        }

       

        // В) ECS Сущности (код сокомандников)
        _renderSystem.DrawRenderList(_world);

        // 3. ОДИН End на весь кадр
        _graphics.Renderer.End();

        base.Draw(gameTime);
    }
}