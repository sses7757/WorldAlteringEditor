using Rampastring.Tools;
using System.Collections.Generic;
using TSMapEditor.Models;

namespace TSMapEditor.UI
{
    /// <summary>
    /// Combines many terrain objects into a single entry.
    /// </summary>
    public class TerrainObjectCollection : ObjectTypeCollection
    {
        public struct TerrainObjectCollectionEntry(TerrainType terrainType)
        {
            public TerrainType TerrainType = terrainType;
        }

        public TerrainObjectCollectionEntry[] Entries;

        public static TerrainObjectCollection InitFromIniSection(IniSection iniSection, List<TerrainType> terrainTypes)
        {
            var terrainObjectCollection = new TerrainObjectCollection
            {
                Name = iniSection.GetStringValue("Name", "Unnamed Collection"),
                AllowedTheaters = iniSection.GetListValue("AllowedTheaters", ',', s => s)
            };

            var entryList = new List<TerrainObjectCollectionEntry>();

            int i = 0;
            while (true)
            {
                string terrainTypeName = iniSection.GetStringValue("TerrainObjectType" + i, null);
                if (string.IsNullOrWhiteSpace(terrainTypeName))
                    break;

                var terrainType = terrainTypes.Find(o => o.ININame == terrainTypeName) ?? throw new INIConfigException($"Terrain object type \"{terrainTypeName}\" not found while initializing terrain object collection \"{terrainObjectCollection.Name}\"!");
                entryList.Add(new TerrainObjectCollectionEntry(terrainType));

                i++;
            }

            terrainObjectCollection.Entries = [.. entryList];
            return terrainObjectCollection;
        }
    }
}
