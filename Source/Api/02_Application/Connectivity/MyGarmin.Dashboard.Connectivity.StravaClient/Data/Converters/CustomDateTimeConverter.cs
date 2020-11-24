using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyGarmin.Dashboard.Connectivity.StravaClient.Data.Converters
{
    internal class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (DateTime.TryParse(reader.GetString(), out var date))
            {
                return date;
            }

            return default;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
    }
}
