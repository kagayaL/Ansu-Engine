using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Render.Graphics;   // ← namespace твоего Renderer
using Runtime;
using Runtime.MapRenderer;
namespace MyGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private Renderer _renderer;
    private Texture2D _testTexture, _secondTexture;
    private MapRenderer _mapRenderer;
    private MapData mapData;
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
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
        mapData = TiledConverter.Convert("C:\\Users\\Denis\\Desktop\\FillerName\\src\\Tests\\RunTime\\Data\\sample.tmj");
        // Создаём текстуру 64×64, заливаем красным
        _secondTexture = new Texture2D(GraphicsDevice, 64, 64);
        Color[] data = new Color[64 * 64];
        for (int i = 0; i < data.Length; i++)
            data[i] = Color.Red;
        _secondTexture.SetData(data);
        _testTexture = Texture2D.FromFile(GraphicsDevice, "C:\\Users\\Denis\\Desktop\\FillerName\\src\\Tests\\RunTime\\Data\\TileSet.png", null);
        
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
        Rectangle rec = new Rectangle(128, 64, 32, 32);
        _renderer.Begin();
        _mapRenderer.DrawMap(_renderer, GraphicsDevice, mapData );
        _renderer.End();

        base.Draw(gameTime);
    }
}