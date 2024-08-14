using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// A collection of email template-specific api endpoints.
    /// </summary>
    public interface IEmailTemplateApiMethods
    {
        /// <summary>
        /// Get all existing email templates on account.
        /// </summary>
        /// <returns></returns>
        List<EmailTemplate> GetAll();
        
        /// <summary>
        /// Get all existing email templates on account (async).
        /// </summary>
        /// <returns></returns>
        Task<List<EmailTemplate>> GetAllAsync();

        /// <summary>
        /// Get a specific email template by its id.
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        EmailTemplate GetById(EmailTemplateId templateId);
        
        /// <summary>
        /// Get a specific email template by its id (async).
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        Task<EmailTemplate> GetByIdAsync(EmailTemplateId templateId);
    }

    internal class EmailTemplateApiMethods : IEmailTemplateApiMethods
    {
        private const string V4_email_templates = "/v4/email/templates";

        private readonly IApiRequestHelper _requestHelper;

        public EmailTemplateApiMethods(IApiRequestHelper requestHelper)
        {
            _requestHelper = requestHelper ?? throw new ArgumentNullException(nameof(requestHelper));
        }

        public List<EmailTemplate> GetAll()
            => GetAllInternal(mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<List<EmailTemplate>> GetAllAsync()
            => GetAllInternal(mode: SyncMode.Async);
        
        private async Task<List<EmailTemplate>> GetAllInternal(SyncMode mode)
        {
            var resource = $"{V4_email_templates}?pageLimit=250";
            
            return mode == SyncMode.Sync 
                ? _requestHelper.ExecuteGetAndIteratePagedResult<EmailTemplate>(resource: resource)
                : await _requestHelper.ExecuteGetAndIteratePagedResultAsync<EmailTemplate>(resource: resource);
        }

        public EmailTemplate GetById(EmailTemplateId templateId)
            => GetByIdInternal(templateId: templateId, mode: SyncMode.Sync).GetAwaiter().GetResult();
        
        public Task<EmailTemplate> GetByIdAsync(EmailTemplateId templateId)
            => GetByIdInternal(templateId: templateId, mode: SyncMode.Async);
        
        private async Task<EmailTemplate> GetByIdInternal(EmailTemplateId templateId, SyncMode mode)
        {
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(templateId), value: templateId);

            const Method method = Method.GET;
            var resource = $"{V4_email_templates}/{templateId}";
            
            return mode == SyncMode.Sync 
                ? _requestHelper.Execute<EmailTemplate>(method: method, resource: resource)
                : await _requestHelper.ExecuteAsync<EmailTemplate>(method: method, resource: resource);
        }
    }
}
