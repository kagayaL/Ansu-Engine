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
using Microsoft.Xna.Framework;

namespace Runtime;
public class LevelLoader
{
    //глубина слоя
    private static int depth = 0;
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
                    LoadTileLayer(mapData, layer, world, device, renderer, resources);                    
                    break;
                case LayerType.Object:
                    LoadObjectLayer(mapData, layer, world, device, renderer, resources);
                    break;
            }
            depth++;
        }
        depth = 0;

    }

    public static void UnloadLevel(MapData mapData, ResourceManager resources)
    {
        resources.UnloadTileset(mapData.TilesetsId);
    }

    private static void LoadTileLayer(MapData mapData, LayerData layer, World world,
        GraphicsDevice device, Renderer renderer, ResourceManager resources )
    {
        RenderTarget2D bakedLayer = MapBaker.BakeLayer(mapData, layer, device, renderer, resources);

        Entity layerEntity = world.CreateEntity();
        Sprite sprite = new Sprite(resources.LoadTexture(bakedLayer), depth);
        NameComponent name = new NameComponent(layer.Name ?? "");
        world.AddComponent(layerEntity, sprite);
        world.AddComponent(layerEntity, Transform.Default);
        world.AddComponent(layerEntity, name);
    }

    private static void LoadObjectLayer(MapData mapData, LayerData layer, World world,
        GraphicsDevice device, Renderer renderer, ResourceManager resources)
    {

        foreach (var obj in layer.Objects ?? []) {

            if (obj == null || obj.Gid == null) continue;

            long gid = obj.Gid ?? 0;
            Texture2D? spriteTexture = resources.GetTexture(mapData.TilesetsId,
                gid & GidResolver.GID_MASK, out var rect);

            if (spriteTexture is null) continue;

            Entity entity = world.CreateEntity();
            Sprite sprite = new Sprite(resources.LoadTexture(spriteTexture), depth,
                MonogameToCore.ConvertRectangle(rect));


            Core.Math.Vector2 scale = new Core.Math.Vector2(obj.ScaleX, obj.ScaleY);
            Core.Math.Vector2 origin = new Core.Math.Vector2(0, 1);

            float rotate = MathHelper.ToRadians(obj.Rotation);           
            Transform transform = new Transform(obj.X, obj.Y, rotate, origin, scale);

            world.AddComponent(entity, sprite);
            world.AddComponent(entity, transform);
        }
    }

}

