using Blish_HUD;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using HexedHero.Blish_HUD.MarkerPackAssistant.Managers;
using System.ComponentModel.Composition;

namespace HexedHero.Blish_HUD.MarkerPackAssistant
{

    [Export(typeof(Module))]
    public class MarkerPackAssistant : Module
    {

        // Fake Singleton
        public static MarkerPackAssistant Instance { get; private set; }
        public ModuleParameters Module { get; private set; }

        private SettingCollection _settingsCollection;

        public readonly Logger Logger = Logger.GetLogger(typeof(MarkerPackAssistant));

        [ImportingConstructor]
        public MarkerPackAssistant([Import("ModuleParameters")] ModuleParameters moduleParameters) : base(moduleParameters)
        {

            // Init
            Module = moduleParameters;
            Instance = this;

        }

        protected override void Initialize()
        {

            // Load managers
            _ = WindowManager.Instance;
            _ = ModuleSettingsManager.Instance;

        }

        protected override void DefineSettings(SettingCollection settings)
        {
            _settingsCollection = settings;

            // Send the settings to the module settings manager
            ModuleSettingsManager.Instance.DefineSettings(settings);

        }

        protected override void Unload()
        {

            // Unload windows
            WindowManager.Instance.Unload();
            ModuleSettingsManager.Instance.Unload();

            // Unload module instance
            Instance = null;

        }

        public override IView GetSettingsView()
        {
            return new SettingsView(_settingsCollection ?? new SettingCollection());
        }

    }

}