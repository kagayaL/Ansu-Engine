using Core.Physics.Components;
using Core.GameData;
using Microsoft.Xna.Framework;

namespace Runtime;

internal class DrawData : IComparable<DrawData>
{
    public Sprite Sprite { get; set; }
    public int Layer { get; set;}
    public Transform Transformed { get; set; }
    public Rectangle Rectangle { get; set; }

    public int CompareTo(DrawData? other)
    {
        if (other == null) return 1;
        if (this == other) return 0;

        if (this.Layer > other.Layer) return 1;
        if (this.Layer == other.Layer) return 0;
        
        return -1;
    }
}

