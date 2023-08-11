using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xunit;

namespace InMobile.Sms.ApiClient.Test
{
    public class ClassNamingTest
    {
        [Fact]
        public void EnsureAllClassesSpelledInMobileWithCorrectCasing_Test()
        {
            var allTypes = typeof(InMobileApiClient).Assembly.GetTypes();
            foreach(var type in allTypes)
            {
                // If it contains "InMobile" in any case, ensure it is the correct casing
                if (type.Name.ToLower().Contains("inmobile")
                        && !type.Name.Contains("InMobile"))
                {
                    Assert.Fail($"The type {type.FullName} does not have InMobile spelled with correct casing");
                }
            }
        }

        [Fact]
        public void EnsureSameNamespaceOnAllClasses_Test()
        {
            var allTypes = typeof(InMobileApiClient).Assembly.GetTypes();
            foreach (var type in allTypes)
            {
                // If it contains "InMobile" in any case, ensure it is the correct casing
                if (CheckIfTypeIsAnonymous(type))
                    continue;
                if (!IsInmobileClass(type))
                    continue;
                if (type.Namespace != "InMobile.Sms.ApiClient")
                {
                    Assert.Fail($"The type {type.FullName} does not have expected namespace");
                }
            }
        }

        private bool IsInmobileClass(Type type)
        {
            return type.Namespace.ToLower().Contains("inmobile");
        }

        private bool CheckIfTypeIsAnonymous(Type type)
        {
            // From https://stackoverflow.com/questions/2483023/how-to-test-if-a-type-is-anonymous
            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                && type.IsGenericType && type.Name.Contains("AnonymousType")
                && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                && type.Attributes.HasFlag(TypeAttributes.NotPublic);
        }
    }
}
