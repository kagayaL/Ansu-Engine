using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Physics.Components;

using Core.Math;

public struct Velocity
{
    public Vector2 Direction {  get; set; }

    public Velocity(Vector2 vector)
    {
        Direction = vector;
    }
    public Velocity(float x, float y)
    {
        Direction = new Vector2(x, y);
    }


}
