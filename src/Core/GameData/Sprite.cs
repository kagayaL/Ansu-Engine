using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.ECS;
using Core.Math;

namespace Core.GameData;

public struct Sprite : IComponent
{

    // ID текстуры
    public int TextureId;

    // Область вырезки на текстуре. null, если рисуется текстура целиком.
    public Rect? SourceRect;

    // Слой, на котором должен быть отрисован спрайт.
    public int Layer;

    public Sprite(int textureId, int layer = 0, Rect? sourceRect = null)
    {
        TextureId = textureId;
        Layer = layer;
        SourceRect = sourceRect;
    }
}