using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Serializer created by https://gist.github.com/alexeyzimarev
    /// Repo: https://gist.github.com/alexeyzimarev/c00b79c11c8cce6f6208454f7933ad24
    /// </summary>
    public class InMobileJsonSerializerSettings : JsonSerializerSettings
    {
        /// <summary>
        /// Used to avoid having the client setting up his/her own serializer and then accidentially affecting how this client works.
        /// </summary>
        public static JsonSerializerSettings Settings { get; } = new JsonSerializerSettings();

        /// <summary>
        /// 
        /// </summary>
        public InMobileJsonSerializerSettings()
        {
            Converters.Add(new EnumConverter<MessageEncoding>());
        }
    }
}
