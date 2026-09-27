using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Runtime;

public record class TiledFormat
{
    [JsonPropertyName("compressionlevel")] public int CompressionLevel { get; init; }
    [JsonPropertyName("height")] public int Height { get; init; }
    [JsonPropertyName("infinite")] public bool Infinite { get; init; }
    [JsonPropertyName("layers")] public List<TiledLayer>? Layers { get; init; }
    [JsonPropertyName("nextlayerid")] public long NextLayerId { get; init; }
    [JsonPropertyName("nextobjectid")] public long NextObjectId { get; init; }
    [JsonPropertyName("orientation")] public string? Orientation { get; init; }
    [JsonPropertyName("renderorder")] public string? RenderOrder { get; init; }
    [JsonPropertyName("tiledversion")] public string? TiledVersion { get; init; }
    [JsonPropertyName("tileheight")] public int TileHeight { get; init; }
    [JsonPropertyName("tilesets")] public List<TileSet>? TileSets { get; init; }
    [JsonPropertyName("tilewidth")] public int TileWidth { get; init; }
    [JsonPropertyName("type")] public string? Type { get; init; }
    [JsonPropertyName("version")] public string? Version { get; init; }
    [JsonPropertyName("width")] public int Width { get; init; }
}

public record class TileSet
{
    [JsonPropertyName("firstgid")] public int FirstGid { get; init; }
    [JsonPropertyName("source")] public string? Source { get; init; }

}

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(TiledTileLayer), "tilelayer")]
[JsonDerivedType(typeof(TiledObjectLayer), "objectgroup")]
public abstract record class TiledLayer
{
    [JsonPropertyName("name")] public string? Name { get; init; }
    [JsonPropertyName("id")] public int Id { get; init; }
    [JsonPropertyName("x")] public int X { get; init; }
    [JsonPropertyName("y")] public int Y { get; init; }
    [JsonPropertyName("opacity")] public float Opacity { get; init; }

}

public record class TiledTileLayer : TiledLayer
{
    [JsonPropertyName("data")] public long[]? Data { get; init; }
    [JsonPropertyName("height")] public int Height { get; init; }
    [JsonPropertyName("visible")] public bool Visible { get; init; }
    [JsonPropertyName("width")] public int Width { get; init; }
    
}

public record class TiledObjectLayer : TiledLayer
{
    [JsonPropertyName("draworder")] public string? DrawOrder { get; init; }
    [JsonPropertyName("objects")] public List<TiledObject>? Objects { get; init; } 
}

public record class TiledObject
{
    [JsonPropertyName("gid")] public long Gid { get; init; }
    [JsonPropertyName("height")] public float Height { get; init; }
    [JsonPropertyName("id")] public long Id { get; init; }
    [JsonPropertyName("name")] public string? Name { get; init; }
    [JsonPropertyName("opacity")] public float Opacity { get; init; }
    [JsonPropertyName("rotation")] public float Rotation { get; init; }
    [JsonPropertyName("type")] public string? Type { get; init; }
    [JsonPropertyName("visible")] public bool Visible { get; init; }
    [JsonPropertyName("width")] public float Width { get; init; }
    [JsonPropertyName("x")] public float X { get; init; }
    [JsonPropertyName("y")] public float Y { get; init; }
}

