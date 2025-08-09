using TSMapEditor.GameMath;
using TSMapEditor.Models;
using TSMapEditor.UI;

namespace TSMapEditor.Mutations.Classes
{
    /// <summary>
    /// A mutation that allows placing a waypoint on the map.
    /// </summary>
    public class PlaceWaypointMutation(IMutationTarget mutationTarget, Point2D cellCoords, int waypointNumber, string waypointColor = null) : Mutation(mutationTarget)
    {
        private readonly Point2D cellCoords = cellCoords;
        private readonly int waypointNumber = waypointNumber;
        private readonly string waypointColor = waypointColor;
        private Waypoint waypoint;

        public override string GetDisplayString()
        {
            return $"Place waypoint {waypointNumber} at {cellCoords}";
        }

        public override void Perform()
        {
            waypoint = new Waypoint
            {
                Identifier = waypointNumber,
                Position = cellCoords,
                EditorColor = waypointColor
            };
            MutationTarget.Map.AddWaypoint(waypoint);
            MutationTarget.AddRefreshPoint(cellCoords, 1);
        }

        public override void Undo()
        {
            MutationTarget.Map.RemoveWaypoint(waypoint);
            MutationTarget.AddRefreshPoint(cellCoords, 1);
        }
    }
}
