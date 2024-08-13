using System;
using System.Threading.Tasks;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Outgoing email specific operations.
    /// </summary>
    public interface IEmailOutgoingApiMethods
    {
        /// <summary>
        /// Send email to one or more recipients.
        /// </summary>
        /// <param name="createInfo"></param>
        /// <returns></returns>
        OutgoingEmailCreateResult SendEmail(OutgoingEmailCreateInfo createInfo);

        /// <summary>
        /// Send email to one or more recipients (async).
        /// </summary>
        /// <param name="createInfo"></param>
        /// <returns></returns>
        Task<OutgoingEmailCreateResult> SendEmailAsync(OutgoingEmailCreateInfo createInfo);

        /// <summary>
        /// Send email using a template.
        /// </summary>
        /// <param name="templateCreateInfo"></param>
        /// <returns></returns>
        OutgoingEmailTemplateCreateResult SendEmailUsingTemplate(OutgoingEmailTemplateCreateInfo templateCreateInfo);

        /// <summary>
        /// Send email using a template (async).
        /// </summary>
        /// <param name="templateCreateInfo"></param>
        /// <returns></returns>
        Task<OutgoingEmailTemplateCreateResult> SendEmailUsingTemplateAsync(OutgoingEmailTemplateCreateInfo templateCreateInfo);

        /// <summary>
        /// Returns events about the email, e.g. Delivered, Clicked or PermanentFail.
        /// Each event will only be returned once. Once called, the event has been removed from our side and cannot be retrieved again using this method.
        /// </summary>
        /// <param name="limit">The maximum amount of events to receive. Limit bust be between 1 and 250.</param>
        /// <returns></returns>
        EmailEventsList<EmailEvent> GetEmailEvents(int limit);

        /// <summary>
        /// Returns events about the email, e.g. Delivered, Clicked or PermanentFail.
        /// Each event will only be returned once. Once called, the event has been removed from our side and cannot be retrieved again using this method (async).
        /// </summary>
        /// <param name="limit">The maximum amount of events to receive. Limit bust be between 1 and 250.</param>
        /// <returns></returns>
        Task<EmailEventsList<EmailEvent>> GetEmailEventsAsync(int limit);
    }

    internal class EmailOutgoingApiMethods : IEmailOutgoingApiMethods
    {
        private const string V4_email_outgoing = "/v4/email/outgoing";

        private readonly IApiRequestHelper _requestHelper;

        public EmailOutgoingApiMethods(IApiRequestHelper requestHelper)
        {
            _requestHelper = requestHelper ?? throw new ArgumentNullException(nameof(requestHelper));
        }

        public OutgoingEmailCreateResult SendEmail(OutgoingEmailCreateInfo createInfo)
            => SendEmailInternal(createInfo: createInfo, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<OutgoingEmailCreateResult> SendEmailAsync(OutgoingEmailCreateInfo createInfo)
            => SendEmailInternal(createInfo: createInfo, mode: SyncMode.Async);

        private async Task<OutgoingEmailCreateResult> SendEmailInternal(OutgoingEmailCreateInfo createInfo, SyncMode mode)
        {
            GuardHelper.EnsureNotNullOrThrow(nameof(createInfo), createInfo);

            const Method method = Method.POST;
            const string resource = V4_email_outgoing;

            return mode == SyncMode.Sync
                ? _requestHelper.Execute<OutgoingEmailCreateResult>(method: method, resource: resource, payload: createInfo)
                : await _requestHelper.ExecuteAsync<OutgoingEmailCreateResult>(method: method, resource: resource, payload: createInfo);
        }

        public OutgoingEmailTemplateCreateResult SendEmailUsingTemplate(OutgoingEmailTemplateCreateInfo templateCreateInfo)
            => SendEmailUsingTemplateInternal(templateCreateInfo: templateCreateInfo, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<OutgoingEmailTemplateCreateResult> SendEmailUsingTemplateAsync(OutgoingEmailTemplateCreateInfo templateCreateInfo)
            => SendEmailUsingTemplateInternal(templateCreateInfo: templateCreateInfo, mode: SyncMode.Async);

        private async Task<OutgoingEmailTemplateCreateResult> SendEmailUsingTemplateInternal(OutgoingEmailTemplateCreateInfo templateCreateInfo, SyncMode mode)
        {
            GuardHelper.EnsureNotNullOrThrow(nameof(templateCreateInfo), templateCreateInfo);

            const Method method = Method.POST;
            var resource = $"{V4_email_outgoing}/sendusingtemplate";

            return mode == SyncMode.Sync
                ? _requestHelper.Execute<OutgoingEmailTemplateCreateResult>(method: method, resource: resource, payload: templateCreateInfo)
                : await _requestHelper.ExecuteAsync<OutgoingEmailTemplateCreateResult>(method: method, resource: resource, payload: templateCreateInfo);
        }

        public EmailEventsList<EmailEvent> GetEmailEvents(int limit)
            => GetEmailEventsInternal(limit: limit, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<EmailEventsList<EmailEvent>> GetEmailEventsAsync(int limit)
            => GetEmailEventsInternal(limit: limit, mode: SyncMode.Async);

        private async Task<EmailEventsList<EmailEvent>> GetEmailEventsInternal(int limit, SyncMode mode)
        {
            if (limit <= 0 || limit > 250)
                throw new ArgumentException($"Invalid limit value: {limit}. Value must be between 1 and 250.");

            const Method method = Method.GET;
            var resource = $"{V4_email_outgoing}/events?limit={limit}";

            return mode == SyncMode.Sync
                ? _requestHelper.Execute<EmailEventsList<EmailEvent>>(method: method, resource: resource)
                : await _requestHelper.ExecuteAsync<EmailEventsList<EmailEvent>>(method: method, resource: resource);
        }
    }
}