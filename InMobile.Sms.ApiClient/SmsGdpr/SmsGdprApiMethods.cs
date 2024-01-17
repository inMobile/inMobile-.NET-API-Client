using System;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// A collection of GDPR-specific api endpoints.
    /// </summary>
    public interface ISmsGdprApiMethods
    {
        /// <summary>
        /// Create information Deletion Request
        /// </summary>
        /// <returns></returns>
        SmsGdprDeletionRequestCreateResult CreateDeletionRequest(NumberInfo numberInfo);
    }

    internal class SmsGdprApiMethods : ISmsGdprApiMethods
    {
        private const string V4_sms_gdpr = "/v4/sms/gdpr";

        private readonly IApiRequestHelper _requestHelper;

        public SmsGdprApiMethods(IApiRequestHelper requestHelper)
        {
            _requestHelper = requestHelper ?? throw new ArgumentNullException(nameof(requestHelper));
        }

        public SmsGdprDeletionRequestCreateResult CreateDeletionRequest(NumberInfo numberInfo)
        {
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(numberInfo), value: numberInfo);

            return _requestHelper.Execute<SmsGdprDeletionRequestCreateResult>(
                method: Method.POST, 
                resource: $"{V4_sms_gdpr}/deletionrequests",
                payload: new SmsGdprDeletionRequestCreateInfo(numberInfo: numberInfo));
        }
    }
}
