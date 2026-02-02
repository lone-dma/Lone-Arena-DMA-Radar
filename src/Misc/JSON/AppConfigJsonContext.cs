/*
 * Lone Arena DMA Radar - Copyright (c) 2026 Lone DMA
 * Licensed under GNU AGPLv3. See https://www.gnu.org/licenses/agpl-3.0.html
 */

using LoneArenaDmaRadar.UI.ColorPicker;

namespace LoneArenaDmaRadar.Misc.JSON
{
    [JsonSourceGenerationOptions(
    WriteIndented = true,
    Converters = [
        typeof(Vector2JsonConverter),
            typeof(Vector3JsonConverter),
            typeof(ColorDictionaryConverter),
            typeof(SKRectJsonConverter)
    ])]
    [JsonSerializable(typeof(ArenaDmaConfig))]
    public partial class AppConfigJsonContext : JsonSerializerContext
    {
    }
}
