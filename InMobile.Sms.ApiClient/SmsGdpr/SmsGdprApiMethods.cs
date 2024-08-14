using System;
using System.Threading.Tasks;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// A collection of GDPR-specific api endpoints.
    /// </summary>
    public interface ISmsGdprApiMethods
    {
        /// <summary>
        /// Create information Deletion Request.
        /// </summary>
        /// <returns></returns>
        SmsGdprDeletionRequestCreateResult CreateDeletionRequest(NumberInfo numberInfo);

        /// <summary>
        /// Create information Deletion Request (async).
        /// </summary>
        /// <returns></returns>
        Task<SmsGdprDeletionRequestCreateResult> CreateDeletionRequestAsync(NumberInfo numberInfo);
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
            => CreateDeletionRequestInternal(numberInfo: numberInfo, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<SmsGdprDeletionRequestCreateResult> CreateDeletionRequestAsync(NumberInfo numberInfo)
            => CreateDeletionRequestInternal(numberInfo: numberInfo, mode: SyncMode.Async);

        private async Task<SmsGdprDeletionRequestCreateResult> CreateDeletionRequestInternal(NumberInfo numberInfo, SyncMode mode)
        {
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(numberInfo), value: numberInfo);

            const Method method = Method.POST;
            var resource = $"{V4_sms_gdpr}/deletionrequests";
            var payload = new SmsGdprDeletionRequestCreateInfo(numberInfo: numberInfo);

            return mode == SyncMode.Sync
                ? _requestHelper.Execute<SmsGdprDeletionRequestCreateResult>(method: method, resource: resource, payload: payload)
                : await _requestHelper.ExecuteAsync<SmsGdprDeletionRequestCreateResult>(method: method, resource: resource, payload: payload);
        }
    }
}