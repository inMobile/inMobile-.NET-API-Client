using System;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    public class MessageEncodingConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(MessageEncoding));
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
                return null;
            MessageEncoding result;
            if(Enum.TryParse< MessageEncoding>(reader.Value as string, out result))
            {
                return result;
            }
            else
            {
                return MessageEncoding.Unknown;
            }
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                var encoding = (MessageEncoding)value;
                writer.WriteValue(encoding.ToString().ToLower());
            }
        }
    }
}