using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Runtime.TIledParse;

public enum TilesetType
{
    Atlas,
    Collection
}

//Данные о тайлсете из tiled
public record class TiledTileset
{
    public TilesetType Type { get; set; }
    public string Name { get; init; } = "";
    public int TileWidth { get; init; }
    public int TileHeight { get; init; }
    public int TileCount { get; init; }
    public int Columns { get; init; }
    public int Margin { get; init; } //отступ от края png
    public int Spacing { get; init; } //пробелы между тайлами
    public GridData? Grid { get; init; } = null;
    public ImageData? Image { get; init; } = null;
    public List<TileData>? Tiles { get; init; } = null;

    public static TiledTileset? Load(string tsxPath)
    {
        var doc = XDocument.Load(tsxPath);
        var root = doc.Root!; // <tileset>
        var grid = root.Element("grid");
        var image = root.Element("image"); // <image>
        var tiles = root.Elements("tile").ToList();


        //если есть image, то не надо обрабатывать как коллекцию
        if (image is not null)
        {
            return new TiledTileset
            {
                Name = (string?)root.Attribute("name") ?? "",
                TileWidth = (int?)root.Attribute("tilewidth") ?? 0,
                TileHeight = (int?)root.Attribute("tileheight") ?? 0,
                TileCount = (int?)root.Attribute("tilecount") ?? tiles.Count,
                Columns = (int?)root.Attribute("columns") ?? 0,
                Margin = (int?)root.Attribute("margin") ?? 0,
                Spacing = (int?)root.Attribute("spacing") ?? 0,
                Image = ParseImage(image),
                Type = TilesetType.Atlas
            };
        } else if (tiles.Count != 0) //значит eсть коллекция изображений
        {
            return new TiledTileset
            {
                Name = (string?)root.Attribute("name") ?? "",
                TileWidth = (int?)root.Attribute("tilewidth") ?? 0,
                TileHeight = (int?)root.Attribute("tileheight") ?? 0,
                TileCount = (int?)root.Attribute("tilecount") ?? tiles.Count,
                Columns = (int?)root.Attribute("columns") ?? 0,
                Margin = (int?)root.Attribute("margin") ?? 0,
                Spacing = (int?)root.Attribute("spacing") ?? 0,
                Grid = ParseGrid(grid),
                Tiles = ParseTile(tiles) ?? [],
                Type = TilesetType.Collection
            };
        }
        return null;

        
    }

    private static ImageData ParseImage(XElement image)
    {
        return new ImageData
        {
            Source = (string?)image.Attribute("source") ?? "",
            Width = (int?)image.Attribute("width") ?? 0,
            Height = (int?)image.Attribute("height") ?? 0
        };
    }
    
    private static GridData? ParseGrid(XElement? grid)
    {
        if (grid is null) return null;
        return new GridData
        {
            Orientation = (string?)grid.Attribute("orientation") ?? "orthogonal",
            Height = (int?)grid.Attribute("height") ?? 0,
            Width = (int?)grid.Attribute("width") ?? 0
        };
    }

    private static List<TileData> ParseTile(List<XElement> tiles)
    {
        List<TileData> result = new List<TileData>();
 
        foreach (var tile in tiles)
        {
            var image = tile.Element("image");
            if (image is null) continue;

            var item = new TileData
            {
                Id = (int?)tile.Attribute("id") ?? 0,
                Image = ParseImage(image)
            };

            result.Add(item);
        }
        return result;
    }
}

public record class TileData
{
    public int Id { get; init; } = 0;
    public ImageData? Image { get; init; } = null;

}
public record class ImageData
{
    public string Source { get; init; } = "";
    public int Width { get; init; }
    public int Height { get; init; }

}

public record class GridData
{
    public string Orientation { get; init; } = "orthogonal";
    public int Width { get; init; }
    public int Height { get; init; }
}