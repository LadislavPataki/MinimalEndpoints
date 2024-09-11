using System.Text.Json;

namespace Kudos.WebApi.JsonSerialization.Extensions;

public static class JsonSerializerExtensions
{
    private static readonly JsonSerializerOptions SerializerOptions = GetDefaultOptions();

    private static JsonSerializerOptions GetDefaultOptions()
    {
        var serializerOptions = new JsonSerializerOptions();
        serializerOptions.ConfigureDefaultOptions();

        return serializerOptions;
    }

    public static T? Deserialize<T>(this string json) =>
        JsonSerializer.Deserialize<T>(json, SerializerOptions);

    public static T? Deserialize<T>(this string json, JsonSerializerOptions serializerOptions) =>
        JsonSerializer.Deserialize<T>(json, serializerOptions);

    public static string Serialize<TValue>(this TValue value) =>
        JsonSerializer.Serialize(value, SerializerOptions);

    public static string Serialize<TValue>(this TValue value, JsonSerializerOptions serializerOptions) =>
        JsonSerializer.Serialize(value, serializerOptions);

    public static JsonElement SerializeToElement<TValue>(this TValue value) =>
        JsonSerializer.SerializeToElement(value, SerializerOptions);

    public static JsonElement SerializeToElement<TValue>(this TValue value, JsonSerializerOptions serializerOptions) =>
        JsonSerializer.SerializeToElement(value, serializerOptions);
}