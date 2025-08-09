using Rampastring.XNAUI;
using Rampastring.XNAUI.XNAControls;
using System;
using System.Linq;
using TSMapEditor.Models;

namespace TSMapEditor.UI.Windows
{
    public class SelectConnectedTileWindow(WindowManager windowManager, Map map) : SelectObjectWindow<CliffType>(windowManager)
    {
        private readonly Map map = map;

        public override void Initialize()
        {
            Name = nameof(SelectConnectedTileWindow);
            base.Initialize();
        }

        protected override void LbObjectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbObjectList.SelectedItem == null)
            {
                SelectedObject = null;
                return;
            }

            SelectedObject = (CliffType)lbObjectList.SelectedItem.Tag;
        }

        public void Open()
        {
            Open(null);
        }

        protected override void ListObjects()
        {
            lbObjectList.Clear();

            foreach (CliffType cliff in map.EditorConfig.Cliffs.Where(cliff =>
                         cliff.AllowedTheaters.Exists(theaterName => theaterName.Equals(map.TheaterName, StringComparison.OrdinalIgnoreCase))))
            {
                if (cliff.IsLegal)
                    lbObjectList.AddItem(new XNAListBoxItem() { Text = cliff.Name, Tag = cliff, TextColor = cliff.Color.GetValueOrDefault(lbObjectList.DefaultItemColor) });
            }
        }
    }
}