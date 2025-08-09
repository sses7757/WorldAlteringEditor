using Rampastring.XNAUI;
using Rampastring.XNAUI.XNAControls;
using System;
using TSMapEditor.Models;

namespace TSMapEditor.UI.Windows
{
    public class SelectSuperWeaponTypeWindow(WindowManager windowManager, Map map) : SelectObjectWindow<SuperWeaponType>(windowManager)
    {
        private readonly Map map = map;
        public bool UseININameAsValue { get; set; }

        public override void Initialize()
        {
            Name = nameof(SelectSuperWeaponTypeWindow);
            base.Initialize();
        }

        protected override void LbObjectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbObjectList.SelectedItem == null)
            {
                SelectedObject = null;
                return;
            }

            SelectedObject = (SuperWeaponType)lbObjectList.SelectedItem.Tag;
        }

        protected override void ListObjects()
        {
            lbObjectList.Clear();

            foreach (var swType in map.Rules.SuperWeaponTypes)
            {
                lbObjectList.AddItem(new XNAListBoxItem() { Text = swType.GetDisplayString(), Tag = swType });
                if (swType == SelectedObject)
                    lbObjectList.SelectedIndex = lbObjectList.Items.Count - 1;
            }
        }
    }
}
