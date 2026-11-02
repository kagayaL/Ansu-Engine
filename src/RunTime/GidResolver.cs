using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Runtime;

internal static class GidResolver
{
    public const uint GID_MASK = 0x0FFFFFFF;

    // Биты флагов
    private const uint FLAG_BIT31 = 0x80000000;
    private const uint FLAG_BIT30 = 0x40000000;
    private const uint FLAG_BIT29 = 0x20000000;

    //выдает нужный спрайт эффект (горизонтальный и вертикальный флипы) и поворот
    public static SpriteEffects GetSpriteEffects(long rawGid, out float rotation)
    {
        uint raw = (uint)rawGid;

        int state = 0;
        if ((raw & FLAG_BIT31) != 0) state |= 0b100;
        if ((raw & FLAG_BIT30) != 0) state |= 0b010;
        if ((raw & FLAG_BIT29) != 0) state |= 0b001;

        rotation = 0f;
        SpriteEffects spriteEffects = SpriteEffects.None;

        //перебираю все значения
        switch (state)
        {
            case 0b000:   // дефолт
                rotation = 0f;
                break;

            case 0b011:   // 1 влево - 90
                rotation = -MathHelper.PiOver2;
                break;

            case 0b110:   // 2 влево - 180
                rotation = MathHelper.Pi;
                break;

            case 0b101:   // 3 влево - 270
                rotation = MathHelper.PiOver2;
                break;
            case 0b100:   // горизонтальный флип
                spriteEffects = SpriteEffects.FlipHorizontally;
                break;

            case 0b010:   // вертикальный флип
                spriteEffects = SpriteEffects.FlipVertically;
                break;
            case 0b111:   // 3 влево + вертикальный
                rotation = 3 * MathHelper.PiOver2;
                spriteEffects = SpriteEffects.FlipVertically;
                break;

            case 0b001:   // 3 влево + горизонтальный
                rotation = 3 * MathHelper.PiOver2;
                spriteEffects = SpriteEffects.FlipHorizontally;
                break;
        }

        return spriteEffects;
    }
}