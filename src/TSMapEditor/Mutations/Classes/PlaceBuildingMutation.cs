using TSMapEditor.GameMath;
using TSMapEditor.Models;
using TSMapEditor.UI;

namespace TSMapEditor.Mutations.Classes
{

    /// <summary>
    /// A mutation that allows placing a building on the map.
    /// </summary>
    public class PlaceBuildingMutation(IMutationTarget mutationTarget, BuildingType buildingType, Point2D cellCoords) : Mutation(mutationTarget)
    {
        private readonly BuildingType buildingType = buildingType;
        private readonly Point2D cellCoords = cellCoords;

        private Structure placedBuilding;

        public override string GetDisplayString()
        {
            return $"Place '{buildingType.GetEditorDisplayName()}' at {cellCoords}";
        }

        public override void Perform()
        {
            var cell = MutationTarget.Map.GetTileOrFail(cellCoords);

            var structure = new Structure(buildingType)
            {
                Owner = MutationTarget.ObjectOwner,
                Position = cellCoords
            };
            MutationTarget.Map.PlaceBuilding(structure);
            MutationTarget.AddRefreshPoint(cellCoords);

            placedBuilding = structure;
            MutationTarget.AddRefreshPoint(cellCoords);
        }

        public override void Undo()
        {
            MutationTarget.Map.RemoveBuilding(placedBuilding);
            MutationTarget.AddRefreshPoint(cellCoords);
        }
    }
}
