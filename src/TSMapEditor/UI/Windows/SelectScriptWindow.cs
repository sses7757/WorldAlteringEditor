using System;
using Rampastring.XNAUI;
using Rampastring.XNAUI.XNAControls;
using TSMapEditor.Models;

namespace TSMapEditor.UI.Windows
{
    public class SelectScriptWindow(WindowManager windowManager, Map map) : SelectObjectWindow<Script>(windowManager)
    {
        private readonly Map map = map;

        public override void Initialize()
        {
            Name = nameof(SelectScriptWindow);
            base.Initialize();
        }

        protected override void LbObjectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbObjectList.SelectedItem == null)
            {
                SelectedObject = null;
                return;
            }

            SelectedObject = (Script)lbObjectList.SelectedItem.Tag;
        }

        protected override void ListObjects()
        {
            lbObjectList.Clear();

            foreach (Script script in map.Scripts)
            {
                lbObjectList.AddItem(new XNAListBoxItem() 
                {
                    Text = $"{script.Name} ({script.ININame})",
                    Tag = script,
                    TextColor = script.EditorColor == null ? lbObjectList.DefaultItemColor : script.XNAColor
                });
                if (script == SelectedObject)
                    lbObjectList.SelectedIndex = lbObjectList.Items.Count - 1;
            }
        }
    }
}
