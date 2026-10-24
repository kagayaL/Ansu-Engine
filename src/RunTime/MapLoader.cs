using Core.ECS;
using Core.GameData.TileMap;
using Core.Physics.Components;
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
    public static void LoadMap(MapData mapData, World world, ResourceManager resource)
    {
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
                    LoadTileLayer(world, layer, mapData, depth++);
                    break;
                default:
                    break;
            }
                
            
            
        }
    }

    private static void LoadTileLayer(World world, LayerData tileLayer, MapData mapData, int depth)
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

        Position zeroPosition = new Position(0, 0);
        Entity layerEntity = world.CreateEntity();

        world.AddComponent(layerEntity, tileMapComponent);
        world.AddComponent(layerEntity, zeroPosition);

    }
}

