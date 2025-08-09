using TSMapEditor.GameMath;
using TSMapEditor.Models;
using TSMapEditor.Mutations.Classes;

namespace TSMapEditor.UI.CursorActions
{
    public class PlaceCellTagCursorAction(ICursorActionTarget cursorActionTarget) : CursorAction(cursorActionTarget)
    {
        public override string GetName() => "Place CellTag";

        private Tag _tag;
        public Tag Tag
        {
            get => _tag;
            set
            {
                if (_tag != value)
                {
                    _tag = value;
                    cellTag.Tag = _tag;
                }
            }
        }

        private readonly CellTag cellTag = new();

        public override void PreMapDraw(Point2D cellCoords)
        {
            var cell = CursorActionTarget.Map.GetTile(cellCoords);

            if (cell.CellTag == null)
            {
                cellTag.Position = cellCoords;
                cell.CellTag = cellTag;
            }

            CursorActionTarget.AddRefreshPoint(cellCoords, 1);
        }

        public override void PostMapDraw(Point2D cellCoords)
        {
            var cell = CursorActionTarget.Map.GetTile(cellCoords);
            if (cell.CellTag == cellTag)
            {
                cell.CellTag = null;
            }

            CursorActionTarget.AddRefreshPoint(cellCoords, 1);
        }

        public override void LeftDown(Point2D cellCoords)
        {
            var cell = CursorActionTarget.Map.GetTile(cellCoords);
            if (cell.CellTag == null)
            {
                CursorActionTarget.MutationManager.PerformMutation(
                    new PlaceCellTagMutation(CursorActionTarget.MutationTarget, cellCoords, Tag));
            }

            base.LeftDown(cellCoords);
        }

        public override void LeftClick(Point2D cellCoords) => LeftDown(cellCoords);
    }
}
