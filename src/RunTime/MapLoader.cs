using Core.ECS;
using Core.GameData.TileMap;
using Core.Physics.Components;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Runtime;
public class MapLoader
{
    private static int _tilesetId = 0;
    public static void LoadMap(MapData mapData, World world, ResourceManager resource, Transform? transform = null)
    {
        Transform mapTransform = transform ?? Transform.Default;

        _tilesetId = resource.LoadTileset(mapData.TilesetsData);
        int depth = 0;

        foreach (var layer in mapData.LayersData ?? new()) 
        {
            //объекты пока скипаю
            switch (layer.Type)
            {
                case LayerType.Object:
                    continue;
                case LayerType.Tile:
                    LoadTileLayer(world, layer, mapData, depth++, mapTransform);
                    break;
                default:
                    break;
            }
                
            
            
        }
    }

    private static void LoadTileLayer(World world, LayerData tileLayer, MapData mapData, int depth, Transform mapTransform)
    {

        TileMapComponent tileMapComponent = new TileMapComponent
        {
            Grid = tileLayer.Grid,
            Cols = tileLayer.Cols,
            Rows = tileLayer.Rows,
            TileSize = mapData.TileSize,
            TilesetsId = _tilesetId,
            Layer = depth
        };

        Entity layerEntity = world.CreateEntity();

        world.AddComponent(layerEntity, tileMapComponent);
        world.AddComponent(layerEntity, mapTransform);

    }
}

