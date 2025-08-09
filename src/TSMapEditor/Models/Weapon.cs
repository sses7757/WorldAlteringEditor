namespace TSMapEditor.Models
{
    public class Weapon(string iniName) : INIDefineable, INIDefined
    {
        public string ININame { get; } = iniName;

        public int Index { get; set; }

        public double Range { get; set; }
    }
}
