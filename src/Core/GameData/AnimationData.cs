using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.ECS;
using Core.Math;

namespace Core.GameData;

public struct AnimationData : IComponent
{
    // Название анимации 
    public string CurrentClip;

    // Кадры анимации
    public Rect[] Frames;
    public float Fps;

    public AnimationData(string currentClip, Rect[] frames, float fps = 10f)
    {
        CurrentClip = currentClip;
        Frames = frames;
        Fps = fps;
    }
}