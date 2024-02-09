using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using HexedHero.Blish_HUD.MarkerPackAssistant.Objects;
using HexedHero.Blish_HUD.MarkerPackAssistant.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HexedHero.Blish_HUD.MarkerPackAssistant.Managers
{
    public class WindowManager
    {

        // Singleton
        private static Lazy<WindowManager> instance = new Lazy<WindowManager>(() => new WindowManager());
        public static WindowManager Instance {
            get {
                if (instance == null) {
                    instance = new Lazy<WindowManager>(() => new WindowManager());
                }
                return instance.Value;
            }
        }

        public StandardWindow MainWindow { get; private set; }
        public AssistanceView AssistanceView { get; private set; }

        private CornerIcon cornerIcon;

        private Texture2D iconTexture;

        private AsyncTexture2D emblemTexture;
        private AsyncTexture2D backgroundTexture;

        private WindowManager()
        {

            Load();

        }

        private void Load()
        {

            // Load needed textures
            iconTexture = MarkerPackAssistant.Instance.Module.ContentsManager.GetTexture("102348_modified.png");
            emblemTexture = AsyncTexture2D.FromAssetId(102348);
            backgroundTexture = AsyncTexture2D.FromAssetId(155985);

            // Make corner icon
            cornerIcon = new CornerIcon()
            {

                Icon = iconTexture,
                BasicTooltipText = "Marker Pack Assistant",
                Priority = 89157212,
                Parent = GameService.Graphics.SpriteScreen

            };

            cornerIcon.Click += delegate { MainWindow.ToggleWindow(AssistanceView); };

            // Make main window
            MainWindow = new StandardWindow(
                    ContentService.Textures.TransparentPixel, // See Below
                    new Rectangle(5, 5, 310, 220), // Window
                    new Rectangle(30, 30, 260, 180) // Content
                )
            {

                Emblem = ContentService.Textures.TransparentPixel, // See Below
                Title = "MPA",
                Location = new Point(100, 100),
                SavesPosition = true,
                Id = "MarkerPackAssistantMainWindow",
                Parent = GameService.Graphics.SpriteScreen,

            };

            // Add the background - Check if the texture was loaded by Blish or another module or this module at a different runtime else run the injection when it is loaded.
            void injectBackground() => Reflection.InjectNewBackground(MainWindow, backgroundTexture, new Rectangle(-10, 30, 340, 320)); // TODO fix -10
            if (backgroundTexture.HasSwapped) { injectBackground(); } else { backgroundTexture.TextureSwapped += delegate { injectBackground(); }; }

            // Add the Emblem - Emblem doesn't have AsyncTexture2D support so we need to set it later, same issue as the background
            void injectEmblem() => MainWindow.Emblem = emblemTexture;
            if (emblemTexture.HasSwapped) { injectEmblem(); } else { emblemTexture.TextureSwapped += delegate { injectEmblem(); }; }

            // Add view
            AssistanceView = new AssistanceView();

        }

        public void Unload()
        {

            // Icon
            cornerIcon?.Dispose();

            // Windows
            MainWindow?.Dispose();
            AssistanceView?.DoUnload();
            AssistanceView = null;

            // Textures
            iconTexture?.Dispose();

            // Reset instance
            instance = null;

        }

    }

}
