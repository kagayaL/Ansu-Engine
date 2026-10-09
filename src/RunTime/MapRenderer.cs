using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Render.Graphics;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Runtime.MapRenderer;

public sealed class MapRenderer
{
    
    //Рисует карту в пределах vieport
    public void DrawMap(Renderer renderer, GraphicsDevice device, MapData data
        /*IReadOnlyDictionary<int, Texture2D> textures, Rectangle vieport*/)
    {
        int tileSize = 32;
        LayerData firstlayer = data.LayersData[0];
        long[]? grid = firstlayer.Grid;
        string imagePath = data.TilesetsData[0].ImageSource;
        Texture2D texture = Texture2D.FromFile(device, imagePath);

        for (int i = 0; i < grid.Length; i++)
        {
            int col = i % firstlayer.Cols;
            int row = i / firstlayer.Cols;
            Vector2 pos = new Vector2(col * tileSize, row * tileSize);
            long gid = grid[i] - 1;
            Rectangle rec = new Rectangle( (tileSize * (int)gid) % 896, tileSize * ((int)gid / 28),
                tileSize, tileSize);
            renderer.DrawSprite(texture, pos, rec);
        }
    }
}
