using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using System.Threading;
using Blish_HUD.Content;
using HexedHero.Blish_HUD.MarkerPackAssistant.Managers;
using System.Diagnostics;
using System.IO;
using System;
using HexedHero.Blish_HUD.MarkerPackAssistant.Utils;
using Blish_HUD.Modules;
using File = System.IO.File;

namespace HexedHero.Blish_HUD.MarkerPackAssistant.Objects
{

    public class AssistanceView : View
    {

        private CancellationTokenSource UpdaterTaskToken = new CancellationTokenSource();

        private int MapID = 0;
        private float CharX = 0;
        private float CharY = 0;
        private float CharZ = 0;

        private FlowPanel GlobalFlowPanel;

        private Label StatsLabel;

        private FlowPanel MapIDFlowPanel;
        private Label MapIDLabel;
        private Image MapIDImage;
        private StandardButton MapIDButton;

        private FlowPanel CordsFlowPanel;
        private Label CordsLabel;
        private Image CordsImage;
        private StandardButton CordsButton;

        private Label ToolsLabel;

        private FlowPanel RunFlowPanel;
        private TextBox RunTextBox;
        private Image RunImage;
        private StandardButton RunButton;

        private FlowPanel RandomGUIDFlowPanel;
        private Label RandomGUIDLabel;
        private Image RandomGUIDImage;
        private StandardButton RandomGUIDButton;

        private FlowPanel POIFlowPanel;
        private Label POILabel;
        private Image POIImage;
        private StandardButton POIButton;

        public AssistanceView() { }

        protected override void Unload()
        {

            GlobalFlowPanel?.Dispose();
            StatsLabel?.Dispose();
            MapIDFlowPanel?.Dispose();
            MapIDLabel?.Dispose();
            MapIDImage?.Dispose();
            MapIDButton?.Dispose();
            CordsFlowPanel?.Dispose();
            CordsLabel?.Dispose();
            CordsImage?.Dispose();
            CordsButton?.Dispose();
            ToolsLabel?.Dispose();
            RunFlowPanel?.Dispose();
            RunTextBox?.Dispose();
            RunImage?.Dispose();
            RunButton?.Dispose();
            RandomGUIDFlowPanel?.Dispose();
            RandomGUIDLabel?.Dispose();
            RandomGUIDImage?.Dispose();
            RandomGUIDButton?.Dispose();
            POIFlowPanel?.Dispose();
            POILabel?.Dispose();
            POIImage?.Dispose();
            POIButton?.Dispose();
            UpdaterTaskToken.Cancel();

        }

        protected override void Build(Container container)
        {

            // Create task
            UpdaterTaskToken = new CancellationTokenSource();

            // Global flow panel
            GlobalFlowPanel = new FlowPanel()
            {

                FlowDirection = ControlFlowDirection.SingleTopToBottom,
                Size = new Point(260, 180),
                Parent = container

            };

            // Stats label
            StatsLabel = new Label()
            {

                Text = "Stats:",
                Location = new Point(0, 0),
                Size = new Point(75, 25),
                Parent = GlobalFlowPanel

            };

            // Map ID
            MapIDFlowPanel = new FlowPanel()
            {

                FlowDirection = ControlFlowDirection.SingleLeftToRight,
                Size = new Point(220, 25),
                Parent = GlobalFlowPanel

            };

            MapIDButton = new StandardButton()
            {

                Text = "Copy",
                Location = new Point(0, 0),
                Size = new Point(70, 25),
                Parent = MapIDFlowPanel

            };
            MapIDButton.Click += async delegate
            {

                MapIDButton.Text = "Copied";
                MapIDButton.Enabled = false;
                _ = ClipboardUtil.WindowsClipboardService.SetTextAsync(MapID.ToString());
                await Task.Delay(333);
                MapIDButton.Text = "Copy";
                MapIDButton.Enabled = true;

            };

            MapIDImage = new Image()
            {

                Location = new Point(0, 0),
                Size = new Point(25, 25),
                Texture = AsyncTexture2D.FromAssetId(716655),
                Parent = MapIDFlowPanel

            };

            MapIDLabel = new Label()
            {

                Text = "ERROR",
                Location = new Point(0, 0),
                Size = new Point(75, 25),
                Parent = MapIDFlowPanel

            };

            // Cords
            CordsFlowPanel = new FlowPanel()
            {

                FlowDirection = ControlFlowDirection.SingleLeftToRight,
                Size = new Point(260, 25),
                Parent = GlobalFlowPanel

            };

            CordsButton = new StandardButton()
            {

                Text = "Copy",
                Location = new Point(0, 0),
                Size = new Point(70, 25),
                Parent = CordsFlowPanel

            };
            CordsButton.Click += async delegate
            {

                CordsButton.Text = "Copied";
                CordsButton.Enabled = false;
                _ = ClipboardUtil.WindowsClipboardService.SetTextAsync($"xpos=\"{CharX}\" ypos=\"{CharY}\" zpos=\"{CharZ}\"");
                await Task.Delay(333);
                CordsButton.Text = "Copy";
                CordsButton.Enabled = true;

            };

            CordsImage = new Image()
            {

                Location = new Point(0, 0),
                Size = new Point(25, 25),
                Texture = AsyncTexture2D.FromAssetId(716655),
                Parent = CordsFlowPanel

            };

            CordsLabel = new Label()
            {

                Text = "ERROR",
                Location = new Point(0, 0),
                Size = new Point(200, 25),
                Parent = CordsFlowPanel

            };

            // Tools label
            ToolsLabel = new Label()
            {

                Text = "Tools:",
                Location = new Point(0, 0),
                Size = new Point(75, 25),
                Parent = GlobalFlowPanel

            };

            // Run
            RunFlowPanel = new FlowPanel()
            {

                FlowDirection = ControlFlowDirection.SingleLeftToRight,
                Size = new Point(260, 25),
                Parent = GlobalFlowPanel

            };

            RunButton = new StandardButton()
            {

                Text = "Run",
                Location = new Point(0, 0),
                Size = new Point(70, 25),
                Parent = RunFlowPanel

            };

            RunButton.Click += async delegate
            {

                RunButton.Text = "Running";
                RunButton.Enabled = false;

                await Task.Run(async () =>
                {

                    string path = ModuleSettingsManager.Instance.ModuleSettings.MarkerPackBuildPath.Value;

                    // Check if the path is to a valid .bat file
                    if (File.Exists(path) && Path.GetExtension(path).Equals(".bat"))
                    {

                        Process process = new Process();
                        process.StartInfo.WorkingDirectory = Path.GetDirectoryName(path);
                        process.StartInfo.FileName = path;
                        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        process.StartInfo.CreateNoWindow = true;
                        process.Start();

                        // Wait for the process to finish with a 30 second limit
                        if (!process.WaitForExit(30000))
                        {

                            process.Kill();
                            RunButton.Text = "KILLED";
                            await Task.Delay(3000);

                        }

                        // Tiny delay to ensure file paths are updated
                        await Task.Delay(25);

                        // Find the pathing module
                        foreach (ModuleManager moduleManager in GameService.Module.Modules)
                        {

                            if (moduleManager.Manifest.Namespace.ToLower().Equals("bh.community.pathing") && moduleManager.Enabled)
                            {

                                // Reload markers
                                Reflection.ReloadPathingMarkers(moduleManager);

                                // Let pathing update
                                await Task.Delay(250);

                            }

                        }

                    }
                    else
                    {

                        RunButton.Text = "INVALID";
                        await Task.Delay(1000);

                    }

                });

                RunButton.Text = "Run";
                RunButton.Enabled = true;

            };

            RunImage = new Image()
            {

                Location = new Point(0, 0),
                Size = new Point(25, 25),
                Texture = AsyncTexture2D.FromAssetId(716655),
                Parent = RunFlowPanel

            };

            RunTextBox = new TextBox()
            {

                Text = ModuleSettingsManager.Instance.ModuleSettings.MarkerPackBuildPath.Value,
                Font = GameService.Content.DefaultFont12,
                Location = new Point(0, 0),
                Size = new Point(160, 25),
                Parent = RunFlowPanel

            };
            RunTextBox.TextChanged += delegate
            {

                ModuleSettingsManager.Instance.ModuleSettings.MarkerPackBuildPath.Value = RunTextBox.Text;

            };

            // Random GUID
            RandomGUIDFlowPanel = new FlowPanel()
            {

                FlowDirection = ControlFlowDirection.SingleLeftToRight,
                Size = new Point(260, 25),
                Parent = GlobalFlowPanel

            };

            RandomGUIDButton = new StandardButton()
            {

                Text = "Copy",
                Location = new Point(0, 0),
                Size = new Point(70, 25),
                Parent = RandomGUIDFlowPanel

            };
            RandomGUIDButton.Click += async delegate
            {

                RandomGUIDButton.Text = "Copied";
                RandomGUIDButton.Enabled = false;
                _ = ClipboardUtil.WindowsClipboardService.SetTextAsync(Common.GetRandomGUID());
                await Task.Delay(333);
                RandomGUIDButton.Text = "Copy";
                RandomGUIDButton.Enabled = true;

            };

            RandomGUIDImage = new Image()
            {

                Location = new Point(0, 0),
                Size = new Point(25, 25),
                Texture = AsyncTexture2D.FromAssetId(716655),
                Parent = RandomGUIDFlowPanel

            };

            RandomGUIDLabel = new Label()
            {

                Text = "Random GUID (Base64)",
                Location = new Point(0, 0),
                Size = new Point(200, 25),
                Parent = RandomGUIDFlowPanel

            };

            // Create POI
            POIFlowPanel = new FlowPanel()
            {

                FlowDirection = ControlFlowDirection.SingleLeftToRight,
                Size = new Point(260, 25),
                Parent = GlobalFlowPanel

            };

            POIButton = new StandardButton()
            {

                Text = "Copy",
                Location = new Point(0, 0),
                Size = new Point(70, 25),
                Parent = POIFlowPanel

            };
            POIButton.Click += async delegate
            {

                POIButton.Text = "Copied";
                POIButton.Enabled = false;
                String Map = $"{MapID}";
                String Position = $"xpos=\"{CharX}\" ypos=\"{CharY}\" zpos=\"{CharZ}\"";
                String randomGUID = Common.GetRandomGUID();
                _ = ClipboardUtil.WindowsClipboardService.SetTextAsync($"<POI MapID=\"{Map}\" {Position} GUID=\"{randomGUID}\"/>"); // TODO format information
                await Task.Delay(333);
                POIButton.Text = "Copy";
                POIButton.Enabled = true;

            };

            POIImage = new Image()
            {

                Location = new Point(0, 0),
                Size = new Point(25, 25),
                Texture = AsyncTexture2D.FromAssetId(716655),
                Parent = POIFlowPanel

            };

            POILabel = new Label()
            {

                Text = "Create POI",
                Location = new Point(0, 0),
                Size = new Point(200, 25),
                Parent = POIFlowPanel

            };

            // Update all values
            UpdateMapID(null, null);
            UpdateCords(null, null);

            // Updating
            GameService.Gw2Mumble.CurrentMap.MapChanged += UpdateMapID;
            Task task = Task.Run(async () =>
            {

                while (!UpdaterTaskToken.Token.IsCancellationRequested)
                {

                    UpdateCords(null, null);
                    await Task.Delay(25, UpdaterTaskToken.Token);

                }

            });

        }

        public void UpdateMapID(object sender, ValueEventArgs<int> e)
        {

            MapID = GameService.Gw2Mumble.CurrentMap.Id;
            MapIDLabel.Text = "Map ID: %id%".Replace("%id%", MapID.ToString());

        }

        public void UpdateCords(object sender, ValueEventArgs<int> e)
        {

            CharX = GameService.Gw2Mumble.PlayerCharacter.Position.X;
            CharY = GameService.Gw2Mumble.PlayerCharacter.Position.Z; // ?
            CharZ = GameService.Gw2Mumble.PlayerCharacter.Position.Y; // ?
            CordsLabel.Text = "XYZ: %location%".Replace("%location%", CharX.ToString("F2") + ", " + CharY.ToString("F2") + ", " + CharZ.ToString("F2"));

        }

    }

}
