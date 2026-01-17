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
