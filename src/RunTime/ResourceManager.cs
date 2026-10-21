using Core.Physics.Components;
using Core.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;


namespace Runtime;
public class ResourceManager : IDisposable
{
    private Dictionary<string, int> _idByPath = [];
    private Dictionary<int, Texture2D> _textureById = [];
    private int _textureId = 1;

    //функция загружает структуру в словарь по пути и выдает ее индекс
    public int LoadTexture(string path, GraphicsDevice graphicDevice)
    {
        int id;
        //если такой путь уже был, то возвращаем старый id
        if (_idByPath.TryGetValue(path, out id))
        {
            return id;
        }

        Texture2D texture = Texture2D.FromFile(graphicDevice, path);

        _idByPath[path] = _textureId;
        _textureById[_textureId] = texture;

        return _textureId++;

    }
    //возвращает текстуру по индексу
    public Texture2D GetTexture(int id)
    {
        return _textureById[id];
    }

    public void Dispose() 
    {
        foreach (var item in _textureById.Values)
        {
            item.Dispose();
        }
        _textureById.Clear();
        _idByPath.Clear();
    }
}
