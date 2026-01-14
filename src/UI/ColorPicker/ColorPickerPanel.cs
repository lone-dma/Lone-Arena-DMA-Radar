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
using LoneArenaDmaRadar.UI.Skia;

namespace LoneArenaDmaRadar.UI.ColorPicker
{
    /// <summary>
    /// Color Picker Panel for the ImGui-based Radar.
    /// </summary>
    internal static class ColorPickerPanel
    {
        // Panel-local state
        private static Vector3 _editingColor = Vector3.One;
        private static ColorPickerOption? _selectedOption;
        private static string _hexInput = "#FFFFFF";

        private static ArenaDmaConfig Config { get; } = Program.Config;

        /// <summary>
        /// Whether the color picker panel is open.
        /// </summary>
        public static bool IsOpen { get; set; }

        /// <summary>
        /// Initialize colors from config. Call once at startup.
        /// </summary>
        public static void Initialize()
        {
            // Add default colors for any missing entries
            foreach (var defaultColor in GetDefaultColors())
                Config.RadarColors.TryAdd(defaultColor.Key, defaultColor.Value);

            // Apply all colors from config
            SetAllColors(Config.RadarColors);
        }

        /// <summary>
        /// Returns all default color combinations for Radar.
        /// </summary>
        private static Dictionary<ColorPickerOption, string> GetDefaultColors()
        {
            return new()
            {
                [ColorPickerOption.LocalPlayer] = SKColors.Green.ToString(),
                [ColorPickerOption.FriendlyPlayer] = SKColors.LimeGreen.ToString(),
                [ColorPickerOption.EnemyPlayer] = SKColors.Red.ToString(),
                [ColorPickerOption.StreamerPlayer] = SKColors.MediumPurple.ToString(),
                [ColorPickerOption.BotPlayer] = SKColors.Yellow.ToString(),
                [ColorPickerOption.FocusedPlayer] = SKColors.Coral.ToString(),
                [ColorPickerOption.DeathMarker] = SKColors.Black.ToString(),
                [ColorPickerOption.Explosives] = SKColors.OrangeRed.ToString(),
            };
        }

        /// <summary>
        /// Apply all colors from the config dictionary.
        /// </summary>
        private static void SetAllColors(IReadOnlyDictionary<ColorPickerOption, string> colors)
        {
            foreach (var color in colors)
            {
                if (!SKColor.TryParse(color.Value, out var skColor))
                    continue;

                ApplyColorToSKPaints(color.Key, skColor);
            }
        }

        /// <summary>
        /// Draw the color picker panel.
        /// </summary>
        public static void Draw()
        {
            bool isOpen = IsOpen;
            if (!ImGui.Begin("Color Picker", ref isOpen, ImGuiWindowFlags.AlwaysAutoResize))
            {
                IsOpen = isOpen;
                ImGui.End();
                return;
            }
            IsOpen = isOpen;

            ImGui.Text("Select a color option to edit:");

            // Color options list
            if (ImGui.BeginListBox("##ColorOptions", new Vector2(250, 300)))
            {
                foreach (var option in Enum.GetValues<ColorPickerOption>())
                {
                    string name = GetFriendlyName(option);
                    bool isSelected = _selectedOption == option;

                    // Show color preview
                    var currentColor = GetCurrentColor(option);
                    ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(currentColor.X, currentColor.Y, currentColor.Z, 1f));

                    if (ImGui.Selectable($"● {name}", isSelected))
                    {
                        _selectedOption = option;
                        _editingColor = currentColor;
                        _hexInput = ColorToHex(currentColor);
                    }

                    ImGui.PopStyleColor();
                }
                ImGui.EndListBox();
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Select a UI element to customize its color");

            ImGui.Separator();

            // Color editor
            if (_selectedOption.HasValue)
            {
                ImGui.Text($"Editing: {GetFriendlyName(_selectedOption.Value)}");

                // Color picker
                if (ImGui.ColorPicker3("##ColorPicker", ref _editingColor))
                {
                    _hexInput = ColorToHex(_editingColor);
                }

                // Hex input
                ImGui.SetNextItemWidth(100);
                if (ImGui.InputText("Hex", ref _hexInput, 10))
                {
                    if (TryParseHex(_hexInput, out var parsed))
                    {
                        _editingColor = parsed;
                    }
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Enter color as hex code (e.g., #FF0000)");

                ImGui.Spacing();

                if (ImGui.Button("Apply"))
                {
                    ApplyColor(_selectedOption.Value, _editingColor);
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Apply the selected color");
                ImGui.SameLine();
                if (ImGui.Button("Reset to Default"))
                {
                    _editingColor = GetDefaultColor(_selectedOption.Value);
                    _hexInput = ColorToHex(_editingColor);
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Reset to the default color");
            }

            ImGui.End();
        }

        private static string GetFriendlyName(ColorPickerOption option)
        {
            return option switch
            {
                ColorPickerOption.LocalPlayer => "Local Player",
                ColorPickerOption.FriendlyPlayer => "Teammate",
                ColorPickerOption.EnemyPlayer => "Enemy",
                ColorPickerOption.StreamerPlayer => "Streamer",
                ColorPickerOption.BotPlayer => "Bot",
                ColorPickerOption.FocusedPlayer => "Focused",
                ColorPickerOption.DeathMarker => "Death Marker",
                ColorPickerOption.Explosives => "Explosives",
                _ => option.ToString()
            };
        }

        private static Vector3 GetCurrentColor(ColorPickerOption option)
        {
            if (Config.RadarColors.TryGetValue(option, out var hex))
            {
                if (TryParseHex(hex, out var color))
                    return color;
            }
            return GetDefaultColor(option);
        }

        private static Vector3 GetDefaultColor(ColorPickerOption option)
        {
            SKColor color = option switch
            {
                ColorPickerOption.LocalPlayer => SKColors.Green,
                ColorPickerOption.FriendlyPlayer => SKColors.LimeGreen,
                ColorPickerOption.EnemyPlayer => SKColors.Red,
                ColorPickerOption.StreamerPlayer => SKColors.MediumPurple,
                ColorPickerOption.BotPlayer => SKColors.Yellow,
                ColorPickerOption.FocusedPlayer => SKColors.Coral,
                ColorPickerOption.DeathMarker => SKColors.Black,
                ColorPickerOption.Explosives => SKColors.OrangeRed,
                _ => SKColors.White
            };

            return new Vector3(color.Red / 255f, color.Green / 255f, color.Blue / 255f);
        }

        private static void ApplyColor(ColorPickerOption option, Vector3 color)
        {
            string hex = ColorToHex(color);
            Config.RadarColors[option] = hex;

            // Apply to SKPaint
            var skColor = new SKColor(
                (byte)(color.X * 255),
                (byte)(color.Y * 255),
                (byte)(color.Z * 255));

            ApplyColorToSKPaints(option, skColor);
        }

        private static void ApplyColorToSKPaints(ColorPickerOption option, SKColor skColor)
        {
            switch (option)
            {
                case ColorPickerOption.LocalPlayer:
                    SKPaints.PaintLocalPlayer.Color = skColor;
                    SKPaints.TextLocalPlayer.Color = skColor;
                    SKPaints.PaintAimviewWidgetLocalPlayer.Color = skColor;
                    break;
                case ColorPickerOption.FriendlyPlayer:
                    SKPaints.PaintTeammate.Color = skColor;
                    SKPaints.TextTeammate.Color = skColor;
                    SKPaints.PaintAimviewWidgetTeammate.Color = skColor;
                    break;
                case ColorPickerOption.EnemyPlayer:
                    SKPaints.PaintPlayer.Color = skColor;
                    SKPaints.TextPlayer.Color = skColor;
                    SKPaints.PaintAimviewWidgetPlayer.Color = skColor;
                    break;
                case ColorPickerOption.StreamerPlayer:
                    SKPaints.PaintStreamer.Color = skColor;
                    SKPaints.TextStreamer.Color = skColor;
                    SKPaints.PaintAimviewWidgetStreamer.Color = skColor;
                    break;
                case ColorPickerOption.BotPlayer:
                    SKPaints.PaintBot.Color = skColor;
                    SKPaints.TextBot.Color = skColor;
                    SKPaints.PaintAimviewWidgetBot.Color = skColor;
                    break;
                case ColorPickerOption.FocusedPlayer:
                    SKPaints.PaintFocused.Color = skColor;
                    SKPaints.TextFocused.Color = skColor;
                    SKPaints.PaintAimviewWidgetFocused.Color = skColor;
                    break;
                case ColorPickerOption.DeathMarker:
                    SKPaints.PaintDeathMarker.Color = skColor;
                    break;
                case ColorPickerOption.Explosives:
                    SKPaints.PaintExplosives.Color = skColor;
                    break;
            }
        }

        private static string ColorToHex(Vector3 color)
        {
            int r = (int)(color.X * 255);
            int g = (int)(color.Y * 255);
            int b = (int)(color.Z * 255);
            return $"#{r:X2}{g:X2}{b:X2}";
        }

        private static bool TryParseHex(string hex, out Vector3 color)
        {
            color = Vector3.One;
            if (string.IsNullOrEmpty(hex))
                return false;

            hex = hex.TrimStart('#');
            if (hex.Length != 6)
                return false;

            try
            {
                int r = Convert.ToInt32(hex.Substring(0, 2), 16);
                int g = Convert.ToInt32(hex.Substring(2, 2), 16);
                int b = Convert.ToInt32(hex.Substring(4, 2), 16);
                color = new Vector3(r / 255f, g / 255f, b / 255f);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
