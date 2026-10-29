using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;

namespace Core.GameData.TileMap;

public enum TileProperty
{
    Decor,      // Без коллизии
    Solid,      // Твердый блок 
    OneWay      // Односторонняя платформа 
}

public class TileSet
{
    // Название тайлсета
    public string Name { get; set; } = string.Empty;
    // ID текстуры тайлсета
    public string TextureId { get; set; } = string.Empty;
    // Размер тайла в пикселях (ширина и высота)
    public int TileSize { get; set; } = 16;

    // Словарь, сопоставляющий ID тайла с его свойствами
    public Dictionary<int, TileProperty> Properties { get; set; } = new();
}