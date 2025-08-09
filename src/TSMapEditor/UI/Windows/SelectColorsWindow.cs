using System;
using Rampastring.XNAUI;
using Rampastring.XNAUI.XNAControls;
using TSMapEditor.Models;

namespace TSMapEditor.UI.Windows
{
    public class SelectColorsWindow(WindowManager windowManager, Map map) : SelectObjectWindow<RulesColor>(windowManager)
    {
        private readonly Map map = map;

        public override void Initialize()
        {
            Name = nameof(SelectColorsWindow);
            base.Initialize();
        }

        protected override void LbObjectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbObjectList.SelectedItem == null)
            {
                SelectedObject = null;
                return;
            }

            SelectedObject = (RulesColor)lbObjectList.SelectedItem.Tag;
        }

        protected override void ListObjects()
        {
            lbObjectList.Clear();

            for (int i = 0; i < map.Rules.Colors.Count; i++)
            {
                var color = map.Rules.Colors[i];

                lbObjectList.AddItem(new XNAListBoxItem() { Text = $"{i} {color.Name}", TextColor = color.XNAColor, Tag = color });
                if (color == SelectedObject)
                    lbObjectList.SelectedIndex = lbObjectList.Items.Count - 1;
            }
        }
    }
}
