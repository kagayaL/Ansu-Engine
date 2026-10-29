using Core.GameData.TileMap;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Render.Graphics;
using Runtime.TIledParse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runtime;
public class MapBaker
{
    
    public static RenderTarget2D BakeLayer(MapData mapData, LayerData layer, GraphicsDevice device, Renderer renderer, ResourceManager resources)
    {
        RenderTarget2D target = new RenderTarget2D(
            device,
            width: mapData.Width * mapData.TileSize, 
            height: mapData.Height * mapData.TileSize,
            mipMap: false,
            preferredFormat: SurfaceFormat.Color,
            preferredDepthFormat: DepthFormat.None,
            preferredMultiSampleCount: 0,
            usage: RenderTargetUsage.PreserveContents
        );

        device.SetRenderTarget(target);

        renderer.Clear();

        renderer.Begin();

        long[]? grid = layer.Grid;

        int tileSize = mapData.TileSize;
        int gridLength = grid == null ? 0 : grid.Length;
        for (int i = 0; i < gridLength; i++)
        {
            Rectangle rectangle;
            Texture2D? texture = resources.GetTexture(mapData.TilesetsId, grid![i], out rectangle);

            if (texture == null) continue;

            int col = i % layer.Cols;
            int row = i / layer.Cols;
            Vector2 position = new Vector2(col * tileSize, row * tileSize);
            renderer.DrawSprite(texture, position, 0f, null, rectangle);
        }

        renderer.End();

        device.SetRenderTarget(null);
        return target;

    }
}

