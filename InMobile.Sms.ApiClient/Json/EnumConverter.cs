using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    internal class EnumConverter<T> : JsonConverter where T : struct
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(T));
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
                return null;
            T result;
            if (Enum.TryParse<T>(reader.Value as string, ignoreCase: true, out result))
            {
                return result;
            }
            else
            {
                return default(T);
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
