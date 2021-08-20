using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    [JsonConverter(typeof(CustomDataTypeBaseJsonConverter))]
    public abstract class CustomDataTypeBase<TValueType, TOwnType> : IEquatable<CustomDataTypeBase<TValueType, TOwnType>>, IComparable<TOwnType>
            where TOwnType : CustomDataTypeBase<TValueType, TOwnType>
            where TValueType : IComparable<TValueType>
    {
        private readonly TValueType _value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected CustomDataTypeBase(TValueType value)
        {
            if (null == value)
                throw new ArgumentNullException(nameof(value));

            if (GetType() != typeof(TOwnType))
                throw new CustomDataTypeBaseTypeMismatchException($"Class {GetType().FullName} has specified a different type as IOwnType, in this case {typeof(TOwnType)}");

            if (typeof(TValueType) == typeof(double) || typeof(TValueType) == typeof(float))
                throw new UnsupportedValueTypeException($"{typeof(TOwnType).FullName} has a value type of {(typeof(TValueType).FullName)} which is not allowed as value type as it would risk of being altered when being serialized and deserialized due to the nature of floating points.");

            _value = value;
        }

        public TValueType Value => _value;

        public override bool Equals(object obj) => EqualsInternal(obj);

        public static bool operator ==(CustomDataTypeBase<TValueType, TOwnType> first, CustomDataTypeBase<TValueType, TOwnType> second) => EqualsInternal(first: first, second: second);
        public static bool operator !=(CustomDataTypeBase<TValueType, TOwnType> first, CustomDataTypeBase<TValueType, TOwnType> second) => !(first == second);

        public bool Equals(CustomDataTypeBase<TValueType, TOwnType> other) => EqualsInternal(other);

        private bool EqualsInternal(object other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (other.GetType() != GetType())
                return false;

            return Value.Equals(((CustomDataTypeBase<TValueType, TOwnType>)other).Value);
        }

        private static bool EqualsInternal(CustomDataTypeBase<TValueType, TOwnType> first, CustomDataTypeBase<TValueType, TOwnType> second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null))
                return true;

            if (ReferenceEquals(first, null) || ReferenceEquals(second, null))
                return false;

            if (first.GetType() != second.GetType())
                return false;

            return first.Value.Equals(second.Value);
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value.ToString();

        public int CompareTo(TOwnType other)
        {
            if (other == null)
                return 1;
            return Value.CompareTo(other.Value);
        }
    }

    public class CustomDataTypeBaseTypeMismatchException : Exception
    {
        public CustomDataTypeBaseTypeMismatchException(string msg) : base(msg) { }
    }

    public class UnsupportedValueTypeException : Exception
    {
        public UnsupportedValueTypeException(string msg) : base(msg) { }
    }
}
