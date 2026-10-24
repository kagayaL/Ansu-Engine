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
using System.Collections.Generic;
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
    private Entity _koza;
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
        _koza = _world.CreateEntity();
        Position position = new Position(100, 100);
        Velocity velocity = new Velocity(100, 100);
        string path = Path.Combine(AppContext.BaseDirectory, "Data", "32tile.png");
        InputComponent inputComponent = new InputComponent(200f);
        Sprite sprite = new Sprite(_manager.LoadTexture(path));
        _world.AddComponent(_koza, position);
        _world.AddComponent(_koza, velocity);
        _world.AddComponent(_koza, sprite);
        _world.AddComponent(_koza, inputComponent);

        _world.AddSystem(new InputSystem(new GameInputProvider(_inputManager)));
        _world.AddSystem(new PlayerMovementSystem());
        _world.AddSystem(new MovementSystem());

        path = Path.Combine(AppContext.BaseDirectory, "Data", "sample.tmj");
        _mapData = TiledConverter.Convert(path);
        MapLoader.LoadMap(_mapData, _world, _manager);

        List<Entity> entities = _world.GetRenderList();

        

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
            _mapRenderer.LoadTextures(_mapData, GraphicsDevice);
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