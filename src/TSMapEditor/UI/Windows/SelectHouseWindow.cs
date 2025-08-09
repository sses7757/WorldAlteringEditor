using System;
using Rampastring.XNAUI;
using Rampastring.XNAUI.XNAControls;
using TSMapEditor.Models;

namespace TSMapEditor.UI.Windows
{
    public class SelectHouseWindow(WindowManager windowManager, Map map) : SelectObjectWindow<House>(windowManager)
    {
        private readonly Map map = map;

        public override void Initialize()
        {
            Name = nameof(SelectHouseWindow);
            base.Initialize();
        }

        protected override void LbObjectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbObjectList.SelectedItem == null)
            {
                SelectedObject = null;
                return;
            }

            SelectedObject = (House)lbObjectList.SelectedItem.Tag;
        }

        protected override void ListObjects()
        {
            lbObjectList.Clear();

            for (int i = 0; i < map.GetHouses().Count; i++)
            {
                House house = map.GetHouses()[i];

                lbObjectList.AddItem(new XNAListBoxItem() { Text = $"{i} {house.ININame}", TextColor = house.XNAColor, Tag = house });
                if (house == SelectedObject)
                    lbObjectList.SelectedIndex = lbObjectList.Items.Count - 1;
            }
        }
    }
}
