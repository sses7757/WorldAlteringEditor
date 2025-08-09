using TSMapEditor.GameMath;

namespace TSMapEditor.Mutations.Classes
{
    struct OriginalCellTerrainData(Point2D cellCoords, int tileIndex, byte subTileIndex, byte heightLevel)
    {
        public Point2D CellCoords = cellCoords;
        public int TileIndex = tileIndex;
        public byte SubTileIndex = subTileIndex;
        public byte HeightLevel = heightLevel;
    }
}
