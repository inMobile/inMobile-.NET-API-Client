using System;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Official InMobile API client
    /// </summary>
    public class InMobileApiClient
    {
        /// <summary>
        /// Outgoing message specific operations.
        /// </summary>
        public ISmsOutgoingApiMethod SmsOutgoing { get; private set; }

        /// <summary>
        /// Blacklist specific operations.
        /// </summary>
        public IBlacklistApiMethods Blacklist { get; private set; }

        /// <summary>
        /// List specific operations.
        /// </summary>
        public IListApiMethods Lists { get; private set; }

        /// <summary>
        /// Temmplate specific operations.
        /// </summary>
        public ISmsTemplateApiMethods SmsTemplates { get; private set; }

        /// <summary>
        /// GDPR specific operations.
        /// </summary>
        public ISmsGdprApiMethods SmsGdpr { get; private set; }

        /// <summary>
        /// Outgoing email specific operations.
        /// </summary>
        public IEmailOutgoingApiMethods EmailOutgoing { get; private set; }

        /// <summary>
        /// Email templates specific operations.
        /// </summary>
        public IEmailTemplateApiMethods EmailTemplates { get; private set; }

        /// <summary>
        /// Tools specific operations.
        /// </summary>
        public IToolsApiMethods Tools { get; private set; }

        /// <summary>
        /// Creates a new api client.
        /// </summary>
        /// <param name="apiKey">The api key to be used.</param>
        /// <param name="baseUrl">The base url of the api. This can be changed in case an internal proxy is in use.</param>
        public InMobileApiClient(InMobileApiKey apiKey, string baseUrl = "https://api.inmobile.com")
        {
            if (apiKey is null)
            {
                throw new ArgumentNullException(nameof(apiKey));
            }

            var requestHelper = new ApiRequestHelper(apiKey: apiKey, baseUrl: baseUrl);

            SmsOutgoing = new SmsOutgoingApiMethods(requestHelper);
            Blacklist = new BlacklistApiMethods(requestHelper);
            Lists = new ListApiMethods(requestHelper);
            SmsTemplates = new SmsTemplateApiMethods(requestHelper);
            SmsGdpr = new SmsGdprApiMethods(requestHelper);
            EmailOutgoing = new EmailOutgoingApiMethods(requestHelper);
            EmailTemplates = new EmailTemplateApiMethods(requestHelper);
            Tools = new ToolsApiMethods(requestHelper);
        }
    }
}
