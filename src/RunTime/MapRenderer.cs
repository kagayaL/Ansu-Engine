using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Render.Graphics;


namespace Runtime.MapRenderer;

public sealed class MapRenderer
{
    private int _tileSize;
    private Dictionary<int, Texture2D> _textures = new();
    
    //Рисует карту в пределах vieport
    public void DrawMap(Renderer renderer, MapData data
        /*Rectangle vieport*/)
    {

        //отрисовываю каждый уровень отдельно
        foreach (var layer in data.LayersData ?? new())
        {
            if (layer.Type == LayerType.Object) continue;
            DrawLayer(renderer, layer, data);

        }
        
    }
    //Функция запускается при LoadContent в Game. Необходимо чтобы программа не загружала
    //все текстуры 60 раз в секунду
    public void LoadTextures(MapData data, GraphicsDevice device)
    {
        _tileSize = data.TileSize;
        foreach (var tileset in data.TilesetsData ?? [])
        {
            if (string.IsNullOrEmpty(tileset.ImageSource)) continue;
            _textures[tileset.FirstGid] = Texture2D.FromFile(device, tileset.ImageSource);
        }
    }

    private void DrawLayer(Renderer renderer, LayerData layer,
        MapData data /*Rectangle vieport*/)
    {
        long[]? grid = layer.Grid;
        if (grid == null) return;

        for (int i = 0; i < grid.Length; i++)
        {
            Rectangle rectangle;
            Texture2D? texture = FindTexture(grid[i], data.TilesetsData, out rectangle);

            if (texture == null) continue;

            int col = i % layer.Cols;
            int row = i / layer.Cols;
            Vector2 pos = new Vector2(col * _tileSize, row * _tileSize);
            
            renderer.DrawSprite(texture, pos, rectangle);
        }
    }

    //ищет текстуру в словаре по индексу тайла
    //и создает правильный прямоугольник для вырезки тайла
    private Texture2D? FindTexture(long gid, List<TilesetData>? tilesetData, out Rectangle rectangle)
    {
        //тайл с gid = 0 - пустой тайл
        if ( gid == 0)
        {
            rectangle = Rectangle.Empty;
            return null;
        }

        foreach (var tileset in tilesetData ?? new()) 
        {


            if ( gid >= tileset.FirstGid && gid <= tileset.LastGid)
            {
                int localId = (int)gid - tileset.FirstGid;
                int col = localId % tileset.Columns;
                int row = localId / tileset.Columns;
                int x = tileset.Margin + col * (tileset.TileWidth + tileset.Spacing);
                int y = tileset.Margin + row * (tileset.TileHeight + tileset.Spacing);

                //проверка на существует ли в словаре нужная текстура, чтобы программа не упала
                if (!_textures.TryGetValue(tileset.FirstGid, out var texture))
                    continue;

                rectangle = new Rectangle(x, y, tileset.TileWidth, tileset.TileHeight);
                return texture;
            }
        }
        rectangle = Rectangle.Empty;
        return null;
    }
    
}
