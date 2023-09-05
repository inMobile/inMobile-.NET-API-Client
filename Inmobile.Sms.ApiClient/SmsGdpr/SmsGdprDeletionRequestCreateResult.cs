using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Result info for a created GDPR deletion request.
    /// </summary>
    public class SmsGdprDeletionRequestCreateResult
    {
        /// <summary>
        /// The Id of the GDPR deletion request
        /// </summary>
        [JsonProperty]
        public SmsGdprDeletionRequestId Id { get; private set; }

        [JsonConstructor]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private SmsGdprDeletionRequestCreateResult()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }
    }
}
