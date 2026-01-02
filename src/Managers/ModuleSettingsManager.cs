using Blish_HUD.Input;
using Blish_HUD.Settings;
using HexedHero.Blish_HUD.MarkerPackAssistant.Objects;
using Microsoft.Xna.Framework.Input;
using System;

namespace HexedHero.Blish_HUD.MarkerPackAssistant.Managers
{
    public class ModuleSettingsManager
    {

        // Singleton
        private static Lazy<ModuleSettingsManager> instance = new Lazy<ModuleSettingsManager>(() => new ModuleSettingsManager());
        public static ModuleSettingsManager Instance {
            get {
                if (instance == null) {
                    instance = new Lazy<ModuleSettingsManager>(() => new ModuleSettingsManager());
                }
                return instance.Value;
            }
        }

        public SettingCollection Settings { get; private set; }
        public ModuleSettings ModuleSettings { get; private set; }

        private ModuleSettingsManager()
        {

            Load();

        }

        private void Load() { }

        public void Unload()
        {
            this.KeyBindCopyMap.Value.Enabled   = false;
            this.KeyBindCopyXYZ.Value.Enabled   = false;
            this.KeyBindCopyGUID.Value.Enabled  = false;
            this.KeyBindCopyPOI.Value.Enabled   = false;
            this.KeyBindRun.Value.Enabled       = false;

            this.KeyBindCopyMap.Value.Activated     -= TriggerCopyMap;
            this.KeyBindCopyXYZ.Value.Activated     -= TriggerCopyXYZ;
            this.KeyBindCopyGUID.Value.Activated    -= TriggerCopyGUID;
            this.KeyBindCopyPOI.Value.Activated     -= TriggerCopyPOI;
            this.KeyBindRun.Value.Activated         -= TriggerRun;

            // Reset instance
            instance = null;

        }

        public void DefineSettings(SettingCollection settings)
        {
            ModuleSettings = new ModuleSettings(settings);
            InitKeyBindSettings(settings);

        }

        #region Keybind Settings

        public SettingEntry<KeyBinding> KeyBindCopyMap { get; private set; }
        public SettingEntry<KeyBinding> KeyBindCopyXYZ { get; private set; }
        public SettingEntry<KeyBinding> KeyBindCopyGUID { get; private set; }
        public SettingEntry<KeyBinding> KeyBindCopyPOI { get; private set; }
        public SettingEntry<KeyBinding> KeyBindRun { get; private set; }

        private void InitKeyBindSettings(SettingCollection settings)
        {
            this.KeyBindCopyMap     = settings.DefineSetting(nameof(this.KeyBindCopyMap),   new KeyBinding(ModifierKeys.Shift | ModifierKeys.Alt, Keys.Q), () => "Copy Map ID",          () => "");
            this.KeyBindCopyXYZ     = settings.DefineSetting(nameof(this.KeyBindCopyXYZ),   new KeyBinding(ModifierKeys.Shift | ModifierKeys.Alt, Keys.W), () => "Copy XYZ Coordinates", () => "");
            this.KeyBindCopyGUID    = settings.DefineSetting(nameof(this.KeyBindCopyGUID),  new KeyBinding(ModifierKeys.Shift | ModifierKeys.Alt, Keys.E), () => "Copy New GUID",        () => "");
            this.KeyBindCopyPOI     = settings.DefineSetting(nameof(this.KeyBindCopyPOI),   new KeyBinding(ModifierKeys.Shift | ModifierKeys.Alt, Keys.R), () => "Copy POI",             () => "");
            this.KeyBindRun         = settings.DefineSetting(nameof(this.KeyBindRun),       new KeyBinding(ModifierKeys.Shift | ModifierKeys.Alt, Keys.T), () => "Run .bat",             () => "");

            HandleKeybinds();
        }

        private void HandleKeybinds()
        {
            this.KeyBindCopyMap.Value.Enabled   = true;
            this.KeyBindCopyXYZ.Value.Enabled   = true;
            this.KeyBindCopyGUID.Value.Enabled  = true;
            this.KeyBindCopyPOI.Value.Enabled   = true;
            this.KeyBindRun.Value.Enabled       = true;

            this.KeyBindCopyMap.Value.Activated     += TriggerCopyMap;
            this.KeyBindCopyXYZ.Value.Activated     += TriggerCopyXYZ;
            this.KeyBindCopyGUID.Value.Activated    += TriggerCopyGUID;
            this.KeyBindCopyPOI.Value.Activated     += TriggerCopyPOI;
            this.KeyBindRun.Value.Activated         += TriggerRun;
        }

        private void TriggerCopyMap(object sender, EventArgs e) {
            _ = WindowManager.Instance.AssistanceView.CopyMapID();
        }

        private void TriggerCopyXYZ(object sender, EventArgs e) {
            _ = WindowManager.Instance.AssistanceView.CopyCords();
        }

        private void TriggerCopyGUID(object sender, EventArgs e) {
           _ = WindowManager.Instance.AssistanceView.CopyRandomGUID();
        }

        private void TriggerCopyPOI(object sender, EventArgs e) {
            _ = WindowManager.Instance.AssistanceView.CopyPOI();
        }

        private void TriggerRun(object sender, EventArgs e) {
            _ = WindowManager.Instance.AssistanceView.RunBat();
        }

        #endregion

    }

}
