using Core.Physics.Components;
using Core.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runtime;
internal class SpriteData : IComparable<SpriteData>
{
    public Sprite Sprite { get; set; }
    public Position Position { get; set; }

    public int CompareTo(SpriteData? other)
    {
        if (other == null) return 1;
        if (this == other) return 0;

        if (this.Sprite.Layer > other.Sprite.Layer) return -1;
        if (this.Sprite.Layer == other.Sprite.Layer) return 1;
        return 0;
    }
}

