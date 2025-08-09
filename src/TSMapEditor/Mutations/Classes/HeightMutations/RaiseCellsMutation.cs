using System.Collections.Generic;
using TSMapEditor.GameMath;
using TSMapEditor.Models;
using TSMapEditor.UI;

namespace TSMapEditor.Mutations.Classes.HeightMutations
{
    /// <summary>
    /// A mutation for increasing cell height levels.
    /// </summary>
    public class RaiseCellsMutation(IMutationTarget mutationTarget, Point2D targetCellCoords, BrushSize brushSize, bool applyOnArea) : Mutation(mutationTarget)
    {
        private Point2D targetCellCoords = targetCellCoords;
        private readonly BrushSize brushSize = brushSize;
        private readonly bool applyOnArea = applyOnArea;

        private readonly List<Point2D> affectedCells = [];

        public override string GetDisplayString()
        {
            return $"Raise cell height at {targetCellCoords} with a brush size of {brushSize}";
        }

        public override void Perform()
        {
            if (!applyOnArea)
            {
                brushSize.DoForBrushSize(offset =>
                {
                    Point2D cellCoords = targetCellCoords + offset;

                    var cell = MutationTarget.Map.GetTile(cellCoords);

                    RaiseCellLevel(cell);
                });
            }
            else
            {
                var targetTile = MutationTarget.Map.GetTile(targetCellCoords);
                var tilesToProcess = Helpers.GetFillAreaTiles(targetTile, MutationTarget.Map, MutationTarget.TheaterGraphics);

                // Process tiles
                foreach (Point2D cellCoords in tilesToProcess)
                {
                    var cell = MutationTarget.Map.GetTile(cellCoords);
                    RaiseCellLevel(cell);
                }
            }

            MutationTarget.AddRefreshPoint(targetCellCoords);
        }

        private void RaiseCellLevel(MapTile cell)
        {
            if (cell == null)
                return;

            if (cell.Level < Constants.MaxMapHeightLevel)
            {
                cell.Level++;
                cell.RefreshLighting(Map.Lighting, MutationTarget.LightingPreviewState);
                affectedCells.Add(cell.CoordsToPoint());
            }
        }

        public override void Undo()
        {
            foreach (Point2D cellCoords in affectedCells)
            {
                var cell = MutationTarget.Map.GetTile(cellCoords);
                if (cell == null)
                    return;

                if (cell.Level > 0)
                {
                    cell.Level--;
                    cell.RefreshLighting(Map.Lighting, MutationTarget.LightingPreviewState);
                }
            }

            MutationTarget.AddRefreshPoint(targetCellCoords);
        }
    }
}
