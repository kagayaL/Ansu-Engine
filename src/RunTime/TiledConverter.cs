using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace Runtime;  

public static class TiledConverter
{
    private const uint GID_MASK = 0x0FFFFFFF;
    public static MapData Convert(string tmjPath)
    {
        string? mapDir = Path.GetDirectoryName(Path.GetFullPath(tmjPath));
        string json = File.ReadAllText(tmjPath);
        var tiled = JsonSerializer.Deserialize<TiledFormat>(json)
        ?? throw new InvalidDataException("Неправильный файл " + tmjPath);
        var layers = new List<LayerData>();

        foreach(var layer in tiled.Layers ?? new())
        {
            switch (layer.Type)
            {
                case "tilegroup":
                    layers.Add(ConvertTile(layer));
                    break;
                case "objectgroup":
                    layers.Add(ConvertObject(layer));
                    break;
            }
        }

        var tilesets = new List<TilesetData>();
        foreach(var tileset in tiled.TileSets ?? new())
        {
            tilesets.Add(new TilesetData
            {
                FirstGid = tileset.FirstGid,
                Source = tileset.Source
            });
        }
        return new MapData
        {
            Height = tiled.Height,
            Width = tiled.Width,
            TileSize = tiled.TileWidth,
            LayersData = layers,
            TilesetsData = tilesets
        };
    }

    private static LayerData ConvertTile(TiledLayer tile)
    {
        var data = tile.Data ?? [];
        var grid = new long[data.Length];

        for (int i = 0; i < grid.Length; i++)
        {
            grid[i] = data[i] & GID_MASK;
        }

        return new LayerData
        {
            Type = LayerType.TILE,
            Name = tile.Name,
            Rows = tile.Height,
            Cols = tile.Width,
            Grid = grid
        };

    }
    private static LayerData ConvertObject(TiledLayer obj)
    {
        var objects = new List<ObjectData>();

        foreach(var o in obj.Objects ?? new())
        {
            objects.Add(new ObjectData
            {
                Name = o.Name,
                Gid = o.Gid & GID_MASK,
                Height = o.Height,
                Width = o.Width,
                Rotation = o.Rotation,
                X = o.X, Y = o.Y,
                Visible = o.Visible,
                Opacity = o.Opacity
            });
        }

        return new LayerData
        {
            Type = LayerType.OBJECT,
            Name = obj.Name,
            DrawOrder = obj.DrawOrder,
            Objects = objects
        };
    }
}

