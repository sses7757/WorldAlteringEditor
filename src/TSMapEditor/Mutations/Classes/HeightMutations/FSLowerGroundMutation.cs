using System;
using System.Collections.Generic;
using System.Linq;
using TSMapEditor.CCEngine;
using TSMapEditor.GameMath;
using TSMapEditor.Models;
using TSMapEditor.UI;
using HCT = TSMapEditor.Mutations.Classes.HeightMutations.HeightComparisonType;

namespace TSMapEditor.Mutations.Classes.HeightMutations
{
    public class FSLowerGroundMutation(IMutationTarget mutationTarget, Point2D originCell, BrushSize brushSize) : LowerGroundMutationBase(mutationTarget, originCell, brushSize)
    {
        private static readonly TransitionRampInfo[] transitionRampInfos =
        [
            new TransitionRampInfo(RampType.None, [HCT.Equal, HCT.Equal, HCT.Equal, HCT.Equal, HCT.Equal, HCT.Equal, HCT.Equal, HCT.Equal]),

            new TransitionRampInfo(RampType.West, [HCT.LowerOrEqual, HCT.HigherOrEqual, HCT.Higher, HCT.HigherOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual] ),
            new TransitionRampInfo(RampType.North, [HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.HigherOrEqual, HCT.Higher, HCT.HigherOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual]),
            new TransitionRampInfo(RampType.East, [HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.HigherOrEqual, HCT.Higher, HCT.HigherOrEqual]),
            new TransitionRampInfo(RampType.South, [HCT.Higher, HCT.HigherOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.HigherOrEqual]),

            new TransitionRampInfo(RampType.CornerNW, [HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.Equal, HCT.Higher, HCT.Equal, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual]),
            new TransitionRampInfo(RampType.CornerNE, [HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.Equal, HCT.Higher, HCT.Equal, HCT.LowerOrEqual]),
            new TransitionRampInfo(RampType.CornerSE, [HCT.Equal, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.Equal, HCT.Higher]),
            new TransitionRampInfo(RampType.CornerSW, [HCT.Equal, HCT.Higher, HCT.Equal, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual]),

            new TransitionRampInfo(RampType.MidNW, [HCT.LowerOrEqual, HCT.HigherOrEqual, HCT.HigherOrEqual, HCT.Higher, HCT.HigherOrEqual, HCT.HigherOrEqual, HCT.Equal, HCT.LowerOrEqual]),
            new TransitionRampInfo(RampType.MidNE, [HCT.Equal, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.HigherOrEqual, HCT.HigherOrEqual, HCT.Higher, HCT.HigherOrEqual, HCT.HigherOrEqual]),
            new TransitionRampInfo(RampType.MidSE, [HCT.HigherOrEqual, HCT.HigherOrEqual, HCT.Equal, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.HigherOrEqual, HCT.HigherOrEqual, HCT.Higher]),
            new TransitionRampInfo(RampType.MidSW, [HCT.HigherOrEqual, HCT.Higher, HCT.HigherOrEqual, HCT.HigherOrEqual, HCT.Equal, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.HigherOrEqual]),

            new TransitionRampInfo(RampType.DoubleUpSWNE, [HCT.Equal, HCT.Higher, HCT.Equal, HCT.Irrelevant, HCT.Equal, HCT.Higher, HCT.Equal, HCT.Irrelevant]),
            new TransitionRampInfo(RampType.DoubleDownSWNE, [HCT.Equal, HCT.Irrelevant, HCT.Equal, HCT.Higher, HCT.Equal, HCT.Irrelevant, HCT.Equal, HCT.Higher]),

            // Fixes for ramps in "odd angles" between cells

            new TransitionRampInfo(RampType.MidNE, [HCT.Equal, HCT.LowerOrEqual, HCT.Equal, HCT.Higher, HCT.Equal, HCT.Irrelevant, HCT.Higher, HCT.Irrelevant]),
            new TransitionRampInfo(RampType.MidSW, [HCT.Equal, HCT.Equal, HCT.Higher, HCT.Irrelevant, HCT.Equal, HCT.Equal, HCT.Equal, HCT.Higher]),

            new TransitionRampInfo(RampType.MidNW, [HCT.Equal, HCT.Higher, HCT.Equal, HCT.Equal, HCT.Higher, HCT.Equal, HCT.Equal, HCT.LowerOrEqual]),
            new TransitionRampInfo(RampType.MidSE, [HCT.Higher, HCT.Equal, HCT.Equal, HCT.LowerOrEqual, HCT.Equal, HCT.Higher, HCT.Equal, HCT.Equal]),

            new TransitionRampInfo(RampType.MidSE, [HCT.Equal, HCT.Higher, HCT.Equal, HCT.LowerOrEqual, HCT.Equal, HCT.Irrelevant, HCT.Higher, HCT.Equal]),
            new TransitionRampInfo(RampType.MidNW, [HCT.Equal, HCT.Irrelevant, HCT.Higher, HCT.Equal, HCT.Equal, HCT.Higher, HCT.Equal, HCT.LowerOrEqual]),

            new TransitionRampInfo(RampType.MidNE, [HCT.Equal, HCT.LowerOrEqual, HCT.Equal, HCT.Equal, HCT.Higher, HCT.Equal, HCT.Equal, HCT.Higher]),
            new TransitionRampInfo(RampType.MidSW, [HCT.Higher, HCT.Equal, HCT.Equal, HCT.Higher, HCT.Equal, HCT.Equal, HCT.LowerOrEqual, HCT.Equal]),

            // "Less likely" cases of mid-ramps, where the cell direclty behind the ramp is not higher but the cells on the "backsides" are
            new TransitionRampInfo(RampType.MidNW, [HCT.LowerOrEqual, HCT.HigherOrEqual, HCT.HigherOrEqual, HCT.HigherOrEqual, HCT.Higher, HCT.HigherOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual]),
            new TransitionRampInfo(RampType.MidNW, [HCT.LowerOrEqual, HCT.HigherOrEqual, HCT.Higher, HCT.HigherOrEqual, HCT.HigherOrEqual, HCT.HigherOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual]),
            new TransitionRampInfo(RampType.MidNE, [HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.HigherOrEqual, HCT.HigherOrEqual, HCT.HigherOrEqual, HCT.Higher, HCT.HigherOrEqual]),
            new TransitionRampInfo(RampType.MidNE, [HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.HigherOrEqual, HCT.Higher, HCT.HigherOrEqual, HCT.HigherOrEqual, HCT.HigherOrEqual]),
            new TransitionRampInfo(RampType.MidSE, [HCT.HigherOrEqual, HCT.HigherOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.HigherOrEqual, HCT.Higher, HCT.HigherOrEqual]),
            new TransitionRampInfo(RampType.MidSE, [HCT.Higher, HCT.HigherOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.HigherOrEqual, HCT.HigherOrEqual, HCT.HigherOrEqual]),
            new TransitionRampInfo(RampType.MidSW, [HCT.HigherOrEqual, HCT.HigherOrEqual, HCT.Higher, HCT.HigherOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.HigherOrEqual]),
            new TransitionRampInfo(RampType.MidSW, [HCT.Higher, HCT.HigherOrEqual, HCT.HigherOrEqual, HCT.HigherOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.HigherOrEqual]),

            // In case it's anything else, we probably need to flatten it
            new TransitionRampInfo(RampType.None, [HCT.Irrelevant, HCT.Irrelevant, HCT.Irrelevant, HCT.Irrelevant, HCT.Irrelevant, HCT.Irrelevant, HCT.Irrelevant, HCT.Irrelevant], 0),
        ];

        // Pre-ramp-placement height fix checks
        private static readonly TransitionRampInfo[] heightFixers =
        [
            new TransitionRampInfo(RampType.None, [HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Equal, HCT.Equal, HCT.Equal, HCT.Equal], 1),
            new TransitionRampInfo(RampType.None, [HCT.Equal, HCT.Equal, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Equal, HCT.Equal, HCT.Higher], 1),
            new TransitionRampInfo(RampType.None, [HCT.Higher, HCT.Higher, HCT.Higher, HCT.Equal, HCT.Equal, HCT.Higher, HCT.Equal, HCT.Equal], 1),
            new TransitionRampInfo(RampType.None, [HCT.Higher, HCT.Equal, HCT.Equal, HCT.Higher, HCT.Equal, HCT.Equal, HCT.Higher, HCT.Higher], 1),
            new TransitionRampInfo(RampType.None, [HCT.Equal, HCT.Higher, HCT.Equal, HCT.Equal, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Equal], 1),
            new TransitionRampInfo(RampType.None, [HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Equal, HCT.Equal], 1),
            new TransitionRampInfo(RampType.None, [HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Equal, HCT.Equal, HCT.Higher], 1),
            new TransitionRampInfo(RampType.None, [HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Equal, HCT.Equal, HCT.Higher, HCT.Higher], 1),
            new TransitionRampInfo(RampType.None, [HCT.Higher, HCT.Higher, HCT.Higher, HCT.Equal, HCT.Equal, HCT.Higher, HCT.Higher, HCT.Higher], 1),
            new TransitionRampInfo(RampType.None, [HCT.Higher, HCT.Higher, HCT.Equal, HCT.Equal, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher], 1),
            new TransitionRampInfo(RampType.None, [HCT.Higher, HCT.Equal, HCT.Equal, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher], 1),
            new TransitionRampInfo(RampType.None, [HCT.Equal, HCT.Equal, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher], 1),
            new TransitionRampInfo(RampType.None, [HCT.Equal, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Equal], 1),
            new TransitionRampInfo(RampType.None, [HCT.Equal, HCT.HigherOrEqual, HCT.Higher, HCT.Equal, HCT.Higher, HCT.Equal, HCT.HigherOrEqual, HCT.Higher], 1),
            new TransitionRampInfo(RampType.None, [HCT.Higher, HCT.Equal, HCT.Higher, HCT.Equal, HCT.HigherOrEqual, HCT.Higher, HCT.Equal, HCT.HigherOrEqual], 1),
            new TransitionRampInfo(RampType.None, [HCT.HigherOrEqual, HCT.Higher, HCT.Equal, HCT.HigherOrEqual, HCT.Higher, HCT.Equal, HCT.Higher, HCT.Equal], 1),
            new TransitionRampInfo(RampType.None, [HCT.Higher, HCT.Equal, HCT.HigherOrEqual, HCT.Higher, HCT.Equal, HCT.HigherOrEqual, HCT.Higher, HCT.Equal], 1),
            new TransitionRampInfo(RampType.None, [HCT.Irrelevant, HCT.Irrelevant, HCT.Higher, HCT.Irrelevant, HCT.Irrelevant, HCT.Higher, HCT.Irrelevant, HCT.Higher], 1),

            new TransitionRampInfo(RampType.None, [HCT.Higher, HCT.Irrelevant, HCT.Irrelevant, HCT.Irrelevant, HCT.Higher, HCT.Irrelevant, HCT.Irrelevant, HCT.Irrelevant], 1),
            new TransitionRampInfo(RampType.None, [HCT.Irrelevant, HCT.Irrelevant, HCT.Higher, HCT.Irrelevant, HCT.Higher, HCT.Irrelevant, HCT.Higher, HCT.Irrelevant], 1),
        ];

        protected override TransitionRampInfo[] GetTransitionRampInfos() => transitionRampInfos;

        protected override TransitionRampInfo[] GetHeightFixers() => heightFixers;

        public override string GetDisplayString()
        {
            return $"Lower ground at {OriginCell} with a brush size of {BrushSize} using non-steep ramps";
        }

        public override void Perform() => LowerGround();


        protected override void CheckCell(Point2D cellCoords)
        {
            if (processedCellsThisIteration.Contains(cellCoords) || cellsToProcess.Contains(cellCoords))
                return;

            MarkCellAsProcessed(cellCoords);

            var thisCell = Map.GetTile(cellCoords);
            if (thisCell == null)
                return;

            if (!IsCellMorphable(thisCell))
                return;

            int biggestHeightDiff = 0;

            for (int direction = 0; direction < (int)Direction.Count; direction++)
            {
                var cell = Map.GetTile(cellCoords + Helpers.VisualDirectionToPoint((Direction)direction));
                if (cell == null)
                    continue;

                if (!IsCellMorphable(cell))
                    continue;

                if (cell.Level < thisCell.Level)
                    biggestHeightDiff = Math.Max(biggestHeightDiff, thisCell.Level - cell.Level);
            }

            // If nearby cells are lower by more than 1 cell, it's necessary to also lower this cell
            if (biggestHeightDiff > 1)
            {
                AddCellToUndoData(thisCell.CoordsToPoint());

                if (thisCell.Level > 0)
                {
                    if (thisCell.Level >= biggestHeightDiff - 1)
                        thisCell.Level = (byte)(thisCell.Level - (biggestHeightDiff - 1));
                    else
                        thisCell.Level--;
                }

                foreach (Point2D offset in SurroundingTiles)
                {
                    RegisterCell(cellCoords + offset);
                }
            }
        }

        /// <summary>
        /// Applies miscellaneous fixes to height data of processed cells.
        /// </summary>
        protected override void CellHeightFixes()
        {
            // We process one set of cells at a time, starting from the cells
            // processed so far.
            // During the processing, we might get new cells to process.
            // We repeat the process until no new cells to process have been added to the list.
            List<Point2D> cellsToCheck = new(totalProcessedCells);
            List<Point2D> newCells = [];

            while (cellsToCheck.Count > 0)
            {
                // Special height fixes
                totalProcessedCells.ForEach(cc =>
                {
                    var cell = Map.GetTile(cc);
                    if (cell == null)
                        return;

                    if (!IsCellMorphable(cell))
                        return;

                    var applyingTransition = Array.Find(heightFixers, tri => tri.Matches(Map, cc, cell.Level));
                    if (applyingTransition != null)
                    {
                        AddCellToUndoData(cell.CoordsToPoint());
                        cell.ChangeTileIndex(0, 0);
                        cell.Level = (byte)(cell.Level + applyingTransition.HeightChange);

                        // If we fixed this flaw from one cell, also check the surrounding ones
                        foreach (var surroundingCell in SurroundingTiles)
                        {
                            if (!newCells.Contains(cc + surroundingCell))
                                newCells.Add(cc + surroundingCell);
                        }
                    }
                });

                var cellsForNextRound = newCells.Where(c => !cellsToCheck.Contains(c));
                cellsToCheck.Clear();
                cellsToCheck.AddRange(cellsForNextRound);
                totalProcessedCells.AddRange(cellsForNextRound.Where(c => !totalProcessedCells.Contains(c)));
                newCells.Clear();
            }
        }
    }
}
