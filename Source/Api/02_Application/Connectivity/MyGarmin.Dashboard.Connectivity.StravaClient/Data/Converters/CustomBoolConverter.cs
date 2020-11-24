using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyGarmin.Dashboard.Connectivity.StravaClient.Data.Converters
{
    internal class CustomBoolConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return (reader.TokenType == JsonTokenType.True);
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
