using System;
using Rampastring.XNAUI;
using Rampastring.XNAUI.XNAControls;
using TSMapEditor.CCEngine;
using TSMapEditor.Models;

namespace TSMapEditor.UI.Windows
{
    public class SelectActionWindow(WindowManager windowManager, Map map) : SelectObjectWindow<TriggerActionType>(windowManager)
    {
        private readonly Map map = map;

        public bool IsAddingNew { get; set; }

        public override void Initialize()
        {
            Name = nameof(SelectActionWindow);
            base.Initialize();
        }

        protected override void LbObjectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbObjectList.SelectedItem == null)
            {
                SelectedObject = null;
                return;
            }

            SelectedObject = (TriggerActionType)lbObjectList.SelectedItem.Tag;
        }

        protected override void ListObjects()
        {
            lbObjectList.Clear();

            foreach (TriggerActionType triggerActionType in map.EditorConfig.TriggerActionTypes.Values)
            {
                lbObjectList.AddItem(new XNAListBoxItem() { Text = $"{triggerActionType.ID} {triggerActionType.Name}", Tag = triggerActionType });
                if (triggerActionType == SelectedObject)
                    lbObjectList.SelectedIndex = lbObjectList.Items.Count - 1;
            }
        }
    }
}
