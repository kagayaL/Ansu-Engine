using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runtime;

public sealed class TilesetTextures
{
    private static int tileSize;
    private static GraphicsDevice? _device;

    private static Texture2D GetTexture(long gid, List<TilesetData> tilesetData)
    {
        Texture2D texture = new Texture2D(_device, tileSize, tileSize);
        return texture;
    }

    public static IReadOnlyDictionary<long, Texture2D> GetTilesetTextures(GraphicsDevice device, MapData data)
    {
        tileSize = data.TileSize;
        Dictionary<long, Texture2D> gidTexturePairs = new Dictionary<long, Texture2D>();
        _device = device;

        foreach (var layer in data.LayersData ?? new())
        {
            if (layer.Type == LayerType.Object) continue;

            long[]? grid = layer.Grid;
            if (grid == null) continue;

            for (int i = 0; i < grid.Length; i++)
            {
                long gid = grid[i];
                gidTexturePairs[gid] = GetTexture(gid, data.TilesetsData);
            }
        }
        return gidTexturePairs;
    }
}
