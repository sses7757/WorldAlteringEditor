using Rampastring.Tools;
using System;
using System.Collections.Generic;
using TSMapEditor.Models.Enums;

namespace TSMapEditor.CCEngine
{
    public class TriggerEventParam(TriggerParamType triggerParamType, string nameOverride, List<string> presetOptions = null)
    {
        public TriggerParamType TriggerParamType { get; } = triggerParamType;
        public string NameOverride { get; } = nameOverride;
        public List<string> PresetOptions { get; } = presetOptions;
    }

    public class TriggerEventType(int id)
    {
        public const int DEF_PARAM_COUNT = 2;
        public const int MAX_PARAM_COUNT = 4;

        public int ID { get; set; } = id;

        public string Name { get; set; }
        public string Description { get; set; }
        public TriggerEventParam[] Parameters { get; } = new TriggerEventParam[MAX_PARAM_COUNT];
        public bool Available { get; set; } = true;

        public int AdditionalParams
        {
            get
            {
                int additionalParams = 0;

                for (int i = DEF_PARAM_COUNT; i < MAX_PARAM_COUNT; i++)
                {
                    var param = Parameters[i];
                    if (param.TriggerParamType != TriggerParamType.Unused)
                        additionalParams++;
                }

                return additionalParams;
            }
        }

        public void ReadPropertiesFromIniSection(IniSection iniSection)
        {
            ID = iniSection.GetIntValue("IDOverride", ID);
            Name = iniSection.GetStringValue(nameof(Name), string.Empty);
            Description = iniSection.GetStringValue(nameof(Description), string.Empty);
            Available = iniSection.GetBooleanValue(nameof(Available), true);

            for (int i = 0; i < Parameters.Length; i++)
            {
                string key = $"P{i + 1}Type";
                string nameOverrideKey = $"P{i + 1}Name";
                string presetOptionsKey = $"P{i + 1}PresetOptions";

                if (!iniSection.KeyExists(key))
                {
                    Parameters[i] = new TriggerEventParam(TriggerParamType.Unused, null);
                    continue;
                }

                var triggerParamType = (TriggerParamType)Enum.Parse(typeof(TriggerParamType), iniSection.GetStringValue(key, string.Empty));
                string nameOverride = iniSection.GetStringValue(nameOverrideKey, null);
                if (triggerParamType == TriggerParamType.WaypointZZ && string.IsNullOrWhiteSpace(nameOverride))
                    nameOverride = "Waypoint";

                List<string> presetOptions = null;
                string presetOptionsString = iniSection.GetStringValue(presetOptionsKey, null);
                if (!string.IsNullOrWhiteSpace(presetOptionsString))
                {
                    presetOptions = new List<string>(presetOptionsString.Split([','], StringSplitOptions.RemoveEmptyEntries));
                }

                Parameters[i] = new TriggerEventParam(triggerParamType, nameOverride, presetOptions);
            }
        }
    }
}
