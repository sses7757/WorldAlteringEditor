using TSMapEditor.Models.ArtConfig;

namespace TSMapEditor.Models
{
    public class AircraftType(string iniName) : TechnoType(iniName), IArtConfigContainer
    {
        public AircraftArtConfig ArtConfig { get; private set; } = new AircraftArtConfig();
        public IArtConfig GetArtConfig() => ArtConfig;

        public override RTTIType WhatAmI() => RTTIType.AircraftType;
    }
}
