using Rampastring.XNAUI;
using Rampastring.XNAUI.XNAControls;
using System;
using TSMapEditor.Models;

namespace TSMapEditor.UI.Windows
{
    public class SelectSpeechWindow(WindowManager windowManager, Map map) : SelectObjectWindow<EvaSpeech>(windowManager)
    {
        private readonly Map map = map;

        public override void Initialize()
        {
            Name = nameof(SelectSpeechWindow);
            base.Initialize();
        }

        protected override void LbObjectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedObject = (EvaSpeech)lbObjectList.SelectedItem?.Tag;
        }

        protected override void ListObjects()
        {
            lbObjectList.Clear();

            var speechList = Constants.IsRA2YR ? map.Rules.Speeches.List : map.EditorConfig.Speeches.List;
            foreach (var evaSpeech in speechList)
            {
                lbObjectList.AddItem(new XNAListBoxItem() { Text = evaSpeech.ToString(), Tag = evaSpeech });
                if (evaSpeech == SelectedObject)
                    lbObjectList.SelectedIndex = lbObjectList.Items.Count - 1;
            }
        }
    }
}
