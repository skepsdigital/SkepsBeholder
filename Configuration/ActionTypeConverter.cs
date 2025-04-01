using System.Text.Json.Serialization;
using System.Text.Json;
using SkepsBeholder.Model.Enum;

namespace SkepsBeholder.Configuration
{
    public class ActionTypeConverter : JsonConverter<ActionTypeEnum>
    {
        public override ActionTypeEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var enumString = reader.GetString();
            if (Enum.TryParse(enumString, true, out ActionTypeEnum result))
            {
                return result;
            }
            return ActionTypeEnum.Unknown; // Se não conseguir converter, usa o valor "Unknown"
        }

        public override void Write(Utf8JsonWriter writer, ActionTypeEnum value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
