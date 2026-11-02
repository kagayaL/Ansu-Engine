using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Runtime;
public class Culling
{
    public static Rectangle CalculateAABB(Vector2 size, Vector2 position,
        float rotation, Vector2 origin, Vector2 scales)
    {
        Vector2 actualSize = new Vector2(size.X * scales.X, size.Y * scales.Y);
        Vector2 actualOrigin = new Vector2(origin.X * actualSize.X, origin.Y * actualSize.Y);
        //4 точки углов
        Vector2 leftTop = new Vector2(position.X - actualOrigin.X, position.Y - actualOrigin.Y);
        Vector2 rightTop = new Vector2(leftTop.X + actualSize.X, leftTop.Y);
        Vector2 leftBottom = new Vector2(leftTop.X, leftTop.Y + actualSize.Y);
        Vector2 rightBottom = leftTop + actualSize;

        Vector2[] corners = { leftTop, rightTop, leftBottom, rightBottom };

        var sin = (float)Math.Sin(rotation);
        var cos = (float)Math.Cos(rotation);

        int minX = int.MaxValue;
        int minY = int.MaxValue;
        int maxX = int.MinValue;
        int maxY = int.MinValue;

        foreach (Vector2 corner in corners)
        {
            //точка относительно origin
            Vector2 localCorner = corner - position;

            float rotateX = localCorner.X * cos - localCorner.Y * sin;
            float rotateY = localCorner.X * sin + localCorner.Y * cos;

            Vector2 rotateCorner = new Vector2(rotateX, rotateY) + position;

            minX = Math.Min(minX, (int)rotateCorner.X);
            minY = Math.Min(minY, (int)rotateCorner.Y);
            maxX = Math.Max(maxX, (int)rotateCorner.X);
            maxY = Math.Max(maxY, (int)rotateCorner.Y);
        }

        return new Rectangle(minX, minY, maxX - minX, maxY - minY);
    }

    public static Rectangle CalculateAABB(Core.Math.Vector2 size, Core.Math.Vector2 position,
        float rotation, Core.Math.Vector2 origin, Core.Math.Vector2 scales)
    {
        Vector2 mSize = CoreToMonogame.ConvertVector2(size);
        Vector2 mPosition = CoreToMonogame.ConvertVector2(position);
        Vector2 mOrigin = CoreToMonogame.ConvertVector2(origin);
        Vector2 mScales = CoreToMonogame.ConvertVector2(scales);

        return CalculateAABB(mSize, mPosition, rotation, mOrigin, mScales);

    }

}
