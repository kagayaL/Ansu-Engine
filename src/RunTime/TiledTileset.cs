using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Runtime;

public record class TiledTileset
{
    public string Name { get; init; } = "";
    public int TileWidth { get; init; }
    public int TileHeight { get; init; }
    public int TileCount { get; init; }
    public int Columns { get; init; }
    public int Margin { get; init; } //отступ от края png
    public int Spacing { get; init; } //пробелы между тайлами
    public string ImageSource { get; init; } = "";
    public int ImageWidth { get; init; }
    public int ImageHeight { get; init; }

    public static TiledTileset? Load(string tsxPath)
    {
        var doc = XDocument.Load(tsxPath);
        var root = doc.Root!;                       // <tileset>
        var image = root.Element("image")!;         // <image>
        if (image == null) return null;

        return new TiledTileset
        {
            Name = (string?)root.Attribute("name") ?? "",
            TileWidth = (int?)root.Attribute("tilewidth") ?? 0,
            TileHeight = (int?)root.Attribute("tileheight") ?? 0,
            TileCount = (int?)root.Attribute("tilecount") ?? 0,
            Columns = (int?)root.Attribute("columns") ?? 0,
            Margin = (int?)root.Attribute("margin") ?? 0, 
            Spacing = (int?)root.Attribute("spacing") ?? 0, 
            ImageSource = (string?)image.Attribute("source") ?? "",
            ImageWidth = (int?)image.Attribute("width") ?? 0,
            ImageHeight = (int?)image.Attribute("height") ?? 0
        };
    }
}