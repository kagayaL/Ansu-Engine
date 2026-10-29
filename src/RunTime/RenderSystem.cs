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
                case DrawType.Tilelayer:
                    DrawTileLayer(drawObject);
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
            else if (world.HasComponent<TileMapComponent>(item))
            {
                tileMap = world.GetComponent<TileMapComponent>(item);
                depth = tileMap.Value.Layer;
                type = DrawType.Tilelayer;
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

    /*  public void DrawSprite(Texture2D tex, Vector2 pos, float rotation, Vector2 origin, Rectangle? src = null,
    Color? tint = null, Vector2? scale = null, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0f)*/

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

    private void DrawTileLayer(DrawData tileLayer)
    {
        if (tileLayer.TileMapComponent == null) return;

        TileMapComponent layer = tileLayer.TileMapComponent.Value;
        Transform t = tileLayer.Transformed;

        long[]? grid = layer.Grid;
        if (grid == null) return;

        int tileSize = layer.TileSize;

        // Всё считаем в Monogame-координатах
        float scaleX = t.Scale.X;
        float scaleY = t.Scale.Y;
        float rotation = t.Rotation;
        float cos = MathF.Cos(rotation);
        float sin = MathF.Sin(rotation);
        Vector2 layerPos = CoreToMonogame.ConvertVector2(t.Position);
        Vector2 spriteScale = new Vector2(scaleX, scaleY);
        Vector2 centerOrigin = new Vector2(0.5f, 0.5f);   // origin в центре тайла

        for (int i = 0; i < grid.Length; i++)
        {
            Rectangle rectangle;
            Texture2D? texture = _manager.GetTexture(layer.TilesetsId, grid[i], out rectangle);
            if (texture == null) continue;

            int col = i % layer.Cols;
            int row = i / layer.Cols;

            // 1. Локальный центр тайла (в координатах слоя, без трансформа)
            float localCx = col * tileSize + tileSize * 0.5f;
            float localCy = row * tileSize + tileSize * 0.5f;

            // 2. Масштаб
            float scx = localCx * scaleX;
            float scy = localCy * scaleY;

            // 3. Поворот вокруг (0,0) слоя
            float rcx = scx * cos - scy * sin;
            float rcy = scx * sin + scy * cos;

            // 4. Сдвиг
            Vector2 worldCenter = new Vector2(rcx + layerPos.X, rcy + layerPos.Y);

            // 5. Рисуем
            _renderer.DrawSprite(
                texture,
                worldCenter,      // position = мировой центр тайла
                rotation,         // поворот слоя
                centerOrigin,     // origin = (0.5, 0.5) — центр
                rectangle,
                spriteScale);
        }
    }
}

