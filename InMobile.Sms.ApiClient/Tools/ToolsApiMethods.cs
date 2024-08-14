using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// A collection of Tools-specific api endpoints.
    /// </summary>
    public interface IToolsApiMethods
    {
        /// <summary>
        /// Parse phone numbers.
        /// </summary>
        /// <returns></returns>
        ResultsList<NumberDetails> ParsePhoneNumbers(List<ParsePhoneNumberInfo> numbersToParse);
        
        /// <summary>
        /// Parse phone numbers (async).
        /// </summary>
        /// <returns></returns>
        Task<ResultsList<NumberDetails>> ParsePhoneNumbersAsync(List<ParsePhoneNumberInfo> numbersToParse);
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
            => ParsePhoneNumbersInternal(numbersToParse: numbersToParse, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<ResultsList<NumberDetails>> ParsePhoneNumbersAsync(List<ParsePhoneNumberInfo> numbersToParse)
            => ParsePhoneNumbersInternal(numbersToParse: numbersToParse, mode: SyncMode.Async);
        
        private async Task<ResultsList<NumberDetails>> ParsePhoneNumbersInternal(List<ParsePhoneNumberInfo> numbersToParse, SyncMode mode)
        {
            GuardHelper.EnsureNotNullOrThrow(nameof(numbersToParse), numbersToParse);

            const Method method = Method.POST;
            var resource = $"{V4_tools}/parsephonenumbers"; 
            var payload = new { NumbersToParse = numbersToParse };
            
            return mode == SyncMode.Sync 
                ? _requestHelper.Execute<ResultsList<NumberDetails>>(method: method, resource: resource, payload: payload)
                : await _requestHelper.ExecuteAsync<ResultsList<NumberDetails>>(method: method, resource: resource, payload: payload);
        }
    }
}
