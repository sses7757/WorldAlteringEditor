using TSMapEditor.GameMath;
using TSMapEditor.Models;
using TSMapEditor.UI;

namespace TSMapEditor.Mutations.Classes
{
    /// <summary>
    /// A mutation that allows placing aircraft on the map.
    /// </summary>
    public class PlaceAircraftMutation(IMutationTarget mutationTarget, AircraftType aircraftType, Point2D cellCoords) : Mutation(mutationTarget)
    {
        private readonly AircraftType aircraftType = aircraftType;
        private readonly Point2D cellCoords = cellCoords;
        private Aircraft aircraft;

        public override string GetDisplayString()
        {
            return $"Place '{aircraftType.GetEditorDisplayName()}' at {cellCoords}";
        }

        public override void Perform()
        {
            var cell = MutationTarget.Map.GetTile(cellCoords);
            if (cell == null)
                return;

            aircraft = new Aircraft(aircraftType)
            {
                Owner = MutationTarget.ObjectOwner,
                Position = cellCoords
            };
            MutationTarget.Map.PlaceAircraft(aircraft);
            MutationTarget.AddRefreshPoint(cellCoords);
        }

        public override void Undo()
        {
            var cell = MutationTarget.Map.GetTile(cellCoords);
            MutationTarget.Map.RemoveAircraft(aircraft);
            MutationTarget.AddRefreshPoint(cellCoords);
        }
    }
}
