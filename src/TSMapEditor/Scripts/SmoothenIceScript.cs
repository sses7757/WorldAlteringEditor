using Rampastring.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using TSMapEditor.CCEngine;
using TSMapEditor.GameMath;
using TSMapEditor.Models;

namespace TSMapEditor.Scripts
{
    /// <summary>
    /// Script that smoothens all pre-placed ice and randomizes
    /// the graphics variations of ice tiles.
    /// </summary>
    public class SmoothenIceScript
    {
        private class IceTransitionDirectionAndTileIndex(int tileIndex, bool isExhaustive, Direction[] directionsWithIce, Direction[] directionsWithoutIce = null)
        {
            public List<Direction> DirectionsWithIce = [.. directionsWithIce];

            /// <summary>
            /// If IsExhaustive is set to false, then this can be used to specify which directions
            /// should be checked for not having ice for this transition to be selected.
            /// </summary>
            public List<Direction> DirectionsWithoutIce = directionsWithoutIce?.ToList();

            /// <summary>
            /// If set to true, then no nearby cell aside from the ones listed 
            /// in DirectionsWithIce may contain ice for this transition to be selected.
            /// </summary>
            public bool IsExhaustive = isExhaustive;

            public int TileIndex = tileIndex;
        }

        private struct PendingTransition(Point2D cellCoords, int tileIndex)
        {
            public Point2D CellCoords = cellCoords;
            public int TileIndex = tileIndex;
        }

        // This is where the fun begins.
        // You have been warned. :evilkane:
        private readonly List<IceTransitionDirectionAndTileIndex> transitionInfo =
        [
            new IceTransitionDirectionAndTileIndex(17, false, [Direction.NE]),
            new IceTransitionDirectionAndTileIndex(18, false, [Direction.SE]),
            new IceTransitionDirectionAndTileIndex(19, false, [Direction.NE, Direction.SE]),
            new IceTransitionDirectionAndTileIndex(20, false, [Direction.SW]),
            new IceTransitionDirectionAndTileIndex(21, false, [Direction.NE, Direction.SW]),
            new IceTransitionDirectionAndTileIndex(22, false, [Direction.SE, Direction.SW]),
            new IceTransitionDirectionAndTileIndex(23, false, [Direction.NE, Direction.SE, Direction.SW]),
            new IceTransitionDirectionAndTileIndex(24, false, [Direction.NW], [Direction.SW]),
            new IceTransitionDirectionAndTileIndex(25, false, [Direction.NE, Direction.NW]),
            new IceTransitionDirectionAndTileIndex(26, false, [Direction.SE, Direction.NW]),
            new IceTransitionDirectionAndTileIndex(27, false, [Direction.NE, Direction.SE, Direction.NW]),
            new IceTransitionDirectionAndTileIndex(28, false, [Direction.SW, Direction.NW], [Direction.NE]),
            new IceTransitionDirectionAndTileIndex(29, false, [Direction.NE, Direction.SW, Direction.NW]),
            new IceTransitionDirectionAndTileIndex(30, false, [Direction.SE, Direction.SW, Direction.NW]),
            new IceTransitionDirectionAndTileIndex(31, false, [Direction.NE, Direction.SE, Direction.SW, Direction.NW]),
            new IceTransitionDirectionAndTileIndex(32, false, [Direction.NE, Direction.S], [Direction.SE, Direction.SW, Direction.NW]),
            new IceTransitionDirectionAndTileIndex(33, false, [Direction.SE, Direction.W], [Direction.NE, Direction.SW, Direction.NW]),
            new IceTransitionDirectionAndTileIndex(34, false, [Direction.NE, Direction.SE, Direction.W], [Direction.SW, Direction.NW]),
            new IceTransitionDirectionAndTileIndex(35, false, [Direction.SW, Direction.N], [Direction.NE, Direction.NW, Direction.SE]),
            new IceTransitionDirectionAndTileIndex(36, false, [Direction.SE, Direction.SW, Direction.N], [Direction.NW, Direction.NE]),
            new IceTransitionDirectionAndTileIndex(37, false, [Direction.NW, Direction.E], [Direction.SE, Direction.NE, Direction.SW]),
            new IceTransitionDirectionAndTileIndex(38, false, [Direction.NE, Direction.NW, Direction.S], [Direction.SE, Direction.SW]),
            new IceTransitionDirectionAndTileIndex(39, false, [Direction.SW, Direction.NW, Direction.E], [Direction.NE, Direction.SW]),
            new IceTransitionDirectionAndTileIndex(40, false, [Direction.NE, Direction.W], [Direction.NW, Direction.SE, Direction.SW]),
            new IceTransitionDirectionAndTileIndex(41, false, [Direction.SE, Direction.N], [Direction.NE, Direction.SW, Direction.NW]),
            new IceTransitionDirectionAndTileIndex(42, false, [Direction.SW, Direction.E], [Direction.SE, Direction.NW, Direction.NE]),
            new IceTransitionDirectionAndTileIndex(43, false, [Direction.NW, Direction.S], [Direction.SE, Direction.SW, Direction.NE]),
            new IceTransitionDirectionAndTileIndex(44, false, [Direction.NE, Direction.S, Direction.W], [Direction.NW, Direction.SE, Direction.SW]),
            new IceTransitionDirectionAndTileIndex(45, false, [Direction.SE, Direction.N, Direction.W], [Direction.SW, Direction.NE, Direction.NW]),
            new IceTransitionDirectionAndTileIndex(46, false, [Direction.SW, Direction.N, Direction.E], [Direction.SE, Direction.NW, Direction.NE]),
            new IceTransitionDirectionAndTileIndex(47, true, [Direction.NW, Direction.E, Direction.S]),
            new IceTransitionDirectionAndTileIndex(48, true, [Direction.N]),
            new IceTransitionDirectionAndTileIndex(49, true, [Direction.E]),
            new IceTransitionDirectionAndTileIndex(50, true, [Direction.S]),
            new IceTransitionDirectionAndTileIndex(51, true, [Direction.W]),
            new IceTransitionDirectionAndTileIndex(52, true, [Direction.E, Direction.N]),
            new IceTransitionDirectionAndTileIndex(53, true, [Direction.E, Direction.S]),
            new IceTransitionDirectionAndTileIndex(54, true, [Direction.S, Direction.W]),
            new IceTransitionDirectionAndTileIndex(55, true, [Direction.W, Direction.N]),
            new IceTransitionDirectionAndTileIndex(56, true, [Direction.E, Direction.S, Direction.N]),
            new IceTransitionDirectionAndTileIndex(57, true, [Direction.E, Direction.S, Direction.W]),
            new IceTransitionDirectionAndTileIndex(58, true, [Direction.S, Direction.W, Direction.N]),
            new IceTransitionDirectionAndTileIndex(59, true, [Direction.E, Direction.W, Direction.N]),
            new IceTransitionDirectionAndTileIndex(60, true, [Direction.S, Direction.N]),
            new IceTransitionDirectionAndTileIndex(61, true, [Direction.E, Direction.W]),
            new IceTransitionDirectionAndTileIndex(62, true, [Direction.E, Direction.S, Direction.W, Direction.N]),
        ];


        public void Perform(Map map)
        {
            transitionInfo.Reverse();

            var iceTileSetInfo = map.TheaterInstance.Theater.IceTileSetInfo;

            if (iceTileSetInfo.Ice1Set == null)
            {
                Logger.Log("Ice tile set 1 not found!");
                return;
            }

            if (iceTileSetInfo.Ice2Set == null)
            {
                Logger.Log("Ice tile set 2 not found!");
                return;
            }

            if (iceTileSetInfo.Ice3Set == null)
            {
                Logger.Log("Ice tile set 3 not found!");
                return;
            }

            if (iceTileSetInfo.IceShoreSet == null)
            {
                Logger.Log("Ice shore tile set not found!");
                return;
            }

            TileSet waterTileSet = map.TheaterInstance.Theater.TileSets.Find(ts => ts.SetName == "Water");
            if (waterTileSet == null)
            {
                Logger.Log("TileSet for regular (non-animated) water not found!");
                return;
            }

            // *****************************************************************************
            // Step 1: Find all ice tile set 02 and 03 tiles and switch them to ice 01 tiles
            // (this simplies the process for us)
            // *****************************************************************************

            map.DoForAllValidTiles(mapCell =>
            {
                int tileIndexInSet = -1;

                if (iceTileSetInfo.Ice2Set.ContainsTile(mapCell.TileIndex))
                {
                    tileIndexInSet = mapCell.TileIndex - iceTileSetInfo.Ice2Set.StartTileIndex;
                }
                else if (iceTileSetInfo.Ice3Set.ContainsTile(mapCell.TileIndex))
                {
                    tileIndexInSet = mapCell.TileIndex - iceTileSetInfo.Ice3Set.StartTileIndex;
                }

                if (tileIndexInSet > -1)
                {
                    mapCell.ChangeTileIndex(iceTileSetInfo.Ice1Set.StartTileIndex + tileIndexInSet, mapCell.SubTileIndex);
                }
            });

            // ***********************************************************
            // Step 2: Find all ice edge tiles and replace them with water
            // (more simplification, we re-generate the ice edges later)
            // ***********************************************************

            map.DoForAllValidTiles(mapCell =>
            {
                if (iceTileSetInfo.Ice1Set.ContainsTile(mapCell.TileIndex))
                {
                    int tileIndexInSet = mapCell.TileIndex - iceTileSetInfo.Ice1Set.StartTileIndex;

                    const int firstIceToWaterTransitionIndex = 17;

                    if (tileIndexInSet >= firstIceToWaterTransitionIndex)
                    {
                        mapCell.ChangeTileIndex(waterTileSet.StartTileIndex, 0);
                    }
                }
            });

            // ************************************************************
            // Step 3: Find all cracked ice and replace it with regular ice
            // (further simplification)
            // ************************************************************

            map.DoForAllValidTiles(mapCell =>
            {
                if (iceTileSetInfo.Ice1Set.ContainsTile(mapCell.TileIndex))
                {
                    int tileIndexInSet = mapCell.TileIndex - iceTileSetInfo.Ice1Set.StartTileIndex;

                    if (tileIndexInSet > 0)
                    {
                        mapCell.ChangeTileIndex(iceTileSetInfo.Ice1Set.StartTileIndex, 0);
                    }
                }
            });

            // *************************************************************************
            // Step 4: Find all water tiles that border ice and apply smooth transitions
            // *************************************************************************

            var directionsWithoutIce = new List<Direction>();

            var pendingTransitions = new List<PendingTransition>();

            map.DoForAllValidTiles(mapCell =>
            {
                if (waterTileSet.ContainsTile(mapCell.TileIndex))
                {
                    // Gather all the cells around this tile that have ice on them.
                    bool[] hasIce = new bool[(int)Direction.Count];
                    int surroundingIceCount = 0;

                    for (int d = 0; d < (int)Direction.Count; d++)
                    {
                        Point2D otherCellCoords = mapCell.CoordsToPoint() + Helpers.VisualDirectionToPoint((Direction)d);

                        MapTile otherCell = map.GetTile(otherCellCoords);
                        if (otherCell == null)
                            continue;

                        if (iceTileSetInfo.Ice1Set.ContainsTile(otherCell.TileIndex) ||
                            iceTileSetInfo.Ice2Set.ContainsTile(otherCell.TileIndex) ||
                            iceTileSetInfo.Ice3Set.ContainsTile(otherCell.TileIndex) ||
                            iceTileSetInfo.IceShoreSet.ContainsTile(otherCell.TileIndex))
                        {
                            hasIce[d] = true;
                            surroundingIceCount++;
                        }
                    }

                    if (surroundingIceCount == 0)
                    {
                        // No ice around this cell, skip it
                        return;
                    }

                    // Find the fitting ice transition for this cell
                    var transition = transitionInfo.Find(tr =>
                    {
                        if (!tr.DirectionsWithIce.TrueForAll(dir => hasIce[(int)dir]))
                            return false;

                        directionsWithoutIce.Clear();

                        if (tr.IsExhaustive)
                        {
                            for (int i = 0; i < (int)Direction.Count; i++)
                            {
                                if (tr.DirectionsWithIce.Contains((Direction)i))
                                    continue;

                                directionsWithoutIce.Add((Direction)i);
                            }

                            return directionsWithoutIce.TrueForAll(dir => !hasIce[(int)dir]);
                        }

                        return tr.DirectionsWithoutIce == null || tr.DirectionsWithoutIce.TrueForAll(dir => !hasIce[(int)dir]);
                    });

                    if (transition == null)
                    {
                        Logger.Log("No valid ice transition info found for cell at " + mapCell.CoordsToPoint().ToString());
                        return;
                    }

                    // Add the transition to pending
                    pendingTransitions.Add(new PendingTransition(mapCell.CoordsToPoint(), iceTileSetInfo.Ice1Set.StartTileIndex + transition.TileIndex));
                }
            });

            foreach (var transition in pendingTransitions)
            {
                var mapCell = map.GetTile(transition.CellCoords);
                mapCell.ChangeTileIndex(transition.TileIndex, 0);
            }

            // ********************************************
            // Step 5: Apply "Auto-LAT" for the cracked ice
            // (necessary for ice borders to look smooth)
            // ********************************************

            pendingTransitions.Clear();

            var nearbyCells = new Point2D[]
            {
                new(0, -1),
                new(-1, 0),
                new(0, 0),
                new(1, 0),
                new(0, 1)
            };

            map.DoForAllValidTiles(mapCell =>
            {
                if (mapCell.TileIndex == iceTileSetInfo.Ice1Set.StartTileIndex)
                {
                    foreach (var autoLatData in AutoLATType.AutoLATData)
                    {
                        bool applyTransition = true;

                        for (int i = 0; i < autoLatData.TransitionMatchArray.Length; i++)
                        {
                            var nearbyCellOffset = nearbyCells[i];
                            var cellCoords = nearbyCellOffset + mapCell.CoordsToPoint();

                            var otherCell = map.GetTile(cellCoords);
                            if (otherCell == null)
                            {
                                applyTransition = false;
                                break;
                            }

                            bool shouldMatch = autoLatData.TransitionMatchArray[i] > 0;
                            if (shouldMatch != (otherCell.TileIndex == iceTileSetInfo.Ice1Set.StartTileIndex))
                            {
                                // This is not the transition we're looking for
                                applyTransition = false;
                                break;
                            }
                        }

                        if (applyTransition)
                        {
                            // Because ice has the regular terrain tile and the LAT in the same TileSet,
                            // we need to increment the transition type index by 1
                            int tileIndexOffset = autoLatData.TransitionTypeIndex + 1;
                            pendingTransitions.Add(new PendingTransition(mapCell.CoordsToPoint(), iceTileSetInfo.Ice1Set.StartTileIndex + tileIndexOffset));

                            break;
                        }
                    }
                }
            });

            foreach (var transition in pendingTransitions)
            {
                var mapCell = map.GetTile(transition.CellCoords);
                mapCell.ChangeTileIndex(transition.TileIndex, 0);
            }

            // ***********************************************************************************
            // Step 6: Find all ice tiles and randomize their tilesets between the 3 ice tile sets
            // ***********************************************************************************

            var random = new Random();

            map.DoForAllValidTiles(mapCell =>
            {
                if (iceTileSetInfo.Ice1Set.ContainsTile(mapCell.TileIndex))
                {
                    int indexInSet = mapCell.TileIndex - iceTileSetInfo.Ice1Set.StartTileIndex;

                    int tileSetToUse = random.Next(0, 3);
                    TileSet iceTileSet = tileSetToUse switch
                    {
                        0 => iceTileSetInfo.Ice1Set,
                        1 => iceTileSetInfo.Ice2Set,
                        _ => iceTileSetInfo.Ice3Set,
                    };
                    mapCell.ChangeTileIndex(iceTileSet.StartTileIndex + indexInSet, 0);
                }
            });
        }
    }
}
