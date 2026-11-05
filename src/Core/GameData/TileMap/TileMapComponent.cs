using Core.ECS;

namespace Core.GameData.TileMap;

public struct TileMapComponent : IComponent
{
    // Массив ID тайлов 
    public long[]? Grid;
    // Высота сетки в клетках
    public int Rows;
    // Ширина сетки в клетках
    public int Cols;
    // Размер клетки в пикселях
    public int TileSize;
    // Ссылка на TileSet
    public int TilesetsId;
    //уровень отрисовки
    public int Layer;

    public TileMapComponent(long[]? grid, int rows, int cols, int tileSize, int tilesetsId, int layer = 0)
    {
        Grid = grid;
        Rows = rows;
        Cols = cols;
        TileSize = tileSize;
        TilesetsId= tilesetsId;
        Layer = layer;
    }
}