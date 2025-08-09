using TSMapEditor.Models.ArtConfig;

namespace TSMapEditor.Models
{
    public class InfantryType(string iniName) : TechnoType(iniName), IArtConfigContainer
    {
        public InfantryArtConfig ArtConfig { get; } = new InfantryArtConfig();
        public IArtConfig GetArtConfig() => ArtConfig;

        public override RTTIType WhatAmI() => RTTIType.InfantryType;
    }
}
