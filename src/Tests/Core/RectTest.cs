using Xunit;

using Core.Math;

namespace Tests.Core;


public class RectTest
{
    [Fact]
    public void RectangleIntersectsAndOverlapTest()
    {
        Rect rect1 = new Rect(66, 66, 6, 6);
        Rect rect2 = new Rect(66 - 5, 66 - 5, 6, 6);
        // rect1 и rect2 должны пересекаться
        Assert.True(rect1.Intersects(rect2));

        Rect resRect = new Rect(66, 66, 1, 1);
        // пересечение rect1 и rect2 равно прямоугольнику с точкой (66,66) и размерами (1,1)
        Assert.Equal(rect1.Overlap(rect2), resRect);
    }

}

