using System;
using System.Collections.Generic;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// A collection of Tools-specific api endpoints.
    /// </summary>
    public interface IToolsApiMethods
    {
        /// <summary>
        /// Parse phone numbers
        /// </summary>
        /// <returns></returns>
        ResultsList<NumberDetails> ParsePhoneNumbers(List<ParsePhoneNumberInfo> numbersToParse);
    }

    internal class ToolsApiMethods : IToolsApiMethods
    {
        private const string V4_tools = "/v4/tools";

        private readonly IApiRequestHelper _requestHelper;

        public ToolsApiMethods(IApiRequestHelper requestHelper)
        {
            _requestHelper = requestHelper ?? throw new ArgumentNullException(nameof(requestHelper));
        }

        public ResultsList<NumberDetails> ParsePhoneNumbers(List<ParsePhoneNumberInfo> numbersToParse)
        {
            GuardHelper.EnsureNotNullOrThrow(nameof(numbersToParse), numbersToParse);

            return _requestHelper.Execute<ResultsList<NumberDetails>>(
                method: Method.POST,
                resource: $"{V4_tools}/parsephonenumbers",
                payload: new
                {
                    NumbersToParse = numbersToParse
                });
        }
    }
}
