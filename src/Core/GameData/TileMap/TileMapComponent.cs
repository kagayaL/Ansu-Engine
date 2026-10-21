using Core.ECS;

namespace Core.GameData.TileMap;

public struct TileMapComponent : IComponent
{
    // Массив ID тайлов 
    public int[] Grid;
    // Высота сетки в клетках
    public int Rows;
    // Ширина сетки в клетках
    public int Cols;
    // Размер клетки в пикселях
    public int TileSize;
    // Ссылка на TileSet
    public string TileSetRef;

    public TileMapComponent(int[] grid, int rows, int cols, int tileSize, string tileSetRef)
    {
        Grid = grid;
        Rows = rows;
        Cols = cols;
        TileSize = tileSize;
        TileSetRef = tileSetRef;
    }
}