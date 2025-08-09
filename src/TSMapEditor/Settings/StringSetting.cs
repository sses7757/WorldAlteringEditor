namespace TSMapEditor.Settings
{
    public class StringSetting(string section, string key, string defaultValue) : SettingBase<string>(section, key, defaultValue)
    {
        protected override string GetValueFromString(string iniValue) => iniValue;

        protected override string GetValueString(string value) => value;
    }
}
