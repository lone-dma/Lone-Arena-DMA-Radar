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

namespace LoneArenaDmaRadar.UI.Panels
{
    /// <summary>
    /// Radar Overlay Panel - Quick access controls shown over the radar.
    /// </summary>
    internal static class RadarOverlayPanel
    {
        /// <summary>
        /// Draw the overlay controls at the top of the radar.
        /// </summary>
        public static void DrawTopBar()
        {
            // Static position in top-left, below menu bar
            ImGui.SetNextWindowPos(new Vector2(10, 25), ImGuiCond.Always);
            ImGui.SetNextWindowBgAlpha(0.7f);

            var flags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.AlwaysAutoResize |
                        ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoSavedSettings;

            if (ImGui.Begin("RadarTopBar", flags))
            {
                // Map Free Toggle Button
                bool isMapFree = RadarWindow.IsMapFreeEnabled;
                if (isMapFree)
                {
                    ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.2f, 0.6f, 0.2f, 1.0f));
                    ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(0.3f, 0.7f, 0.3f, 1.0f));
                    ImGui.PushStyleColor(ImGuiCol.ButtonActive, new Vector4(0.1f, 0.5f, 0.1f, 1.0f));
                }

                if (ImGui.Button(isMapFree ? "Map Free: ON" : "Map Free: OFF"))
                {
                    RadarWindow.IsMapFreeEnabled = !isMapFree;
                    if (isMapFree) // Was on, now turning off
                    {
                        RadarWindow.MapPanPosition = Vector2.Zero;
                    }
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Toggle free map panning (drag to move map)");

                if (isMapFree)
                {
                    ImGui.PopStyleColor(3);
                }
                ImGui.End();
            }
        }

        /// <summary>
        /// Draw the map setup helper overlay.
        /// </summary>
        public static void DrawMapSetupHelper()
        {
            bool mapSetupVisible = MapSetupHelperPanel.ShowOverlay;
            if (!mapSetupVisible)
                return;

            var io = ImGui.GetIO();
            ImGui.SetNextWindowPos(new Vector2(io.DisplaySize.X - 310, 10), ImGuiCond.Appearing);
            ImGui.SetNextWindowSize(new Vector2(300, 80), ImGuiCond.Appearing);
            ImGui.SetNextWindowBgAlpha(0.8f);

            if (ImGui.Begin("Map Setup Helper", ref mapSetupVisible, ImGuiWindowFlags.NoSavedSettings))
            {
                ImGui.Text("Current Coordinates:");
                ImGui.TextColored(new Vector4(0.2f, 0.8f, 0.2f, 1f), MapSetupHelperPanel.Coords);
            }
            ImGui.End();
            MapSetupHelperPanel.ShowOverlay = mapSetupVisible;
        }
    }
}
