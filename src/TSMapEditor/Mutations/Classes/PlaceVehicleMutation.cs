using TSMapEditor.GameMath;
using TSMapEditor.Models;
using TSMapEditor.UI;

namespace TSMapEditor.Mutations.Classes
{
    /// <summary>
    /// A mutation that allows placing a vehicle on the map.
    /// </summary>
    public class PlaceVehicleMutation(IMutationTarget mutationTarget, UnitType unitType, Point2D cellCoords) : Mutation(mutationTarget)
    {
        private readonly UnitType unitType = unitType;
        private readonly Point2D cellCoords = cellCoords;
        private Unit unit;

        public override string GetDisplayString()
        {
            return $"Place '{unitType.GetEditorDisplayName()}' at {cellCoords}";
        }

        public override void Perform()
        {
            var cell = MutationTarget.Map.GetTileOrFail(cellCoords);

            unit = new Unit(unitType)
            {
                Owner = MutationTarget.ObjectOwner,
                Position = cellCoords
            };
            MutationTarget.Map.PlaceUnit(unit);
            MutationTarget.AddRefreshPoint(cellCoords);
        }

        public override void Undo()
        {
            var cell = MutationTarget.Map.GetTile(cellCoords);
            MutationTarget.Map.RemoveUnit(unit);
            MutationTarget.AddRefreshPoint(cellCoords);
        }
    }
}
