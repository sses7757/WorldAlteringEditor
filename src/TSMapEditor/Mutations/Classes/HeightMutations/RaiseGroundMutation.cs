using System;
using TSMapEditor.CCEngine;
using TSMapEditor.GameMath;
using TSMapEditor.Models;
using TSMapEditor.UI;
using HCT = TSMapEditor.Mutations.Classes.HeightMutations.HeightComparisonType;

namespace TSMapEditor.Mutations.Classes.HeightMutations
{
    /// <summary>
    /// A mutation for "smartly" raising ground, with automatic application of ramps.
    /// </summary>
    public class RaiseGroundMutation(IMutationTarget mutationTarget, Point2D originCell, BrushSize brushSize) : RaiseGroundMutationBase(mutationTarget, originCell, brushSize)
    {
        private static readonly TransitionRampInfo[] transitionRampInfos =
        [
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

            new TransitionRampInfo(RampType.SteepSE, [HCT.LowerOrEqual, HCT.HigherOrEqual, HCT.Higher, HCT.MuchHigher, HCT.Higher, HCT.HigherOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual]),
            new TransitionRampInfo(RampType.SteepSW, [HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.HigherOrEqual, HCT.Higher, HCT.MuchHigher, HCT.Higher, HCT.HigherOrEqual]),
            new TransitionRampInfo(RampType.SteepNW, [HCT.Higher, HCT.HigherOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.HigherOrEqual, HCT.Higher, HCT.MuchHigher]),
            new TransitionRampInfo(RampType.SteepNE, [HCT.Higher, HCT.MuchHigher, HCT.Higher, HCT.HigherOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.LowerOrEqual, HCT.HigherOrEqual]),

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
        ];

        // Pre-ramp-placement height fix checks
        private static readonly TransitionRampInfo[] heightFixers =
        [
            new(RampType.None, [HCT.Equal, HCT.Equal, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Equal, HCT.Equal, HCT.Higher], 1),
            new(RampType.None, [HCT.Higher, HCT.Higher, HCT.Higher, HCT.Equal, HCT.Equal, HCT.Higher, HCT.Equal, HCT.Equal], 1),
            new(RampType.None, [HCT.Higher, HCT.Equal, HCT.Equal, HCT.Higher, HCT.Equal, HCT.Equal, HCT.Higher, HCT.Higher], 1),
            new(RampType.None, [HCT.Equal, HCT.Higher, HCT.Equal, HCT.Equal, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Equal], 1),
            new(RampType.None, [HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Equal, HCT.Equal], 1),
            new(RampType.None, [HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Equal, HCT.Equal, HCT.Higher], 1),
            new(RampType.None, [HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Equal, HCT.Equal, HCT.Higher, HCT.Higher], 1),
            new(RampType.None, [HCT.Higher, HCT.Higher, HCT.Higher, HCT.Equal, HCT.Equal, HCT.Higher, HCT.Higher, HCT.Higher], 1),
            new(RampType.None, [HCT.Higher, HCT.Higher, HCT.Equal, HCT.Equal, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher], 1),
            new(RampType.None, [HCT.Higher, HCT.Equal, HCT.Equal, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher], 1),
            new(RampType.None, [HCT.Equal, HCT.Equal, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher], 1),
            new(RampType.None, [HCT.Equal, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Higher, HCT.Equal], 1),
            new(RampType.None, [HCT.Equal, HCT.HigherOrEqual, HCT.Higher, HCT.Equal, HCT.Higher, HCT.Equal, HCT.HigherOrEqual, HCT.Higher], 1),
            new(RampType.None, [HCT.Higher, HCT.Equal, HCT.Higher, HCT.Equal, HCT.HigherOrEqual, HCT.Higher, HCT.Equal, HCT.HigherOrEqual], 1),
            new(RampType.None, [HCT.HigherOrEqual, HCT.Higher, HCT.Equal, HCT.HigherOrEqual, HCT.Higher, HCT.Equal, HCT.Higher, HCT.Equal], 1),
            new(RampType.None, [HCT.Higher, HCT.Equal, HCT.HigherOrEqual, HCT.Higher, HCT.Equal, HCT.HigherOrEqual, HCT.Higher, HCT.Equal], 1),

            new(RampType.None, [HCT.Higher, HCT.Irrelevant, HCT.Irrelevant, HCT.Irrelevant, HCT.Higher, HCT.Irrelevant, HCT.Irrelevant, HCT.Irrelevant], 1),
            new(RampType.None, [HCT.Irrelevant, HCT.Irrelevant, HCT.Higher, HCT.Irrelevant, HCT.Higher, HCT.Irrelevant, HCT.Higher, HCT.Irrelevant], 1),
        ];

        protected override TransitionRampInfo[] GetTransitionRampInfos() => transitionRampInfos;

        protected override TransitionRampInfo[] GetHeightFixers() => heightFixers;

        public override string GetDisplayString()
        {
            return $"Raise ground at {OriginCell} with a brush size of {BrushSize} using steep ramps";
        }

        public override void Perform() => RaiseGround();

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

            // With steep ramps we only consider the nearest 4 cells in the "primary directions"

            var northernCell = Map.GetTile(cellCoords + new Point2D(0, -1));
            if (northernCell != null && northernCell.Level > thisCell.Level && IsCellMorphable(northernCell))
            {
                biggestHeightDiff = Math.Max(biggestHeightDiff, northernCell.Level - thisCell.Level);
            }

            var southernCell = Map.GetTile(cellCoords + new Point2D(0, 1));
            if (southernCell != null && southernCell.Level > thisCell.Level && IsCellMorphable(southernCell))
            {
                biggestHeightDiff = Math.Max(biggestHeightDiff, southernCell.Level - thisCell.Level);
            }

            var westernCell = Map.GetTile(cellCoords + new Point2D(-1, 0));
            if (westernCell != null && westernCell.Level > thisCell.Level && IsCellMorphable(westernCell))
            {
                biggestHeightDiff = Math.Max(biggestHeightDiff, westernCell.Level - thisCell.Level);
            }

            var easternCell = Map.GetTile(cellCoords + new Point2D(1, 0));
            if (easternCell != null && easternCell.Level > thisCell.Level && IsCellMorphable(easternCell))
            {
                biggestHeightDiff = Math.Max(biggestHeightDiff, easternCell.Level - thisCell.Level);
            }

            // If nearby cells are raised by more than 1 cell, it's necessary to also raise this cell
            if (biggestHeightDiff > 1)
            {
                AddCellToUndoData(thisCell.CoordsToPoint());
                thisCell.Level += (byte)(biggestHeightDiff - 1);

                foreach (Point2D offset in SurroundingTiles)
                {
                    RegisterCell(cellCoords + offset);
                }
            }
        }
    }
}
