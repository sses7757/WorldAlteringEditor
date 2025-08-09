using Microsoft.Xna.Framework;

namespace TSMapEditor.Misc
{
    public static class NamedColors
    {        
        public static NamedColor[] GenericSupportedNamedColors =
        [
            new("Teal", new Color(0, 196, 196)),
            new("Green", new Color(0, 255, 0)),
            new("Dark Green", Color.Green),
            new("Lime Green", Color.LimeGreen),
            new("Yellow", Color.Yellow),
            new("Orange", Color.Orange),
            new("Red", Color.Red),
            new("Blood Red", Color.DarkRed),
            new("Pink", Color.HotPink),
            new("Cherry", Color.Pink),
            new("Purple", Color.MediumPurple),
            new("Sky Blue", Color.SkyBlue),
            new("Blue", new Color(40, 40, 255)),
            new("Brown", Color.Brown),
            new("Metalic", new Color(160, 160, 200)),
        ];
    }

    public struct NamedColor(string name, Color value)
    {
        public string Name = name;
        public Color Value = value;
    }
}
