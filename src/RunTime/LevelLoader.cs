using Core.ECS;
using Microsoft.Xna.Framework.Graphics;
using Render.Graphics;
using Runtime.TIledParse;
using Core.GameData;
using Core.Physics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Runtime;
public class LevelLoader
{
    public static void LoadLevel(string tmjPath, World world, GraphicsDevice device,
        Renderer renderer, ResourceManager resources)
    {
        MapData mapData = TiledConverter.Convert(tmjPath);
        mapData.TilesetsId = resources.LoadTileset(mapData.TilesetsData);

        foreach (var layer in mapData.LayersData ?? [])
        {
            switch (layer.Type)
            {
                case LayerType.Tile:
                    LoadTilelayer(mapData, layer, world, device, renderer, resources);                    
                    break;
                case LayerType.Object:
                    //пока не обрабатываю объекты
                    break;
            }
        }
        resources.DisposeTilesets();

    }

    private static void LoadTilelayer(MapData mapData, LayerData layer, World world,
        GraphicsDevice device, Renderer renderer, ResourceManager resources )
    {
        RenderTarget2D bakedLayer = MapBaker.BakeLayer(mapData, layer, device, renderer, resources);

        Entity layerEntity = world.CreateEntity();
        Sprite sprite = new Sprite(resources.LoadTexture(bakedLayer));
        world.AddComponent(layerEntity, sprite);
        world.AddComponent(layerEntity, Transform.Default);
    }

}

