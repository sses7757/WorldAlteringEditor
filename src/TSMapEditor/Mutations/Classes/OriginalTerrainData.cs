using TSMapEditor.GameMath;

namespace TSMapEditor.Mutations.Classes
{
    public struct OriginalTerrainData(int tileIndex, byte subTileIndex, byte level, Point2D cellCoords)
    {
        public int TileIndex = tileIndex;
        public byte SubTileIndex = subTileIndex;
        public byte Level = level;
        public Point2D CellCoords = cellCoords;
    }
}
