using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kudos.WebApi.JsonSerialization.Converters;

public class DateTimeConverter : JsonConverter<DateTime>
{
    private const string DefaultDateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffK";

    public override DateTime Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return DateTime.Parse(reader.GetString() ?? string.Empty, CultureInfo.InvariantCulture);
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateTime value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Kind switch
        {
            DateTimeKind.Utc => value.ToString(DefaultDateTimeFormat),
            DateTimeKind.Local => value.ToUniversalTime().ToString(DefaultDateTimeFormat),
            DateTimeKind.Unspecified => new DateTime(value.Ticks, DateTimeKind.Utc).ToString(DefaultDateTimeFormat),
            _ => new DateTime(value.Ticks, DateTimeKind.Utc).ToString(DefaultDateTimeFormat)
        });
    }
}