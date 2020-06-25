using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyGarmin.Dashboard.Connectivity.GarminClient.Data.Converters
{
    internal class CustomFloatConverter : JsonConverter<float>
    {
        public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.Null && reader.TryGetSingle(out var output))
            {
                return output;
            }
            else
            {
                return default;
            }
        }

        public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}
