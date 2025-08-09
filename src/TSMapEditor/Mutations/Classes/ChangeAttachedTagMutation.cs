using TSMapEditor.Models;
using TSMapEditor.UI;

namespace TSMapEditor.Mutations.Classes
{
    /// <summary>
    /// A mutation that allows changing a Techno's 
    /// (building/vehicle/infantry/aircraft) attached trigger tag.
    /// </summary>
    public class ChangeAttachedTagMutation(IMutationTarget mutationTarget, TechnoBase techno, Tag tag) : Mutation(mutationTarget)
    {
        private readonly TechnoBase techno = techno;
        private readonly Tag tag = tag;

        private Tag oldAttachedTag;

        public override string GetDisplayString()
        {
            return $"Change attached tag of '{techno.GetObjectType().GetEditorDisplayName()}' at {techno.Position} to '{tag.Name}'";
        }

        public override void Perform()
        {
            oldAttachedTag = techno.AttachedTag;
            techno.AttachedTag = tag;
        }

        public override void Undo()
        {
            techno.AttachedTag = oldAttachedTag;
        }
    }
}
