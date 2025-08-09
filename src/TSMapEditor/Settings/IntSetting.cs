using Rampastring.Tools;
using System.Globalization;

namespace TSMapEditor.Settings
{
    public class IntSetting(string section, string key, int defaultValue) : SettingBase<int>(section, key, defaultValue)
    {
        protected override int GetValueFromString(string iniValue)
        {
            return Conversions.IntFromString(iniValue, DefaultValue);
        }

        protected override string GetValueString(int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
