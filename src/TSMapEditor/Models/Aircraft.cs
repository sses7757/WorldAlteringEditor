namespace TSMapEditor.Models
{
    public class Aircraft(AircraftType objectType) : Foot<AircraftType>(objectType)
    {
        public AircraftType AircraftType => ObjectType;

        // [Aircraft]
        // INDEX=OWNER,ID,HEALTH,X,Y,FACING,MISSION,TAG,VETERANCY,GROUP,AUTOCREATE_NO_RECRUITABLE,AUTOCREATE_YES_RECRUITABLE

        public override RTTIType WhatAmI() => RTTIType.Aircraft;

        public override bool Remapable() => ObjectType.ArtConfig.Remapable;
    }
}
