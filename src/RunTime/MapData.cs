using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Runtime;

public record class MapData
{
    [JsonPropertyName("width")] public int Width { get; init; }
    [JsonPropertyName("height")] public int Height { get; init; }
    [JsonPropertyName("tile_size")] public int TileSize { get; init; }
    [JsonPropertyName("layers")] public List<LayerData>? LayersData { get; init; }
    [JsonPropertyName("tilesets")] public List<TilesetData>? TilesetsData { get; init; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LayerType
{
    [JsonStringEnumMemberName("tile")] Tile,
    [JsonStringEnumMemberName("object")] Object
}

public record class LayerData
{
    [JsonPropertyName("type")] public LayerType Type { get; init; }
    [JsonPropertyName("name")] public string? Name { get; init; }
    [JsonPropertyName("visible")] public bool? Visible { get; init; }

    // Для Tile
    [JsonPropertyName("grid")] public long[]? Grid { get; init; }
    [JsonPropertyName("cols")] public int Cols { get; init; }
    [JsonPropertyName("rows")] public int Rows { get; init; }

    // Для Object
    [JsonPropertyName("draw_order")] public string? DrawOrder { get; init; }
    [JsonPropertyName("objects")] public List<ObjectData>? Objects { get; init; }
}

public record class TilesetData
{
    [JsonPropertyName("first_gid")] public int FirstGid { get; init; }
    [JsonPropertyName("last_gid")] public int LastGid { get; init; }
    [JsonPropertyName("source")] public string? Source { get; init; }
    [JsonPropertyName("name")] public string Name { get; init; } = "";
    [JsonPropertyName("tile_width")] public int TileWidth { get; init; }
    [JsonPropertyName("tile_height")] public int TileHeight { get; init; }
    [JsonPropertyName("tile_count")] public int TileCount { get; init; }
    [JsonPropertyName("columns")] public int Columns { get; init; }
    [JsonPropertyName("margin")] public int Margin { get; init; }
    [JsonPropertyName("spacing")] public int Spacing { get; init; }
    [JsonPropertyName("image_source")] public string ImageSource { get; init; } = "";
    [JsonPropertyName("image_width")] public int ImageWidth { get; init; }
    [JsonPropertyName("image_height")] public int ImageHeight { get; init; }
}

public record class ObjectData
{
    [JsonPropertyName("name")] public string? Name { get; init; }
    [JsonPropertyName("gid")] public long? Gid { get; init; }
    [JsonPropertyName("height")] public float Height { get; init; }
    [JsonPropertyName("width")] public float Width { get; init; }
    [JsonPropertyName("rotation")] public float Rotation { get; init; }
    [JsonPropertyName("x")] public float X { get; init; }
    [JsonPropertyName("y")] public float Y { get; init; }
    [JsonPropertyName("visible")] public bool Visible { get; init; }
    [JsonPropertyName("opacity")] public float Opacity { get; init; }
}