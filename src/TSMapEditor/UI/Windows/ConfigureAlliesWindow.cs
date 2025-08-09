using Rampastring.XNAUI;
using Rampastring.XNAUI.XNAControls;
using System;
using System.Collections.Generic;
using System.Linq;
using TSMapEditor.Models;
using TSMapEditor.UI.Controls;

namespace TSMapEditor.UI.Windows
{
    public class ConfigureAlliesWindow(WindowManager windowManager, Map map) : INItializableWindow(windowManager)
    {
        public event EventHandler AlliesUpdated;

        private readonly Map map = map;

        private XNAPanel panelCheckBoxes;
        private EditorButton btnApply;

        private readonly List<XNACheckBox> checkBoxes = [];

        private House house;

        public override void Initialize()
        {
            Name = nameof(ConfigureAlliesWindow);
            base.Initialize();

            panelCheckBoxes = FindChild<XNAPanel>(nameof(panelCheckBoxes));

            btnApply = FindChild<EditorButton>("btnApply");
            btnApply.LeftClick += BtnApply_LeftClick;
        }

        private void BtnApply_LeftClick(object sender, EventArgs e)
        {
            string allies = string.Join(',', new string[] { house.ININame }.Concat(checkBoxes.FindAll(chk => chk.Checked).Select(chk => chk.Text)));
            house.Allies = allies;

            AlliesUpdated?.Invoke(this, EventArgs.Empty);

            Hide();
        }

        public void Open(House house)
        {
            this.house = house;

            RefreshCheckBoxes();

            Show();
        }

        private void RefreshCheckBoxes()
        {
            checkBoxes.ForEach(chk => panelCheckBoxes.RemoveChild(chk));
            checkBoxes.Clear();

            string[] existingAllies = house.Allies.Split([','], StringSplitOptions.RemoveEmptyEntries);

            int y = 0;

            bool useTwoColumns = map.Houses.Count > 16;
            bool isSecondColumn = false;

            foreach (var otherHouse in map.Houses)
            {
                if (otherHouse == house)
                    continue;

                var checkBox = new XNACheckBox(WindowManager)
                {
                    Name = "chk" + otherHouse.ININame,
                    X = isSecondColumn ? 150 : 0,
                    Y = y,
                    Text = otherHouse.ININame,
                    Checked = Array.Exists(existingAllies, s => s == otherHouse.ININame)
                };
                panelCheckBoxes.AddChild(checkBox);
                checkBoxes.Add(checkBox);

                if (!useTwoColumns || isSecondColumn)
                    y = checkBox.Bottom + Constants.UIVerticalSpacing;

                if (useTwoColumns)
                    isSecondColumn = !isSecondColumn;
            }

            panelCheckBoxes.Height = checkBoxes.Count > 0 ? checkBoxes[checkBoxes.Count - 1].Bottom : 0;
            btnApply.Y = panelCheckBoxes.Bottom + Constants.UIEmptyTopSpace;
            Height = btnApply.Bottom + Constants.UIEmptyBottomSpace;

            CenterOnParent();
        }
    }
}
