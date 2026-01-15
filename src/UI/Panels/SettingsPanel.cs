/*
 * Lone EFT DMA Radar
 * Brought to you by Lone (Lone DMA)
 * 
MIT License

Copyright (c) 2025 Lone DMA

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 *
*/

using ImGuiNET;
using LoneArenaDmaRadar.Misc.JSON;
using LoneArenaDmaRadar.UI.ColorPicker;
using LoneArenaDmaRadar.UI.Hotkeys;
using LoneArenaDmaRadar.UI.Skia;

namespace LoneArenaDmaRadar.UI.Panels
{
    /// <summary>
    /// Settings Panel for the ImGui-based Radar.
    /// </summary>
    internal static class SettingsPanel
    {
        // Panel-local state for tracking window open/close
        private static bool _isOpen;

        private static ArenaDmaConfig Config { get; } = Program.Config;

        /// <summary>
        /// Whether the settings panel is open.
        /// </summary>
        public static bool IsOpen
        {
            get => _isOpen;
            set => _isOpen = value;
        }

        /// <summary>
        /// Draw the settings panel.
        /// </summary>
        public static void Draw()
        {
            bool isOpen = _isOpen;
            if (!ImGui.Begin("Settings", ref isOpen, ImGuiWindowFlags.AlwaysAutoResize))
            {
                _isOpen = isOpen;
                ImGui.End();
                return;
            }
            _isOpen = isOpen;

            if (ImGui.BeginTabBar("SettingsTabs"))
            {
                DrawGeneralTab();
                DrawAboutTab();

                ImGui.EndTabBar();
            }

            ImGui.End();
        }

        private static void DrawGeneralTab()
        {
            if (ImGui.BeginTabItem("General"))
            {
                ImGui.SeparatorText("Tools");

                if (ImGui.Button("Hotkey Manager"))
                {
                    HotkeyManagerPanel.IsOpen = true;
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Configure keyboard hotkeys for radar functions");
                ImGui.SameLine();
                if (ImGui.Button("Color Picker"))
                {
                    ColorPickerPanel.IsOpen = true;
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Customize colors for players, loot, and UI elements");
                ImGui.SameLine();
                if (ImGui.Button("Map Setup Helper##btn"))
                {
                    MapSetupHelperPanel.IsOpen = true;
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Adjust map calibration settings (X, Y, Scale)");

                ImGui.Separator();

                if (ImGui.Button("Restart Radar"))
                {
                    Memory.RestartRadar();
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Restart the radar memory reader");
                ImGui.SameLine();
                if (ImGui.Button("Backup Config"))
                {
                    BackupConfig();
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Create a backup of your current configuration");
                ImGui.SameLine();
                if (ImGui.Button("Open Config Folder"))
                {
                    OpenConfigFolder();
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Open the folder containing configuration files");

                ImGui.SeparatorText("Display Settings");

                // UI Scale
                float uiScale = Config.UI.UIScale;
                if (ImGui.SliderFloat("UI Scale", ref uiScale, 0.5f, 2.0f, "%.1f"))
                {
                    Config.UI.UIScale = uiScale;
                    UpdateUIScale(uiScale);
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Scale UI elements (text, icons, widgets)");

                // Zoom
                int zoom = Config.UI.Zoom;
                if (ImGui.SliderInt("Zoom (F1/F2)", ref zoom, 1, 200))
                {
                    Config.UI.Zoom = zoom;
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Map zoom level (lower = more zoomed in)");

                // Aimline Length
                int aimlineLength = Config.UI.AimLineLength;
                if (ImGui.SliderInt("Aimline Length", ref aimlineLength, 0, 1500))
                {
                    Config.UI.AimLineLength = aimlineLength;
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Length of player aim direction lines");

                ImGui.SeparatorText("Widgets");

                bool aimviewWidget = Config.AimviewWidget.Enabled;
                if (ImGui.Checkbox("Aimview Widget", ref aimviewWidget))
                {
                    Config.AimviewWidget.Enabled = aimviewWidget;
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("3D view showing players in your field of view");

                ImGui.SeparatorText("Visibility");

                bool highAlert = Config.UI.HighAlert;
                if (ImGui.Checkbox("High Alert", ref highAlert))
                {
                    Config.UI.HighAlert = highAlert;
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Show aimlines when other players aim at you.");

                ImGui.EndTabItem();
            }
        }

        private static void DrawAboutTab()
        {
            if (ImGui.BeginTabItem("About"))
            {
                ImGui.Text(Program.Name);
                ImGui.Separator();
                ImGui.TextWrapped("A DMA-based radar for Escape From Tarkov Arena.");

                ImGui.Spacing();
                if (ImGui.Button("Visit Website"))
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo("https://lone-dma.org/") { UseShellExecute = true });
                    }
                    catch { }
                }

                ImGui.EndTabItem();
            }
        }

        #region Helper Methods

        private static void BackupConfig()
        {
            try
            {
                var backupFile = Path.Combine(Program.ConfigPath.FullName, $"{ArenaDmaConfig.Filename}.userbak");
                File.WriteAllText(backupFile, JsonSerializer.Serialize(Program.Config, AppJsonContext.Default.ArenaDmaConfig));
                MessageBox.Show(RadarWindow.Handle, $"Backed up to {backupFile}", "Backup Config", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(RadarWindow.Handle, $"Error: {ex.Message}", "Backup Config", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static void OpenConfigFolder()
        {
            try
            {
                Process.Start(new ProcessStartInfo(Program.ConfigPath.FullName) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show(RadarWindow.Handle, $"Error: {ex.Message}", "Open Config", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void UpdateUIScale(float newScale)
        {
            // Update Paints
            SKPaints.TextOutline.StrokeWidth = 2f * newScale;
            SKPaints.PaintLocalPlayer.StrokeWidth = 1.66f * newScale;
            SKPaints.PaintTeammate.StrokeWidth = 1.66f * newScale;
            SKPaints.PaintPlayer.StrokeWidth = 1.66f * newScale;
            SKPaints.PaintBot.StrokeWidth = 1.66f * newScale;
            SKPaints.PaintFocused.StrokeWidth = 1.66f * newScale;
            SKPaints.PaintDeathMarker.StrokeWidth = 3f * newScale;
            SKPaints.PaintTransparentBacker.StrokeWidth = 0.25f * newScale;
            SKPaints.PaintExplosives.StrokeWidth = 3f * newScale;

            // Fonts
            SKFonts.UIRegular.Size = 12f * newScale;
            SKFonts.UILarge.Size = 48f * newScale;
        }

        #endregion
    }
}
