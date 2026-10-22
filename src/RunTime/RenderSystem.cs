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
using Core.ECS;
using Core.GameData.TileMap;
using System.Runtime;

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
    public void DrawSprites(World world) // потом очередь поменять на world и уже из него брать
    {
        LoadSprites(world);
        foreach (var sprite in _sprites)
        {
            if (sprite.Sprite != null)
            {
                Texture2D? texture = _manager.GetTexture(sprite.Sprite.Value.TextureId);

                if (texture == null) continue;

                Vector2 position = CoreToMonogame.ConvertVector2(sprite.Position.Coordinates);
                Rectangle? rect = CoreToMonogame.ConvertRectangle(sprite.Sprite.Value.SourceRect);

                _renderer.DrawSprite(texture, position, rect);
            }
            
        }
        
    }

    //Из очереди грузим в мой список и сортируем по слою, чтобы правильно рисовать
    private void LoadSprites(World world)
    {
        List<Entity> query = world.GetRenderList();

        _sprites.Clear();
        foreach (var item in query)
        {
            Sprite? sprite = null;
            TileMapComponent? tileMap = null;
            int depth = 0;

            if (world.HasComponent<Sprite>(item))
            {
                sprite = world.GetComponent<Sprite>(item);
                depth = sprite.Value.Layer;
            }
            else if (world.HasComponent<TileMapComponent>(item))
            {
                tileMap = world.GetComponent<TileMapComponent>(item);
                depth = tileMap.Value.Layer;
            }

            SpriteData data = new SpriteData
                {
                    Position = world.GetComponent<Position>(item),
                    Sprite = sprite,
                    TileMapComponent = tileMap,
                    Layer = depth
    
            };
            _sprites.Add(data);
        }
        _sprites.Sort();
    }
}

