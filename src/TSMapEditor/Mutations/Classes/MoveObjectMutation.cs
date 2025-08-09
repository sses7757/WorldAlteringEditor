using TSMapEditor.GameMath;
using TSMapEditor.Models;
using TSMapEditor.UI;

namespace TSMapEditor.Mutations.Classes
{
    /// <summary>
    /// A mutation that moves a game object on the map.
    /// </summary>
    public class MoveObjectMutation(IMutationTarget mutationTarget, IMovable movable, Point2D newPosition) : Mutation(mutationTarget)
    {
        private readonly IMovable movable = movable;
        private Point2D oldPosition = movable.Position;
        private Point2D newPosition = newPosition;

        private void MoveObject(Point2D position)
        {
            switch (movable.WhatAmI())
            {
                case RTTIType.Aircraft:
                    MutationTarget.Map.MoveAircraft((Aircraft)movable, position);
                    break;
                case RTTIType.Building:
                    MutationTarget.Map.MoveBuilding((Structure)movable, position);
                    break;
                case RTTIType.Unit:
                    MutationTarget.Map.MoveUnit((Unit)movable, position);
                    break;
                case RTTIType.Infantry:
                    MutationTarget.Map.MoveInfantry((Infantry)movable, position);
                    break;
                case RTTIType.Terrain:
                    MutationTarget.Map.MoveTerrainObject((TerrainObject)movable, position);
                    break;
                case RTTIType.Waypoint:
                    MutationTarget.Map.MoveWaypoint((Waypoint)movable, position);
                    break;
                case RTTIType.CellTag:
                    MutationTarget.Map.MoveCellTag((CellTag)movable, position);
                    break;
            }

            MutationTarget.AddRefreshPoint(newPosition);
            MutationTarget.AddRefreshPoint(oldPosition);
        }

        public override string GetDisplayString()
        {
            return $"Move {movable.WhatAmI()} from {oldPosition} to {newPosition}";
        }

        public override void Perform()
        {
            // TODO handle sub-cell for infantry
            MoveObject(newPosition);
        }

        public override void Undo()
        {
            // TODO handle sub-cell for infantry
            MoveObject(oldPosition);
        }
    }
}
