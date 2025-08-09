using System.Globalization;

namespace TSMapEditor.Models
{
    public class SuperWeaponType(string iniName) : AbstractObject, INIDefined
    {
        [INI(false)]
        public string ININame { get; } = iniName;

        [INI(false)]
        public int Index { get; set; }

        public string Name { get; set; }

        public string GetDisplayString() => $"{Index.ToString(CultureInfo.InvariantCulture)} {GetDisplayStringWithoutIndex()}";

        public string GetDisplayStringWithoutIndex() => $"{Name} ({ININame})";

        public override RTTIType WhatAmI() => RTTIType.SuperWeaponType;
    }
}
