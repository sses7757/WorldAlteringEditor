namespace TSMapEditor.Models
{
    public class TeamTypeFlag(string name, bool defaultValue)
    {
        public string Name { get; } = name;
        public bool DefaultValue { get; } = defaultValue;
    }
}
