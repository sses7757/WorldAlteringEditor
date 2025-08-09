using System;
using Rampastring.XNAUI;
using Rampastring.XNAUI.XNAControls;
using TSMapEditor.Models;

namespace TSMapEditor.UI.Windows
{
    public class SelectHouseTypeWindow(WindowManager windowManager, Map map) : SelectObjectWindow<HouseType>(windowManager)
    {
        private readonly Map map = map;

        public override void Initialize()
        {
            Name = nameof(SelectHouseTypeWindow);
            base.Initialize();
        }

        protected override void LbObjectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbObjectList.SelectedItem == null)
            {
                SelectedObject = null;
                return;
            }

            SelectedObject = (HouseType)lbObjectList.SelectedItem.Tag;
        }

        protected override void ListObjects()
        {
            lbObjectList.Clear();

            var houseTypes = map.GetHouseTypes();

            for (int i = 0; i < houseTypes.Count; i++)
            {
                HouseType houseType = houseTypes[i];

                lbObjectList.AddItem(new XNAListBoxItem()
                {
                    Text = $"{houseType.Index} {houseType.ININame}",
                    TextColor = Helpers.GetHouseTypeUITextColor(houseType),
                    Tag = houseType
                });

                if (houseType == SelectedObject)
                    lbObjectList.SelectedIndex = lbObjectList.Items.Count - 1;
            }
        }
    }
}
