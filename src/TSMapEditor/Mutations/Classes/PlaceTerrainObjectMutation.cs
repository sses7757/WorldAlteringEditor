using System;
using TSMapEditor.GameMath;
using TSMapEditor.Models;
using TSMapEditor.UI;

namespace TSMapEditor.Mutations.Classes
{
    /// <summary>
    /// A mutation that allows placing terrain objects on the map.
    /// </summary>
    public class PlaceTerrainObjectMutation(IMutationTarget mutationTarget, TerrainType terrainType, Point2D cellCoords) : Mutation(mutationTarget)
    {
        private readonly TerrainType terrainType = terrainType;
        private readonly Point2D cellCoords = cellCoords;

        public override string GetDisplayString()
        {
            return $"Place terrain object '{terrainType.GetEditorDisplayName()}' at {cellCoords}";
        }

        public override void Perform()
        {
            var tile = MutationTarget.Map.GetTile(cellCoords);
            if (tile.TerrainObject != null)
                throw new InvalidOperationException("Cannot place a terrain object on a tile that already has a terrain object!");

            var terrainObject = new TerrainObject(terrainType, cellCoords);
            MutationTarget.Map.AddTerrainObject(terrainObject);
            MutationTarget.InvalidateMap();
        }

        public override void Undo()
        {
            MutationTarget.Map.RemoveTerrainObject(cellCoords);
            MutationTarget.InvalidateMap();
        }
    }
}
