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
    public void DrawRenderList(World world, Render.Camera.Viewport viewport) // потом очередь поменять на world и уже из него брать
    {
        LoadSprites(world, viewport);
        foreach (var drawObject in _drawList)
        {
            DrawSprite(drawObject);     
        }
    }
    

    //Из очереди грузим в мой список и сортируем по слою, чтобы правильно рисовать
    private void LoadSprites(World world, Render.Camera.Viewport viewport)
    {
        List<Entity> query = world.GetRenderList();

        _drawList.Clear();
        foreach (var item in query)
        {
            Texture2D? texture;
            Sprite sprite;
            Transform transform;
            int depth = 0;

            if (!world.HasComponent<Sprite>(item) || !world.HasComponent<Transform>(item)) continue;
            
            sprite = world.GetComponent<Sprite>(item);
            transform = world.GetComponent<Transform>(item);
            depth = sprite.Layer;
            texture = _manager.GetTexture(sprite.TextureId);
            
            if (texture is null) continue;

            
            Core.Math.Vector2 size = sprite.SourceRect is null ? 
                new Core.Math.Vector2(texture.Width, texture.Height) :
                new Core.Math.Vector2(sprite.SourceRect.Value.Width, sprite.SourceRect.Value.Height);
            //находим границы спрайта
            Rectangle aabb = Culling.CalculateAABB(size, transform.Position, transform.Rotation, transform.Origin, transform.Scale);
            //проверяем находится ли объект в пределах вьюпорта
            if (!viewport.IsVisible(aabb)) continue;

            DrawData data = new DrawData
            {
                Transformed = transform,
                Sprite = sprite,
                Layer = depth

            };
            _drawList.Add(data);
        }
        _drawList.Sort();
    }

    private void DrawSprite(DrawData sprite)
    {

        Texture2D? texture = _manager.GetTexture(sprite.Sprite.TextureId);

        if (texture is null) return;

        Vector2 position = CoreToMonogame.ConvertVector2(sprite.Transformed.Position);
        Vector2 scale = CoreToMonogame.ConvertVector2(sprite.Transformed.Scale);
        Vector2 origin = CoreToMonogame.ConvertVector2(sprite.Transformed.Origin);
        Rectangle? rectangle = CoreToMonogame.ConvertRectangle(sprite.Sprite.SourceRect);

        _renderer.DrawSprite(texture, position, sprite.Transformed.Rotation, origin, rectangle, scale);
    }

}

