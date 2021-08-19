using Newtonsoft.Json;
using Xunit;

namespace InMobile.Sms.ApiClient.Test.Common
{ 
    public class CustomDataTypeBaseJsonConverter_Test
    {
        [Fact]

        public void CustomDataTypeBaseJsonConverter_RawValue_TypeLong_Serialize_Test()
        {
            var input = new UnitTestCustomLong(123);
            var serialized = JsonConvert.SerializeObject(input);
            Assert.Equal(@"123", serialized);
            var deserialized = JsonConvert.DeserializeObject<UnitTestCustomLong>(serialized);

            Assert.IsType<UnitTestCustomLong>(deserialized);
            Assert.Equal(new UnitTestCustomLong(123), (UnitTestCustomLong)deserialized);
        }

        [Fact]

        public void CustomDataTypeBaseJsonConverter_RawValue_TypeInt_Serialize_Test()
        {
            var input = new UnitTestCustomInt(123);
            var serialized = JsonConvert.SerializeObject(input);
            Assert.Equal(@"123", serialized);
            var deserialized = JsonConvert.DeserializeObject<UnitTestCustomInt>(serialized);

            Assert.IsType<UnitTestCustomInt>(deserialized);
            Assert.Equal(new UnitTestCustomInt(123), (UnitTestCustomInt)deserialized);
        }

        [Fact]

        public void CustomDataTypeBaseJsonConverter_RawValue_TypeString_Serialize_Test()
        {
            var input = new UnitTestCustomString("DKK");
            var serialized = JsonConvert.SerializeObject(input);
            Assert.Equal(@"""DKK""", serialized);
            var deserialized = JsonConvert.DeserializeObject<UnitTestCustomString>(serialized);

            Assert.IsType<UnitTestCustomString>(deserialized);
            Assert.Equal(new UnitTestCustomString("DKK"), ((UnitTestCustomString)deserialized));
        }

        [Fact]

        public void CustomDataTypeBaseJsonConverter_RawValue_TypeCustomDecimal_Test()
        {
            var input = new UnitTestCustomDecimal(4321.1234m);
            var serialized = JsonConvert.SerializeObject(input);
            Assert.Equal(@"4321.1234", serialized);
            var deserialized = JsonConvert.DeserializeObject<UnitTestCustomDecimal>(serialized);

            Assert.IsType<UnitTestCustomDecimal>(deserialized);
            Assert.Equal(4321.1234m, ((UnitTestCustomDecimal)deserialized).Value);
        }

        [Fact]
        
        public void CustomDataTypeBaseJsonConverter_RawValue_Null_Serialize_Test()
        {
            UnitTestCustomString input = null;
            var serialized = JsonConvert.SerializeObject(input);
            Assert.Equal(@"null", serialized);
            var deserialized = JsonConvert.DeserializeObject<UnitTestCustomString>(serialized);
            Assert.Null(deserialized);
        }

        [Fact]
        
        public void CustomDataTypeBaseJsonConverter_ValueOnObject_Serialize_Test()
        {
            var serialized = JsonConvert.SerializeObject(new ObjectContainingACustomerType() { MyType = new UnitTestCustomString("someValue12") });
            Assert.Equal(@"{""MyType"":""someValue12""}", serialized);
            var deserialized = JsonConvert.DeserializeObject<ObjectContainingACustomerType>(serialized);
            Assert.IsType<ObjectContainingACustomerType>(deserialized);
            Assert.Equal(new UnitTestCustomString("someValue12"), ((ObjectContainingACustomerType)deserialized).MyType);
            Assert.Equal("someValue12", ((ObjectContainingACustomerType)deserialized).MyType.Value);
        }

        [Fact]
        
        public void CustomDataTypeBaseJsonConverter_ValueOnObject_NullOnObject_Null_Serialize_Test()
        {
            var serialized = JsonConvert.SerializeObject(new ObjectContainingACustomerType() { MyType = null });
            Assert.Equal(@"{""MyType"":null}", serialized);
            var deserialized = JsonConvert.DeserializeObject<ObjectContainingACustomerType>(serialized);
            Assert.IsType< ObjectContainingACustomerType>(deserialized);
            Assert.Null(((ObjectContainingACustomerType)deserialized).MyType);
        }

        [Fact]
        
        public void CanConvert_Test()
        {
            var converter = new CustomDataTypeBaseJsonConverter();
            Assert.True(converter.CanConvert(typeof(UnitTestCustomLong)));
            Assert.True(converter.CanConvert(typeof(UnitTestCustomString)));
            Assert.False(converter.CanConvert(typeof(CustomDataTypeBaseJsonConverter_Test)));
        }

        [Fact]
        public void CustomDataTypeBaseJsonConverter_RawValue_TypeHasMultipleConstructor_Serialize_Test()
        {
            var serialized = JsonConvert.SerializeObject(new MultipleConstructorsType(999));
            Assert.Equal(@"999", serialized);
            var deserialized = JsonConvert.DeserializeObject<MultipleConstructorsType>(serialized);
            Assert.IsType<MultipleConstructorsType>(deserialized);
            Assert.Equal(999L, ((MultipleConstructorsType)deserialized).Value);
        }

        [Fact]
        
        public void CustomDataTypeBaseJsonConverter_ValueOnObject_TypeHasMultipleConstructor_Serialize_Test()
        {
            var serialized = JsonConvert.SerializeObject(new CustomerTypeWith_MultipleConstructorProperty() { MultipleConstructorValue = new MultipleConstructorsType(999) });
            Assert.Equal(@"{""MultipleConstructorValue"":999}", serialized);
            var deserialized = JsonConvert.DeserializeObject<CustomerTypeWith_MultipleConstructorProperty>(serialized);
            Assert.IsType<CustomerTypeWith_MultipleConstructorProperty>(deserialized);
            Assert.Equal(999L, ((CustomerTypeWith_MultipleConstructorProperty)deserialized).MultipleConstructorValue.Value);
        }

        [Fact]
        
        public void CustomDataTypeBaseJsonConverter_ValueOnObject_TypeHasMultipleConstructor_NullValue_Serialize_Test()
        {
            var serialized = JsonConvert.SerializeObject(new CustomerTypeWith_MultipleConstructorProperty() { MultipleConstructorValue = null });
            Assert.Equal(@"{""MultipleConstructorValue"":null}", serialized);
            var deserialized = JsonConvert.DeserializeObject<CustomerTypeWith_MultipleConstructorProperty>(serialized);
            Assert.IsType<CustomerTypeWith_MultipleConstructorProperty>(deserialized);
            Assert.Null(((CustomerTypeWith_MultipleConstructorProperty)deserialized).MultipleConstructorValue);
        }

        public class MultipleConstructorsType : CustomDataTypeBase<long, MultipleConstructorsType>
        {
            public MultipleConstructorsType(string someStringConstructor) : base(111) { }
            /// <summary>
            /// This is the expected constructor to be used
            /// </summary>
            /// <param name="value"></param>
            public MultipleConstructorsType(long value) : base(value) { }
            public MultipleConstructorsType(long value, int someContructorWithMultipleParameters) : base(222) { }   
        }

        public class CustomerTypeWith_MultipleConstructorProperty
        {
            public MultipleConstructorsType MultipleConstructorValue { get; set; }
        }

        public class ObjectContainingACustomerType
        {
            public UnitTestCustomString MyType { get; set; }
        }

        public class UnitTestCustomLong : CustomDataTypeBase<long, UnitTestCustomLong>
        {
            public UnitTestCustomLong(long value) : base(value)
            {
            }
        }

        public class UnitTestCustomInt : CustomDataTypeBase<int, UnitTestCustomInt>
        {
            public UnitTestCustomInt(int value) : base(value)
            {
            }
        }

        public class UnitTestCustomString : CustomStringTypeBase<UnitTestCustomString>
        {
            public UnitTestCustomString(string value) : base(value: value)
            {
            }

            public const int MinLength = 3;
            public const int MaxLength = 3;
        }

        public class UnitTestCustomDecimal : CustomDataTypeBase<decimal, UnitTestCustomDecimal>
        {
            public UnitTestCustomDecimal(decimal value) : base(value)
            {
            }
        }
    }
}
