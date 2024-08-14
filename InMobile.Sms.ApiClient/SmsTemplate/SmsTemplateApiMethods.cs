using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// A collection of SMS template-specific api endpoints.
    /// </summary>
    public interface ISmsTemplateApiMethods
    {
        /// <summary>
        /// Get all existing SMS templates on account.
        /// </summary>
        /// <returns></returns>
        List<SmsTemplate> GetAll();

        /// <summary>
        /// Get all existing SMS templates on account (async).
        /// </summary>
        /// <returns></returns>
        Task<List<SmsTemplate>> GetAllAsync();

        /// <summary>
        /// Get a specific SMS template by its id.
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        SmsTemplate GetById(SmsTemplateId templateId);

        /// <summary>
        /// Get a specific SMS template by its id (async).
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        Task<SmsTemplate> GetByIdAsync(SmsTemplateId templateId);
    }

    internal class SmsTemplateApiMethods : ISmsTemplateApiMethods
    {
        private const string V4_sms_templates = "/v4/sms/templates";

        private readonly IApiRequestHelper _requestHelper;

        public SmsTemplateApiMethods(IApiRequestHelper requestHelper)
        {
            _requestHelper = requestHelper ?? throw new ArgumentNullException(nameof(requestHelper));
        }

        public List<SmsTemplate> GetAll()
            => GetAllInternal(mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<List<SmsTemplate>> GetAllAsync()
            => GetAllInternal(mode: SyncMode.Async);

        private async Task<List<SmsTemplate>> GetAllInternal(SyncMode mode)
        {
            var resource = $"{V4_sms_templates}?pageLimit=250";

            return mode == SyncMode.Sync
                ? _requestHelper.ExecuteGetAndIteratePagedResult<SmsTemplate>(resource: resource)
                : await _requestHelper.ExecuteGetAndIteratePagedResultAsync<SmsTemplate>(resource: resource);
        }

        public SmsTemplate GetById(SmsTemplateId templateId)
            => GetByIdInternal(templateId: templateId, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<SmsTemplate> GetByIdAsync(SmsTemplateId templateId)
            => GetByIdInternal(templateId: templateId, mode: SyncMode.Async);

        private async Task<SmsTemplate> GetByIdInternal(SmsTemplateId templateId, SyncMode mode)
        {
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(templateId), value: templateId);

            const Method method = Method.GET;
            var resource = $"{V4_sms_templates}/{templateId}";

            return mode == SyncMode.Sync
                ? _requestHelper.Execute<SmsTemplate>(method: method, resource: resource)
                : await _requestHelper.ExecuteAsync<SmsTemplate>(method: method, resource: resource);
        }
    }
}