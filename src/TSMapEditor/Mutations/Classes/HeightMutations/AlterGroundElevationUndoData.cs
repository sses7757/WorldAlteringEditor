using TSMapEditor.GameMath;

namespace TSMapEditor.Mutations.Classes.HeightMutations
{
    /// <summary>
    /// Struct for the undo data of mutations based on this class.
    /// </summary>
    public struct AlterGroundElevationUndoData(Point2D cellCoords, int tileIndex, int subTileIndex, int heightLevel)
    {
        public Point2D CellCoords = cellCoords;
        public int TileIndex = tileIndex;
        public int SubTileIndex = subTileIndex;
        public int HeightLevel = heightLevel;
    }
}
