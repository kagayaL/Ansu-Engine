using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Render.Assets;

public class AssetCache
{
    private readonly ContentManager _content;
    private readonly GraphicsDevice _graphicsDevice;

    //кэш для текстур (тайлсеты, спрайты)
    private readonly Dictionary<string, Texture2D> _textureCache;
    
    //кэш для шрифтов (SpriteFont)
    private readonly Dictionary<string, SpriteFont> _fontCache;
    
    //заглушка: розовый квадрат 1x1 пиксель
    private Texture2D _fallbackTexture;

    public AssetCache(ContentManager content, GraphicsDevice graphicsDevice)
    {
        _content = content ?? throw new ArgumentNullException(nameof(content));
        _graphicsDevice = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));
        
        _textureCache = new Dictionary<string, Texture2D>();
        _fontCache = new Dictionary<string, SpriteFont>();

        _fallbackTexture = new Texture2D(_graphicsDevice, 1, 1);
        _fallbackTexture.SetData(new[] { Color.Magenta });
    }

    //получает текстуру. Если она уже в кэше — возвращает из кэша. 
    //если нет — загружает. Если файла нет — возвращает розовую заглушку.
    public Texture2D GetTexture(string path)
    {
        if (_textureCache.TryGetValue(path, out Texture2D cachedTexture))
        {
            return cachedTexture;
        }
        
        try
        {
            Texture2D newTexture = _content.Load<Texture2D>(path);
            _textureCache[path] = newTexture;
            return newTexture;
        }
        catch (ContentLoadException)
        {
            Console.WriteLine($"[AssetCache] Не удалось загрузить текстуру '{path}'. Используется заглушка.");
            _textureCache[path] = _fallbackTexture; 
            return _fallbackTexture;
        }
    }

    //получает шрифт. Работает по тому же принципу, что и GetTexture.
    public SpriteFont? GetFont(string path)
    {
        if (_fontCache.TryGetValue(path, out SpriteFont cachedFont))
        {
            return cachedFont;
        }

        try
        {
            SpriteFont newFont = _content.Load<SpriteFont>(path);
            _fontCache[path] = newFont;
            return newFont;
        }
        catch (ContentLoadException)
        {
            Console.WriteLine($"[AssetCache] Не удалось загрузить шрифт '{path}'.");
            return null;
        }
    }

    //очищает кэш
    public void ClearCache()
    {
        _textureCache.Clear();
        _fontCache.Clear();
    }
}
