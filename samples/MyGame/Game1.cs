using Core.ECS;
using Core.Physics.Systems;
using Core.GameData;
using Core.Physics.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Render.Graphics;

using Runtime;
using Runtime.TIledParse;

using System;
using System.IO;
using Render.Input;
using Core.Input;
namespace MyGame;

public class Game1 : Game
{
    private InputManager _inputManager;
    private GraphicsDeviceManager _graphics;
    private Renderer _renderer;
    private MapData _mapData;
    private World _world;
    private ResourceManager _manager;
    private RenderSystem _renderSystem;
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _manager = new ResourceManager(GraphicsDevice);
        _inputManager = new InputManager();
        _renderer = new Renderer(GraphicsDevice);


        _renderSystem = new RenderSystem(_renderer, _manager);

        

        _world = new World();
        Entity koza = _world.CreateEntity();
        Entity kozel = _world.CreateEntity();

        Core.Math.Vector2 position = new Core.Math.Vector2(100, 100);
        Core.Math.Vector2 scale = new Core.Math.Vector2(1, 1);
        Transform transform = new Transform(position, MathHelper.ToRadians(30f), scale);

        Velocity velocity = new Velocity(100, 100);
        string path = Path.Combine(AppContext.BaseDirectory, "Data", "32tile.png");
        InputComponent inputComponent = new InputComponent(200f);
        Sprite sprite1 = new Sprite(_manager.LoadTexture(path), 4);
        Sprite sprite2 = new Sprite(_manager.LoadTexture(path), 4);

        _world.AddComponent(koza, transform);
        _world.AddComponent(koza, velocity);
        _world.AddComponent(koza, sprite1);

        transform.Position += new Core.Math.Vector2(32, 0);
        transform.Origin = new Core.Math.Vector2(0.5f, -0.5f);
        _world.AddComponent(kozel, transform);
        _world.AddComponent(kozel, sprite2);

        _world.AddComponent(koza, inputComponent);

        _world.AddSystem(new InputSystem(new GameInputProvider(_inputManager)));
        _world.AddSystem(new PlayerMovementSystem());
        _world.AddSystem(new MovementSystem());

        path = Path.Combine(AppContext.BaseDirectory, "Data", "sample.tmj");
        LevelLoader.LoadLevel(path, _world, GraphicsDevice, _renderer, _manager);
        Transform mapTransform = new Transform(0, 0, MathHelper.ToRadians(10f));
        
    }

    protected override void Update(GameTime gameTime)
    {
        _inputManager.Update();
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_inputManager.IsActionPressed("Interact"))
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Data", "sample.tmj");
            path = "C:/Users/Denis/Desktop/FillerName/src/Tests/RunTime/Data/sample.tmj";
            _mapData = TiledConverter.Convert(path);
        }

        _world.Update(dt);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _renderer.Clear(Color.Gray);
        _renderer.Begin();
        //_mapRenderer.DrawMap(_renderer, _mapData);
        _renderSystem.DrawRenderList(_world);
        _renderer.End();

        base.Draw(gameTime);
    }
}