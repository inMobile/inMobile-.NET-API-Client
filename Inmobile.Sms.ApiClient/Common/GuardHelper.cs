using System;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Helper for guarding against invalid inputs
    /// </summary>
    public static class GuardHelper
    {
        /// <summary>
        /// Checks that value is not null - throws if it is
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void EnsureNotNullOrThrow(string parameterName, object? value)
        {
            if (value == null)
                throw new ArgumentException($"'{parameterName}' cannot be null.", nameof(value));
        }
    }
}
