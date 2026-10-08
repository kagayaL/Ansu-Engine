using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Runtime;

//инфа об уровне
public record class MapData
{
    public int Height { get; init; }
    public int Width { get; init; }
    public int TileSize { get; init; }
    public List<LayerData>? LayersData { get; init; }
    public List<TilesetData>? TilesetsData { get; init; }

}

public enum LayerType
{
    TILE,
    OBJECT
}

public record class LayerData
{
    public LayerType Type { get; init; }
    public string? Name { get; init; }
    public bool? Visible { get; init; }

    //Свойства для TILE:
    public long[]? Grid { get; init; }
    public int Cols { get; init; }
    public int Rows { get; init; }

    //Свойства для OBJECT:
    public string? DrawOrder { get; init; }
    public List<ObjectData>? Objects { get; init; }
}

public record class TilesetData
{
    public int FirstGid { get; init; }
    public string? Source { get; init; }
}
public record class ObjectData
{
    public string? Name { get; init; }
    public long? Gid { get; init; }
    public float Height { get; init; }
    public float Width { get; init; }
    public float Rotation { get; init; }
    public float X { get; init; }
    public float Y { get; init; }
    public bool Visible { get; init; }
    public float Opacity { get; init; }

}