using TSMapEditor.GameMath;
using TSMapEditor.Models;
using TSMapEditor.UI;

namespace TSMapEditor.Mutations.Classes
{
    /// <summary>
    /// A mutation that allows placing a CellTag on the map.
    /// </summary>
    public class PlaceCellTagMutation(IMutationTarget mutationTarget, Point2D cellCoords, Tag tag) : Mutation(mutationTarget)
    {
        private readonly Point2D cellCoords = cellCoords;
        private readonly Tag tag = tag;

        public override string GetDisplayString()
        {
            return $"Place CellTag for '{tag.Name}' at {cellCoords}";
        }

        public override void Perform()
        {
            MutationTarget.Map.AddCellTag(new CellTag(cellCoords, tag));
            MutationTarget.AddRefreshPoint(cellCoords, 1);
        }

        public override void Undo()
        {
            MutationTarget.Map.RemoveCellTagFrom(cellCoords);
            MutationTarget.AddRefreshPoint(cellCoords, 1);
        }
    }
}
