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
    public string TextureId;

    // Область вырезки на текстуре. null, если рисуется текстура целиком.
    public Rect? SourceRect;


    // Слой, на котором должен быть отрисован спрайт.
    public Layer Layer;

    public Sprite(string textureId, Layer layer = Layer.World, Rect? sourceRect = null)
    {
        TextureId = textureId;
        Layer = layer;
        SourceRect = sourceRect;
    }
}