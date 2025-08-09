using System;
using Rampastring.XNAUI;
using Rampastring.XNAUI.XNAControls;
using TSMapEditor.Models;

namespace TSMapEditor.UI.Windows
{
    /// <summary>
    /// A window that allows the user to select a TaskForce (for example, for a TeamType).
    /// </summary>
    public class SelectTaskForceWindow(WindowManager windowManager, Map map) : SelectObjectWindow<TaskForce>(windowManager)
    {
        private readonly Map map = map;

        public override void Initialize()
        {
            Name = nameof(SelectTaskForceWindow);
            base.Initialize();
        }

        protected override void LbObjectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbObjectList.SelectedItem == null)
            {
                SelectedObject = null;
                return;
            }

            SelectedObject = (TaskForce)lbObjectList.SelectedItem.Tag;
        }

        protected override void ListObjects()
        {
            lbObjectList.Clear();

            foreach (TaskForce taskForce in map.TaskForces)
            {
                lbObjectList.AddItem(new XNAListBoxItem() { Text = $"{taskForce.Name} ({taskForce.ININame})", Tag = taskForce });
                if (taskForce == SelectedObject)
                    lbObjectList.SelectedIndex = lbObjectList.Items.Count - 1;
            }
        }
    }
}
