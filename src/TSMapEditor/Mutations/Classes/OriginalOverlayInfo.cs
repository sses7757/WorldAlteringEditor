using TSMapEditor.GameMath;

namespace TSMapEditor.Mutations.Classes
{
    /// <summary>
    /// Struct for un-do data of mutations that change overlay of cells.
    /// </summary>
    struct OriginalOverlayInfo(int overlayTypeIndex, int frameIndex, Point2D cellCoords)
    {
        public int OverlayTypeIndex = overlayTypeIndex;
        public int FrameIndex = frameIndex;
        public Point2D CellCoords = cellCoords;
    }
}
