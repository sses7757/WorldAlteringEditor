using TSMapEditor.Models;

namespace TSMapEditor.Rendering
{
    public class GraphicalBaseNode(BaseNode baseNode, BuildingType buildingType, House owner)
    {
        public BaseNode BaseNode { get; } = baseNode;
        public BuildingType BuildingType { get; set; } = buildingType;
        // public Structure Structure { get; set; }
        public House Owner { get; set; } = owner;
    }
}
