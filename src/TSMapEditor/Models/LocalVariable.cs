namespace TSMapEditor.Models
{
    public class LocalVariable(int index)
    {
        public int Index { get; } = index;
        public string Name { get; set; }
        public int InitialState { get; set; }
    }
}
