using TSMapEditor.Models;
using TSMapEditor.Rendering;
using TSMapEditor.UI;

namespace TSMapEditor.Mutations.Classes
{
    public class PlaceTubeMutation(IMutationTarget mutationTarget, Tube tube) : Mutation(mutationTarget)
    {
        private readonly Tube tube = tube;

        public override string GetDisplayString()
        {
            return $"Place tunnel tube of length {tube.Directions.Count} at {tube.EntryPoint}";
        }

        public override void Perform()
        {
            MutationTarget.Map.Tubes.Add(tube);
            TubeRefreshHelper.MapViewRefreshTube(tube, MutationTarget);
        }

        public override void Undo()
        {
            MutationTarget.Map.Tubes.Remove(tube);
            TubeRefreshHelper.MapViewRefreshTube(tube, MutationTarget);
        }
    }
}
