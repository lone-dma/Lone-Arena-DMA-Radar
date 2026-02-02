/*
 * Lone Arena DMA Radar - Copyright (c) 2026 Lone DMA
 * Licensed under GNU AGPLv3. See https://www.gnu.org/licenses/agpl-3.0.html
 */

using LoneArenaDmaRadar.UI.ColorPicker;
using LoneArenaDmaRadar.UI.Maps;
using LoneArenaDmaRadar.Web.TarkovDev;
using VmmSharpEx.Extensions.Input;

namespace LoneArenaDmaRadar.Misc.JSON
{
    /// <summary>
    /// AOT-compatible JSON serializer context for the application's configuration types.
    /// </summary>
    [JsonSourceGenerationOptions(
        WriteIndented = false,
        PropertyNameCaseInsensitive = true,
        AllowTrailingCommas = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        ReadCommentHandling = JsonCommentHandling.Skip,
        Converters = [
            typeof(Vector2JsonConverter),
            typeof(Vector3JsonConverter),
            typeof(ColorDictionaryConverter),
            typeof(SKRectJsonConverter)
        ])]
    // Main config
    [JsonSerializable(typeof(ArenaDmaConfig))]
    // Sub-configs
    [JsonSerializable(typeof(DMAConfig))]
    [JsonSerializable(typeof(UIConfig))]
    [JsonSerializable(typeof(AimviewWidgetConfig))]
    // Enums
    [JsonSerializable(typeof(ColorPickerOption))]
    [JsonSerializable(typeof(Win32VirtualKey))]
    // Dictionary types
    [JsonSerializable(typeof(ConcurrentDictionary<Win32VirtualKey, string>))]
    [JsonSerializable(typeof(ConcurrentDictionary<ColorPickerOption, string>))]
    [JsonSerializable(typeof(ConcurrentDictionary<string, byte>))]
    [JsonSerializable(typeof(ConcurrentDictionary<int, int>))]
    [JsonSerializable(typeof(ConcurrentDictionary<int, byte>))]
    // SkiaSharp types
    [JsonSerializable(typeof(SKSize))]
    [JsonSerializable(typeof(SKRect))]
    // System.Numerics types
    [JsonSerializable(typeof(Vector2))]
    [JsonSerializable(typeof(Vector3))]
    // Collection types
    [JsonSerializable(typeof(HashSet<string>))]
    [JsonSerializable(typeof(Dictionary<string, string>))]
    // Primitive types (for converters)
    [JsonSerializable(typeof(byte))]
    // Map config types
    [JsonSerializable(typeof(EftMapConfig))]
    [JsonSerializable(typeof(EftMapConfig.Layer))]
    [JsonSerializable(typeof(List<EftMapConfig.Layer>))]
    [JsonSerializable(typeof(List<string>))]
    // TarkovDev API types
    [JsonSerializable(typeof(TarkovMarketItem))]
    // TarkovDev list types
    [JsonSerializable(typeof(List<TarkovMarketItem>))]
    // Misc
    [JsonSerializable(typeof(JsonDocument))]
    public partial class AppJsonContext : JsonSerializerContext
    {
    }
}
