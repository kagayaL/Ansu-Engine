using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Render.Graphics;
using Runtime;
using Runtime.MapRenderer;
namespace MyGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private Renderer _renderer;
    private Texture2D _testTexture, _secondTexture;
    private MapRenderer _mapRenderer;
    private MapData _mapData;
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
        _renderer = new Renderer(GraphicsDevice);
        _mapRenderer = new();
        _mapData = TiledConverter.Convert("C:\\Users\\Denis\\Desktop\\FillerName\\src\\Tests\\RunTime\\Data\\sample.tmj");
        _mapRenderer.LoadTextures(_mapData, GraphicsDevice);

    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _renderer.Clear(Color.Gray);
        _renderer.Begin();
        _mapRenderer.DrawMap(_renderer, _mapData);
        _renderer.End();

        base.Draw(gameTime);
    }
}