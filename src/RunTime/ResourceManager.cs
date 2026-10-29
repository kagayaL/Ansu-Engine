using Core.GameData;
using Core.Physics.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Runtime;
public class ResourceManager : IDisposable
{
    private GraphicsDevice _graphicDevice;
    private Dictionary<string, int> _idByPath = [];
    private Dictionary<int, Texture2D> _textureById = [];
    private Dictionary<int, TilesetRegistry> _tilesetsById = [];
    private int _textureId = 1;
    private int _tilesetId = 1;

    public ResourceManager(GraphicsDevice graphicDevice)
    {
        _graphicDevice = graphicDevice;
    }

    //функция загружает структуру в словарь по пути и выдает ее индекс
    public int LoadTexture(string path)
    {
        int id;
        //если такой путь уже был, то возвращаем старый id
        if (_idByPath.TryGetValue(path, out id))
        {
            return id;
        }

        Texture2D texture = Texture2D.FromFile(_graphicDevice, path);

        _idByPath[path] = _textureId;
        _textureById[_textureId] = texture;

        return _textureId++;

    }
    //если передаем просто текстуру, то проверки не нужны
    public int LoadTexture(Texture2D texture)
    {
        _textureById[_textureId] = texture;
        return _textureId++;
    }
    //добавляет в словарь _tilesetById пару вида id - TilesetRegistry
    public int LoadTileset(List<TilesetData>? tilesetData)
    {
        TilesetRegistry tilesetRegistry = new TilesetRegistry(_graphicDevice);
        tilesetRegistry.LoadTextures(tilesetData);

        _tilesetsById[_tilesetId] = tilesetRegistry;

        return _tilesetId++;
    }

    public void UnloadTileset(int tilesetId)
    {
        if (_tilesetsById.Remove(tilesetId, out var registry))
            registry.Dispose();
    }

    public Texture2D? GetTexture(int tilesetId, long gid, out Rectangle rectangle)
    {
        //проверка есть ли текстура
        if (_tilesetsById.TryGetValue(tilesetId, out TilesetRegistry? registry))
        {
            return registry.GetTexture(gid, out rectangle);
        }
        rectangle = Rectangle.Empty;
        return null;
    }

    //возвращает текстуру по индексу
    public Texture2D? GetTexture(int textureId)
    {
        //проверка есть ли текстура
        if (_textureById.TryGetValue(textureId, out Texture2D? texture))
        {
            return texture;
        }
        return null;
    }

    public void Dispose()
    {
        foreach (var tex in _textureById.Values)
        {
            tex.Dispose();
        }

        _textureById.Clear();
        _idByPath.Clear();

        foreach (var registry in _tilesetsById.Values)
        {
            registry.Dispose();
        }

        _tilesetsById.Clear();

    }
    public void DisposeTilesets()
    {
        foreach (var registry in _tilesetsById.Values)
        {
            registry.Dispose();
        }

        _tilesetsById.Clear();
    }
    
}


public class TilesetRegistry : IDisposable
{
    private readonly Dictionary<int, Texture2D> _textureById = [];
    private readonly Dictionary<int, TilesetData> _tilesetById = [];

    private GraphicsDevice _graphicsDevice;

    public TilesetRegistry(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;
    }

    public void LoadTextures(List<TilesetData>? tilesetDatas)
    {
        foreach (var tileset in tilesetDatas ?? new())
        {
            if (string.IsNullOrEmpty(tileset.ImageSource)) continue;
            _textureById[tileset.FirstGid] = Texture2D.FromFile(_graphicsDevice, tileset.ImageSource);
            _tilesetById[tileset.FirstGid] = tileset;
        }
    }

    public Texture2D? GetTexture(long gid, out Rectangle rectangle)
    {
        //тайл с gid = 0 - пустой тайл
        if (gid == 0)
        {
            rectangle = Rectangle.Empty;
            return null;
        }

        foreach (var tileset in _tilesetById.Values)
        {

            if (gid >= tileset.FirstGid && gid <= tileset.LastGid)
            {
                int localId = (int)gid - tileset.FirstGid;
                int col = localId % tileset.Columns;
                int row = localId / tileset.Columns;
                int x = tileset.Margin + col * (tileset.TileWidth + tileset.Spacing);
                int y = tileset.Margin + row * (tileset.TileHeight + tileset.Spacing);

                //проверка на существует ли в словаре нужная текстура, чтобы программа не упала
                if (!_textureById.TryGetValue(tileset.FirstGid, out var texture))
                    continue;

                rectangle = new Rectangle(x, y, tileset.TileWidth, tileset.TileHeight);
                return texture;
            }
        }   
        rectangle = Rectangle.Empty;
        return null;
    }
    

    public void Dispose()
    {
        foreach (var item in _textureById.Values)
        {
            item.Dispose();
        }
        _textureById.Clear();
        _tilesetById.Clear();
    }
}