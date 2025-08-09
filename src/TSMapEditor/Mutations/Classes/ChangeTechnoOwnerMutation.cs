using TSMapEditor.Models;
using TSMapEditor.UI;

namespace TSMapEditor.Mutations.Classes
{
    /// <summary>
    /// A mutation that changes the owner of an object.
    /// </summary>
    public class ChangeTechnoOwnerMutation(TechnoBase techno, House newOwner, IMutationTarget mutationTarget) : Mutation(mutationTarget)
    {
        private readonly TechnoBase techno = techno;
        private readonly House oldOwner = techno.Owner;
        private readonly House newOwner = newOwner;

        public override string GetDisplayString()
        {
            return $"Change owner of {techno.GetObjectType().GetEditorDisplayName()} at {techno.Position} to {newOwner.ININame}";
        }

        public override void Perform()
        {
            techno.Owner = newOwner;
            MutationTarget.AddRefreshPoint(techno.Position);
        }

        public override void Undo()
        {
            techno.Owner = oldOwner;
            MutationTarget.AddRefreshPoint(techno.Position);
        }
    }
}
