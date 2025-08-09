using TSMapEditor.GameMath;

namespace TSMapEditor.Mutations.Classes
{
    /// <summary>
    /// Struct for un-do data of mutations that change smudges of cells.
    /// </summary>
    struct OriginalSmudgeInfo(int smudgeTypeIndex, Point2D cellCoords)
    {
        public int SmudgeTypeIndex = smudgeTypeIndex;
        public Point2D CellCoords = cellCoords;
    }
}
