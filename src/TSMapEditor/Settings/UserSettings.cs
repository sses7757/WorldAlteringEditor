using Rampastring.Tools;
using System;
using System.Threading.Tasks;

namespace TSMapEditor.Settings
{
    public class UserSettings
    {
        private const string General = "General";
        private const string Display = "Display";
        private const string MapView = "MapView";

        public UserSettings()
        {
            if (Instance != null)
                throw new InvalidOperationException("User settings can only be initialized once.");

            Instance = this;

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

        public IniFile UserSettingsIni { get; }

        public void SaveSettings()
        {
            foreach (var setting in settings)
            {
                setting.WriteValue(UserSettingsIni, false);
            }

            RecentFiles.WriteToIniFile(UserSettingsIni);

            UserSettingsIni.WriteIniFile();
        }

        public async Task SaveSettingsAsync()
        {
            await Task.Factory.StartNew(SaveSettings);
        }

        public static UserSettings Instance { get; private set; }

        private readonly IINILoadable[] settings;

        public IntSetting TargetFPS = new(Display, "TargetFPS", 240);
        public IntSetting GraphicsLevel = new(Display, nameof(GraphicsLevel), 1);
        public IntSetting ResolutionWidth = new(Display, "ResolutionWidth", -1);
        public IntSetting ResolutionHeight = new(Display, "ResolutionHeight", -1);
        public DoubleSetting RenderScale = new(Display, "RenderScale", 1.0);
        public BoolSetting Borderless = new(Display, "Borderless", false);
        public BoolSetting FullscreenWindowed = new(Display, "FullscreenWindowed", false);

        public IntSetting ScrollRate = new(MapView, nameof(ScrollRate), 15);
        public IntSetting MapWideOverlayOpacity = new(MapView, "MapWideOverlayOpacity", 50);

        public StringSetting Theme = new(General, "Theme", "Default");
        public BoolSetting UseBoldFont = new(General, "UseBoldFont", false);
        public BoolSetting SmartScriptActionCloning = new(General, "SmartScriptActionCloning", true);
        public IntSetting AutoSaveInterval = new(General, "AutoSaveInterval", 300);
        public IntSetting SidebarWidth = new(General, "SidebarWidth", 250);

        public BoolSetting MultithreadedTextureLoading = new(General, "MultithreadedTextureLoading", true);

        public StringSetting GameDirectory = new(General, "GameDirectory", string.Empty);
        public StringSetting LastScenarioPath = new(General, nameof(LastScenarioPath), "Maps/Custom/");

        public StringSetting TextEditorPath = new(General, "TextEditorPath", string.Empty);

        public RecentFiles RecentFiles = new();
    }
}
