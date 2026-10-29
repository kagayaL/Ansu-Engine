using Core.ECS;
using Core.GameData;
using Core.GameData.TileMap;
using Core.Physics.Components;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Render.Graphics;

using Runtime.TIledParse;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Runtime;

public class RenderSystem
{
    private List<DrawData> _drawList = [];
    private Renderer _renderer;
    private ResourceManager _manager;

    public RenderSystem(Renderer renderer, ResourceManager manager)
    {
        _renderer = renderer;
        _manager = manager;
    }

    //рисует спрайты из очереди. перед этим батч должен быть открыт
    public void DrawRenderList(World world) // потом очередь поменять на world и уже из него брать
    {
        LoadSprites(world);
        foreach (var drawObject in _drawList)
        {
            switch (drawObject.DrawType)
            {
                case DrawType.Sprite:
                    DrawSprite(drawObject);
                    break;
                default:
                    continue;
            }
        }
    }
    

    //Из очереди грузим в мой список и сортируем по слою, чтобы правильно рисовать
    private void LoadSprites(World world)
    {
        List<Entity> query = world.GetRenderList();

        _drawList.Clear();
        foreach (var item in query)
        {
            Sprite? sprite = null;
            TileMapComponent? tileMap = null;
            DrawType type = DrawType.Sprite;
            int depth = 0;

            if (world.HasComponent<Sprite>(item))
            {
                sprite = world.GetComponent<Sprite>(item);
                depth = sprite.Value.Layer;
            }

            DrawData data = new DrawData
            {
                Transformed = world.GetComponent<Transform>(item),
                Sprite = sprite,
                TileMapComponent = tileMap,
                Layer = depth,
                DrawType = type

            };
            _drawList.Add(data);
        }
        _drawList.Sort();
    }

    private void DrawSprite(DrawData sprite)
    {
        if (sprite.Sprite == null) return;

        Texture2D? texture = _manager.GetTexture(sprite.Sprite.Value.TextureId);

        if (texture == null) return;

        Vector2 position = CoreToMonogame.ConvertVector2(sprite.Transformed.Position);
        Vector2 scale = CoreToMonogame.ConvertVector2(sprite.Transformed.Scale);
        Vector2 origin = CoreToMonogame.ConvertVector2(sprite.Transformed.Origin);
        Rectangle? rectangle = CoreToMonogame.ConvertRectangle(sprite.Sprite.Value.SourceRect);

        _renderer.DrawSprite(texture, position, sprite.Transformed.Rotation, origin, rectangle, scale);
    }

}

