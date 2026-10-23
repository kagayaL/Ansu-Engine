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
                    Position = world.GetComponent<Position>(item),
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

        Vector2 position = CoreToMonogame.ConvertVector2(sprite.Position.Coordinates);
        Rectangle? rect = CoreToMonogame.ConvertRectangle(sprite.Sprite.Value.SourceRect);

        _renderer.DrawSprite(texture, position, rect);
    }

    private void DrawTileLayer(DrawData tileLayer) 
    {
        Console.WriteLine("draw layer");
        if (tileLayer.TileMapComponent == null) return;

        TileMapComponent? layer = tileLayer.TileMapComponent;

        long[]? grid = layer.Value.Grid;
        if (grid == null) return;

        int tileSize = layer.Value.TileSize;

        for (int i = 0; i < grid.Length; i++)
        {
            Rectangle rectangle;
            Texture2D? texture = _manager.GetTexture(layer.Value.TilesetsId, grid[i], out rectangle);

            if (texture == null) continue;

            int col = i % layer.Value.Cols;
            int row = i / layer.Value.Cols;
            Vector2 pos = new Vector2(col * tileSize, row * tileSize);

            _renderer.DrawSprite(texture, pos, rectangle);
        }
    }
}

