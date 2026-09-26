using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Runtime;

//инфа об уровне
public record class LayerData
{
    /* эта залупа нужна потому что
    кто то решил что стандарт у имен свойств с большой буквы, 
    а стадарт у json с маленькой при этом json серилайзер
    именно свойства считывает
    */
    [JsonPropertyName("grid")]
    public required int[] Grid { get; init; }
    [JsonPropertyName("rows")]
    public int Rows { get; init; }
    [JsonPropertyName("cols")]
    public int Cols { get; init; }
    [JsonPropertyName("tile_size")]
    public int TileSize { get; init; }
    [JsonPropertyName("objects")]
    public required List<LayerObject> Objects { get; init; }

    //подумаю еще как проверку сделать надо будет почитать
    public static void Validate(LayerData layerData)
    {
    }

}

public record class LayerObject
{
    [JsonPropertyName("type")]
    public int Type { get; init; }
    [JsonPropertyName("x")]
    public float X { get; init; }
    [JsonPropertyName("y")]
    public float Y { get; init; }
    [JsonPropertyName("props")]
    public required Dictionary<string, string> Properties { get; init; }
}

