using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    #pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class CustomDataTypeBaseJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (IsSubclassOfRawGeneric(generic: typeof(CustomDataTypeBase<,>), toCheck: objectType))
            {
                return true;
            }

            return false;
        }

        private bool CheckTypeMatching(ParameterInfo constructorsParameterInfo, Type jsonReadersType)
        {
            if (constructorsParameterInfo.ParameterType == jsonReadersType)
                return true;

            // Special case - ints are represented as longs by the json reader
            if (constructorsParameterInfo.ParameterType == typeof(int) && jsonReadersType == typeof(long))
                return true;

            // Special case - decimals are represented as doubles by the json reader
            if (constructorsParameterInfo.ParameterType == typeof(decimal) && jsonReadersType == typeof(double))
                return true;

            return false;
        }

        #pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        public override object? ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (CanConvert(objectType) == false)
                throw new ArgumentException("Cant deserialize object type " + objectType.FullName);

            if (reader.Value == null)
                return null;

            // Find a constructor that can be used
            var constructors = objectType.GetConstructors() // Get all constructors
                                    .Where(constructorInfo => constructorInfo.GetParameters().Length == 1)
                                    .Where(constructorInfo => CheckTypeMatching(constructorsParameterInfo: constructorInfo.GetParameters().Single(), jsonReadersType: reader.Value.GetType()))
                                    .ToList(); // With exactly 1 parameter

            if (constructors.Count == 0)
                throw new Exception($"Cannot deserialize type {objectType.GetType()} as it does not have a constructor with exactly one input parameter of type {reader.Value.GetType().Name}");

            if (constructors.Count > 1)
                throw new Exception($"Unexpected error with multiple matching constructors with exactly one parameter of type {reader.Value.GetType().Name}");

            var targetConstructor = constructors.Single();

            object outputCustomObject;
            if (targetConstructor.GetParameters().Single().ParameterType == typeof(int) && reader.Value.GetType() == typeof(long))
            {
                // Special case with int/long
                outputCustomObject = targetConstructor.Invoke(new object[] { Convert.ToInt32(reader.Value) });
            }
            else if (targetConstructor.GetParameters().Single().ParameterType == typeof(decimal) && reader.Value.GetType() == typeof(double))
            {
                // Special case with decimal/double
                outputCustomObject = targetConstructor.Invoke(new object[] { Convert.ToDecimal(reader.Value) });
            }
            else
            {
                // Standard case
                outputCustomObject = targetConstructor.Invoke(new object[] { reader.Value });
            }

            return outputCustomObject;
        }

        public override void WriteJson(JsonWriter writer, object customerBaseType, JsonSerializer serializer)
        {
            if (customerBaseType == null)
                throw new ArgumentException("Never expected value to be NULL as this is not allowed for CustomDataTypeBase");

            if (CanConvert(customerBaseType.GetType()) == false)
                throw new ArgumentException($"Cant serialize object type {customerBaseType.GetType().FullName}");

            if (customerBaseType == null)
            {
                writer.WriteValue((object?)null);
            }
            else
            {
                // Extract VALUE
                var valueProperty = customerBaseType.GetType().GetProperty(name: "Value");
                var innerValue = valueProperty.GetValue(customerBaseType);

                writer.WriteValue(innerValue);
            }
        }

        // https://stackoverflow.com/questions/457676/check-if-a-class-is-derived-from-a-generic-class
        static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}