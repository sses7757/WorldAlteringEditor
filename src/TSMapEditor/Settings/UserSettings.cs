using Rampastring.Tools;
using System;
using System.Threading.Tasks;

namespace TSMapEditor.Settings
{
    public static class UserSettings
    {
        private const string General = "General";
        private const string Display = "Display";
        private const string MapView = "MapView";

        static UserSettings()
        {
            UserSettingsIni = new IniFile(Environment.CurrentDirectory + "/MapEditorSettings.ini");

            settings =
            [
                TargetFPS,
                GraphicsLevel,
                ResolutionWidth,
                ResolutionHeight,
                RenderScale,
                Borderless,
                FullscreenWindowed,

                ScrollRate,
                MapWideOverlayOpacity,

                Theme,
                UseBoldFont,
                SmartScriptActionCloning,
                AutoSaveInterval,
                SidebarWidth,

                MultithreadedTextureLoading,

                GameDirectory,
                LastScenarioPath,

                TextEditorPath
            ];

            foreach (var setting in settings)
                setting.LoadValue(UserSettingsIni);

            RecentFiles.ReadFromIniFile(UserSettingsIni);
        }

        public static IniFile UserSettingsIni { get; }

        public static void SaveSettings()
        {
            foreach (var setting in settings)
            {
                setting.WriteValue(UserSettingsIni, false);
            }

            RecentFiles.WriteToIniFile(UserSettingsIni);

            UserSettingsIni.WriteIniFile();
        }

        public static async Task SaveSettingsAsync()
        {
            await Task.Factory.StartNew(SaveSettings);
        }

        private static readonly IINILoadable[] settings;

		public static IntSetting TargetFPS = new(Display, "TargetFPS", 240);
		public static IntSetting GraphicsLevel = new(Display, nameof(GraphicsLevel), 1);
        public static IntSetting ResolutionWidth = new(Display, "ResolutionWidth", -1);
        public static IntSetting ResolutionHeight = new(Display, "ResolutionHeight", -1);
        public static DoubleSetting RenderScale = new(Display, "RenderScale", 1.0);
        public static BoolSetting Borderless = new(Display, "Borderless", false);
        public static BoolSetting FullscreenWindowed = new(Display, "FullscreenWindowed", false);

        public static IntSetting ScrollRate = new(MapView, nameof(ScrollRate), 15);
        public static IntSetting MapWideOverlayOpacity = new(MapView, "MapWideOverlayOpacity", 50);

        public static StringSetting Theme = new(General, "Theme", "Default");
        public static BoolSetting UseBoldFont = new(General, "UseBoldFont", false);
        public static BoolSetting SmartScriptActionCloning = new(General, "SmartScriptActionCloning", true);
        public static IntSetting AutoSaveInterval = new(General, "AutoSaveInterval", 300);
        public static IntSetting SidebarWidth = new(General, "SidebarWidth", 250);

        public static BoolSetting MultithreadedTextureLoading = new(General, "MultithreadedTextureLoading", true);

        public static StringSetting GameDirectory = new(General, "GameDirectory", string.Empty);
        public static StringSetting LastScenarioPath = new(General, nameof(LastScenarioPath), "Maps/Custom/");

        public static StringSetting TextEditorPath = new(General, "TextEditorPath", string.Empty);

        public static RecentFiles RecentFiles = new();
    }
}
