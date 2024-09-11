using System.Text.Json;
using System.Text.Json.Serialization;
using Kudos.WebApi.JsonSerialization.Converters;

namespace Kudos.WebApi.JsonSerialization.Extensions;

public static class JsonSerializerOptionsExtensions
{
    public static void ConfigureDefaultOptions(this JsonSerializerOptions serializerOptions)
    {
        serializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        serializerOptions.Converters.Add(new DateTimeConverter());
        serializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        serializerOptions.PropertyNameCaseInsensitive = true;
    }
}