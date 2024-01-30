using System;
using System.Collections.Generic;

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
        /// Get a specific email template by its id.
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        EmailTemplate GetById(EmailTemplateId templateId);
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
        {
            return _requestHelper.ExecuteGetAndIteratePagedResult<EmailTemplate>(resource: $"{V4_email_templates}?pageLimit=250");
        }

        public EmailTemplate GetById(EmailTemplateId templateId)
        {
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(templateId), value: templateId);
            return _requestHelper.Execute<EmailTemplate>(method: Method.GET, resource: $"{V4_email_templates}/{templateId}");
        }
    }
}
