/*
 * Lone Arena DMA Radar - Copyright (c) 2026 Lone DMA
 * Licensed under GNU AGPLv3. See https://www.gnu.org/licenses/agpl-3.0.html
 */

namespace LoneArenaDmaRadar.UI.Maps
{
    /// <summary>
    /// Defines a .JSON Map Config File
    /// </summary>
    public sealed class EftMapConfig
    {
        /// <summary>
        /// Map ID(s) for this Map.
        /// </summary>
        [JsonPropertyName("mapID")]
        public List<string> MapID { get; set; }
        /// <summary>
        /// Name of map (Ex: CUSTOMS)
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
        /// <summary>
        /// Bitmap 'X' Coordinate of map 'Origin Location' (where Unity X is 0).
        /// </summary>
        [JsonPropertyName("x")]
        public float X { get; set; }
        /// <summary>
        /// Bitmap 'Y' Coordinate of map 'Origin Location' (where Unity Y is 0).
        /// </summary>
        [JsonPropertyName("y")]
        public float Y { get; set; }
        /// <summary>
        /// Arbitrary scale value to align map scale between the Bitmap and Game Coordinates.
        /// </summary>
        [JsonPropertyName("scale")]
        public float Scale { get; set; }
        /// <summary>
        /// How much to scale up the original SVG Image during rasterization.
        /// </summary>
        [JsonPropertyName("rasterScale")]
        public float RasterScale { get; set; }
        /// <summary>
        /// TRUE if the map drawing should not dim layers, otherwise FALSE if dimming is permitted.
        /// This is a global setting that applies to all layers.
        /// </summary>
        [JsonPropertyName("disableDimming")]
        public bool DisableDimming { get; set; }
        /// <summary>
        /// Contains the Map Layers to load for the current Map Configuration.
        /// </summary>
        [JsonPropertyName("mapLayers")]
        public List<Layer> MapLayers { get; set; }

        /// <summary>
        /// A single layer of a Multi-Layered Map.
        /// </summary>
        public sealed class Layer
        {
            /// <summary>
            /// Minimum height (Unity Y Coord) for this map layer.
            /// NULL: No minimum height.
            /// </summary>
            [JsonPropertyName("minHeight")]
            public float? MinHeight { get; set; }
            /// <summary>
            /// Maximum height (Unity Y Coord) for this map layer.
            /// NULL: No maximum height.
            /// </summary>
            [JsonPropertyName("maxHeight")]
            public float? MaxHeight { get; set; }
            /// <summary>
            /// TRUE if when this layer is in the foreground, the lower layers cannot be dimmed. Otherwise FALSE.
            /// </summary>
            [JsonPropertyName("cannotDimLowerLayers")]
            public bool CannotDimLowerLayers { get; set; }
            /// <summary>
            /// Relative File path to this map layer's PNG Image.
            /// </summary>
            [JsonPropertyName("filename")]
            public string Filename { get; set; }
        }
    }
}
