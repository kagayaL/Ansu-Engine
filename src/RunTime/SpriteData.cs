using Core.Physics.Components;
using Core.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.GameData.TileMap;

namespace Runtime;

public enum DrawType
{
    Sprite,
    Tilelayer
}
internal class DrawData : IComparable<DrawData>
{
    public Sprite? Sprite { get; set; }
    public TileMapComponent? TileMapComponent {get; set;}
    public DrawType DrawType { get; set; }
    public int Layer { get; set;}
    public Position Position { get; set; }

    public int CompareTo(DrawData? other)
    {
        if (other == null) return 1;
        if (this == other) return 0;

        if (this.Layer > other.Layer) return 1;
        if (this.Layer == other.Layer) return -1;
        
        return 0;
    }
}

