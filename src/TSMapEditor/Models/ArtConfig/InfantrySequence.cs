using Rampastring.Tools;
using System;

namespace TSMapEditor.Models.ArtConfig
{
    /// <summary>
    /// A Tiberian Sun / Red Alert 2 infantry art sequence.
    /// </summary>
    public class InfantrySequence(string iniName)
    {
        public string ININame { get; } = iniName;

        // We only care about the 'Ready' status
        public InfantrySequencePart Ready { get; private set; }

        /// <summary>
        /// Palette override introduced in Red Alert 2.
        /// </summary>
        public string Palette { get; set; }

        public void ParseFromINISection(IniSection iniSection)
        {
            int[] values = Array.ConvertAll(iniSection.GetStringValue("Ready", "0,1,1").Split(','), x => Conversions.IntFromString(x, 0));
            if (values.Length != 3)
                throw new FormatException("Invalid Ready= in infantry sequence " + ININame);

            Ready = new InfantrySequencePart(values[0], values[1], values[2]);

            Palette = iniSection.GetStringValue(nameof(Palette), Palette);
        }
    }

    public struct InfantrySequencePart(int startFrame, int frameCount, int facingMultiplier)
    {
        public int StartFrame = startFrame;
        public int FrameCount = frameCount;
        public int FacingMultiplier = facingMultiplier;
    }
}
