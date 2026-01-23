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
