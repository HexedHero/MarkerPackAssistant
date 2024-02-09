using Blish_HUD.Content;
using Blish_HUD.Controls;
using System.Reflection;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Module = Blish_HUD.Modules.Module;
using Blish_HUD.Modules;

namespace HexedHero.Blish_HUD.MarkerPackAssistant.Utils
{

    // Fixes an issue where you cannot update the window's background after its been set,
    // even if you you a AsyncTexture2D and swap the texture later and I need to resize the background
    // and cannot without making everything wait for the AsyncTexture2D to complete.
    // I'm sorry...
    public class Reflection
    {

        public static void InjectNewBackground(StandardWindow Window, AsyncTexture2D backgroundTexture, Rectangle Bounds)
        {

            InjectNewBackground(Window, (Texture2D)backgroundTexture, Bounds);

        }

        public static void InjectNewBackground(StandardWindow Window, Texture2D backgroundTexture, Rectangle Bounds)
        {

            Type baseType = Window.GetType().BaseType;

            // Force set the WindowBackground
            PropertyInfo windowBackgroundPropertyInfo = baseType.GetProperty("WindowBackground", BindingFlags.NonPublic | BindingFlags.Instance);
            windowBackgroundPropertyInfo.SetValue(Window, (AsyncTexture2D)backgroundTexture);

            // Update the background bounds
            InjectNewBackgroundBounds(Window, Bounds);

        }

        public static void InjectNewBackgroundBounds(StandardWindow Window, Rectangle Bounds)
        {

            Type baseType = Window.GetType().BaseType;

            // Force set the background image bounds
            PropertyInfo windowContainerPropertyInfo = baseType.GetProperty("BackgroundDestinationBounds", BindingFlags.NonPublic | BindingFlags.Instance);
            windowContainerPropertyInfo.SetValue(Window, Bounds);

        }

        public static void ReloadPathingMarkers(ModuleManager moduleManager)
        {

            // Don't even ask
            Module pathingModule = moduleManager.ModuleInstance;
            Type moduleType = pathingModule.GetType();
            PropertyInfo packInitiatorProperty = moduleType.GetProperty("PackInitiator");
            var packInitiator = packInitiatorProperty.GetValue(pathingModule);
            MethodInfo reloadPacksMethod = packInitiator.GetType().GetMethod("ReloadPacks");
            reloadPacksMethod.Invoke(packInitiator, null);

        }

    }
}
