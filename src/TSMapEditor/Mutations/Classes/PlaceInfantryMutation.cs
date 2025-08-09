using System;
using TSMapEditor.GameMath;
using TSMapEditor.Models;
using TSMapEditor.UI;

namespace TSMapEditor.Mutations.Classes
{
    /// <summary>
    /// A mutation that allows placing infantry on the map.
    /// </summary>
    public class PlaceInfantryMutation(IMutationTarget mutationTarget, InfantryType infantryType, Point2D cellCoords, SubCell subCell) : Mutation(mutationTarget)
    {
        private readonly InfantryType infantryType = infantryType;
        private readonly Point2D cellCoords = cellCoords;
        private readonly SubCell subCell = subCell;

        private Infantry placedInfantry;

        public override string GetDisplayString()
        {
            return $"Place '{infantryType.GetEditorDisplayName()}' at {cellCoords}";
        }

        public override void Perform()
        {
            var cell = MutationTarget.Map.GetTile(cellCoords) ?? throw new InvalidOperationException("Invalid cell coords");
            if (cell.Infantry[(int)subCell] != null)
                throw new InvalidOperationException(nameof(PlaceInfantryMutation) + ": cannot place infantry on an occupied sub-cell spot!");

            var infantry = new Infantry(infantryType)
            {
                Owner = MutationTarget.ObjectOwner,
                Position = cellCoords,
                SubCell = subCell
            };
            placedInfantry = infantry;

            MutationTarget.Map.PlaceInfantry(infantry);
            MutationTarget.AddRefreshPoint(cellCoords);
        }

        public override void Undo()
        {
            MutationTarget.Map.RemoveInfantry(placedInfantry);
            MutationTarget.AddRefreshPoint(placedInfantry.Position);
        }
    }
}
