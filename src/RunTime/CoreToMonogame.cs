using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runtime;
public class CoreToMonogame
{
    //преобразует прямоугольник из Core в прямоугольник Monogame
    public static Rectangle? ConvertRectangle(Core.Math.Rect? rectangleFromCore)
    {
        if (rectangleFromCore == null) return null;

        Rectangle rectangle = new Rectangle();
        rectangle.X = (int)rectangleFromCore.Value.X;
        rectangle.Y = (int)rectangleFromCore.Value.Y;
        rectangle.Width = (int)rectangleFromCore.Value.Width;
        rectangle.Height = (int)rectangleFromCore.Value.Height;
        return rectangle;
    }

    //преобразует Vector2 из Core в из Monogame
    public static Vector2 ConvertVector2(Core.Math.Vector2 vectorFromCore)
    {
        return new(vectorFromCore.X, vectorFromCore.Y);
    }
}

public class MonogameToCore
{
    //преобразует прямоугольник из Monogame в прямоугольник из Core
    public static Core.Math.Rect? ConvertRectangle(Rectangle? rectangleFromMonogame)
    {
        if (rectangleFromMonogame == null) return null;
        
        float x = rectangleFromMonogame.Value.X;
        float y = rectangleFromMonogame.Value.Y;
        float width = (float)rectangleFromMonogame.Value.Width;
        float height = (float)rectangleFromMonogame.Value.Height;

        Core.Math.Rect rectangle = new Core.Math.Rect(x, y, width, height);
        
        return rectangle;
    }

    //преобразует Vector2 из Monogame в Core
    public static Vector2 ConvertVector2(Core.Math.Vector2 vectorFromMonogame)
    {
        return new(vectorFromMonogame.X, vectorFromMonogame.Y);
    }
}