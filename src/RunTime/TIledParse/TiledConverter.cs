using Core.GameData.TileMap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Runtime.TIledParse;

//Конвретация map.tmj в MapData
public static class TiledConverter
{
    private const uint GID_MASK = 0x0FFFFFFF;

    //Конвертирует tmj файл в мою структуру MapData.
    public static MapData Convert(string tmjPath)
    {
        string? mapDir = Path.GetDirectoryName(Path.GetFullPath(tmjPath));
        string json = File.ReadAllText(tmjPath);
        var tiled = JsonSerializer.Deserialize<TiledFormat>(json)
        ?? throw new InvalidDataException("Неправильный файл " + tmjPath);
        
        //========================= заполняем тайлсеты ==============================
        var tilesets = new List<TilesetData>();
        foreach (var tileset in tiled.TileSets ?? new())
        {
            if (tileset.Source == null) continue;


            // если source не null, то и mapDir не null
            string tsxPath = Path.Combine(mapDir!, tileset.Source);
            TiledTileset? tiledTileset = TiledTileset.Load(tsxPath);
            if (tiledTileset == null) continue;

            switch (tiledTileset.Type)
            {
                case TilesetType.Atlas:
                    tilesets.Add(ParseAtlas(tiledTileset, tileset, mapDir!));
                    break;
                case TilesetType.Collection:
                    tilesets.Add(ParseCollection(tiledTileset, tileset, mapDir!));
                    break;
            }
        }

        //======================= заполняем слои ==========================================
        var layers = new List<LayerData>();
        foreach (var layer in tiled.Layers ?? new())
        {
            switch (layer.Type)
            {
                case "tilelayer":
                    layers.Add(ConvertTile(layer));
                    break;
                case "objectgroup":
                    layers.Add(ConvertObject(layer, tilesets));
                    break;
            }
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
            Type = LayerType.Tile,
            Name = tile.Name,
            Visible = tile.Visible,
            Rows = tile.Height,
            Cols = tile.Width,
            Grid = grid
        };

    }
    private static LayerData ConvertObject(TiledLayer obj, List<TilesetData>? tilesets)
    {
        var objects = new List<ObjectData>();

        foreach (var o in obj.Objects ?? new())
        {
            var tileset = FindTileset(o.Gid & GID_MASK, tilesets);
            if (tileset is null) continue;

            float scaleX = o.X / tileset.TileWidth;
            float scaleY = o.Y / tileset.TileHeight;

            objects.Add(new ObjectData
            {
                Name = o.Name,
                Gid = o.Gid & GID_MASK,
                Height = o.Height,
                Width = o.Width,
                Rotation = o.Rotation,
                X = o.X,
                Y = o.Y,
                Visible = o.Visible,
                Opacity = o.Opacity,
                ScaleX = scaleX,
                ScaleY = scaleY
            });
        }

        return new LayerData
        {
            Type = LayerType.Object,
            Name = obj.Name,
            Visible = obj.Visible,
            DrawOrder = obj.DrawOrder,
            Objects = objects
        };
    }

    //ищем тайлсет которому принадлежит gid для скейлов
    private static TilesetData? FindTileset(long? gid, List<TilesetData>? tilesets)
    {
        if (gid is null) return null;

        foreach (var tileset in tilesets ?? [])
        {
            if (gid >= tileset.FirstGid &&  gid <= tileset.LastGid)
            {
                return tileset;
            }
        }
        return null;
    }

    private static TilesetData ParseAtlas(TiledTileset tiledTileset, Tileset tileset, string mapDir)
    {
        //тип Atlas гарантирует что Image не null (смотреть TiledTileset.cs)
        string imagePath = Path.Combine(mapDir!, tiledTileset.Image!.Source);

        var image = new Runtime.ImageData
        {
            Source = imagePath,
            Height = tiledTileset.Image.Height,
            Width = tiledTileset.Image.Width
        };

        return new TilesetData
        {
            Type = Runtime.TilesetType.Atlas,
            FirstGid = tileset.FirstGid,
            LastGid = tileset.FirstGid + tiledTileset.TileCount - 1,
            Source = tileset.Source,
            Name = tiledTileset.Name,
            TileWidth = tiledTileset.TileWidth,
            TileHeight = tiledTileset.TileHeight,
            TileCount = tiledTileset.TileCount,
            Columns = tiledTileset.Columns,
            Margin = tiledTileset.Margin,
            Spacing = tiledTileset.Spacing,
            Image = image
        };
    }

    private static TilesetData ParseCollection(TiledTileset tiledTileset, Tileset tileset, string mapDir)
    {
        //тайлы с id и imageData
        var tiles = tiledTileset.Tiles;
        var images = new List<Runtime.ImageData>();

        foreach (var tile in tiles ?? [])
        {
            //опять же мой код гарантирует что там не null
            string imagePath = Path.Combine(mapDir!, tile.Image!.Source);

            var image = tile.Image;

            images.Add(new Runtime.ImageData
            {
                Source = imagePath,
                Height = tile.Image.Height,
                Width = tile.Image.Width
            });

        }
        return new TilesetData
        {
            Type = Runtime.TilesetType.Collection,
            FirstGid = tileset.FirstGid,
            LastGid = tileset.FirstGid + tiledTileset.TileCount - 1,
            Source = tileset.Source,
            Name = tiledTileset.Name,
            TileWidth = tiledTileset.TileWidth,
            TileHeight = tiledTileset.TileHeight,
            TileCount = tiledTileset.TileCount,
            Columns = tiledTileset.Columns,
            Margin = tiledTileset.Margin,
            Spacing = tiledTileset.Spacing,
            Images = images
        };
    }
}
