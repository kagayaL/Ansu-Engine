using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Math;

namespace Core.Physics.Components;

public struct Position
{
    public Vector2 Coordinates { get; set; }
    public Position(Vector2 vector)
    {
        Coordinates = vector;
    }
    public Position(float x, float y)
    {
        Coordinates = new Vector2(x, y);
    }
}
