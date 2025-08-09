using TSMapEditor.GameMath;
using TSMapEditor.Models;

namespace TSMapEditor.Rendering
{
    internal struct AlphaImageRenderStruct(Point2D point, ShapeImage alphaImage, GameObject ownerObject)
    {
        public Point2D Point = point;
        public ShapeImage AlphaImage = alphaImage;
        public GameObject OwnerObject = ownerObject;
    }
}
