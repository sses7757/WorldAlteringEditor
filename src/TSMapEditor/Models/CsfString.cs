namespace TSMapEditor.Models
{
    public class CsfString(string id, string value)
    {
        public string ID { get; } = id;
        public string Value { get; } = value;
    }
}
