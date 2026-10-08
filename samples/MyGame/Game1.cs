using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Render.Graphics;   // ← namespace твоего Renderer

namespace MyGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private Renderer _renderer;
    private Texture2D _testTexture, _secondTexture;

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

        // Создаём текстуру 64×64, заливаем красным
        _testTexture = new Texture2D(GraphicsDevice, 64, 64);
        _secondTexture = new Texture2D(GraphicsDevice, 64, 64);
        Color[] data = new Color[64 * 64];
        for (int i = 0; i < data.Length; i++)
            data[i] = Color.Red;
        _secondTexture.SetData(data);
        _testTexture = Texture2D.FromFile(GraphicsDevice, "C:\\Users\\Denis\\Desktop\\FillerName\\src\\Tests\\RunTime\\Data\\32tile.png", null);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _renderer.Clear(Color.CornflowerBlue);

        _renderer.Begin();
        _renderer.DrawSprite(_testTexture, new Vector2(100, 100));
        _renderer.DrawSprite(_secondTexture, new Vector2(10, 10));
        _renderer.End();

        base.Draw(gameTime);
    }
}