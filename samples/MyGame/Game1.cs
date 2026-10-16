using Core.ECS;
using Core.GameData;
using Core.Physics.Components;
using Core.Physics.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Render.Graphics;
using Runtime;
using Runtime.MapRenderer;
using Runtime.TIledParse;
using System;
using System.Collections.Generic;
using System.IO;
namespace MyGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private Renderer _renderer;
    private Texture2D _testTexture, _secondTexture;
    private MapRenderer _mapRenderer;
    private MapData _mapData;
    private World _world;
    private Entity _koza;
    private ResourceManager _manager;
    private RenderSystem _renderSystem;
    private List<(Position, Sprite)> _query;
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
        _query = [];
        _renderSystem = new RenderSystem();
        _world = new World();
        _manager = new ResourceManager();
        _koza = _world.CreateEntity();
        Position position = new Position(100, 100);
        Velocity velocity = new Velocity(1, 1);
        string path = Path.Combine(AppContext.BaseDirectory, "Data", "32tile.png");
        Sprite sprite = new Sprite(_manager.LoadTexture(path, GraphicsDevice));
        _world.AddComponent(_koza, position);
        _world.AddComponent(_koza, velocity);
        _world.AddComponent(_koza, sprite);
        _world.AddSystem(new MovementSystem());
        _query.Add((_world.GetComponent<Position>(_koza), _world.GetComponent<Sprite>(_koza)));


        _renderer = new Renderer(GraphicsDevice);
        _mapRenderer = new();
        path = Path.Combine(AppContext.BaseDirectory, "Data", "sample.tmj");
        _mapData = TiledConverter.Convert(path);
        _mapRenderer.LoadTextures(_mapData, GraphicsDevice);

    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        _world.Update(1);
        _query[0] = (_world.GetComponent<Position>(_koza), _world.GetComponent<Sprite>(_koza));
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _renderer.Clear(Color.Gray);
        _renderer.Begin();
        _mapRenderer.DrawMap(_renderer, _mapData);
        _renderSystem.DrawSprites(_query, GraphicsDevice, _renderer, _manager);
        _renderer.End();

        base.Draw(gameTime);
    }
}