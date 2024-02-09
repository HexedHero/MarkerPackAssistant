using Blish_HUD.Settings;

namespace HexedHero.Blish_HUD.MarkerPackAssistant.Objects
{
    public class ModuleSettings
    {

        public SettingCollection SettingCollection { get; private set; }

        public SettingEntry<string> MarkerPackBuildPath { get; private set; }

        public ModuleSettings(SettingCollection settingsCollection)
        {

            this.SettingCollection = settingsCollection;

            MarkerPackBuildPath = settingsCollection.DefineSetting(
                nameof(MarkerPackBuildPath),
                "C:\\path\\to\\install.bat",
                () => "Batch path",
                () => "The path location to your batch file to install your marker pack."
            );

        }

    }
}
