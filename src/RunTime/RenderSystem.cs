using Core.GameData;
using Core.Physics.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Render.Graphics;

namespace Runtime;
public class RenderSystem
{
    private List<SpriteData> _sprites = [];
    private Renderer _renderer;
    private ResourceManager _manager;
    
    public RenderSystem(Renderer renderer, ResourceManager manager)
    {
        _renderer = renderer;
        _manager = manager;
    }

    //рисует спрайты из очереди. перед этим батч должен быть открыт
    public void DrawSprites(List<(Position, Sprite)> query) // потом очередь поменять на world и уже из него брать
    {
        LoadSprites(query);
        foreach (var sprite in _sprites)
        {
            Texture2D texture = _manager.GetTexture(sprite.Sprite.TextureId);

            Vector2 position = CoreToMonogame.ConvertVector2(sprite.Position.Coordinates);
            Rectangle? rect = CoreToMonogame.ConvertRectangle(sprite.Sprite.SourceRect);
            
            _renderer.DrawSprite(texture, position, rect);
        }
        
    }

    //Из очереди грузим в мой список и сортируем по слою, чтобы правильно рисовать
    private void LoadSprites(List<(Position, Sprite)> query)
    {
        _sprites.Clear();
        foreach (var item in query)
        {
            SpriteData data = new SpriteData
            {
                Position = item.Item1,
                Sprite = item.Item2
            };
            _sprites.Add(data);
        }
        _sprites.Sort();
    }
}

