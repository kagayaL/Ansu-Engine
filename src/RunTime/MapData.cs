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

}

public enum LayerType
{
    TILE,
    OBJECT
}

public record class LayerData
{
    public LayerType Type { get; init; }


    //Свойства для TILE:


    //Свойства для OBJECT:

}