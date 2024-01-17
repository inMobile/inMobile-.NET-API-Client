using System;

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
        /// Send email using a template.
        /// </summary>
        /// <param name="templateCreateInfo"></param>
        /// <returns></returns>
        OutgoingEmailTemplateCreateResult SendEmailUsingTemplate(OutgoingEmailTemplateCreateInfo templateCreateInfo);

        /// <summary>
        /// Each event will only be returned once. Once called, the event has been removed from our side and cannot be retrieved again using this method.
        /// </summary>
        /// <param name="limit">The maximum amount of events to receive. Limit bust be between 1 and 250.</param>
        /// <returns></returns>
        EmailEventsList<EmailEvent> GetEmailEvents(int limit);
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
        {
            GuardHelper.EnsureNotNullOrThrow(nameof(createInfo), createInfo);

            return _requestHelper.Execute<OutgoingEmailCreateResult>(
                method: Method.POST,
                resource: $"{V4_email_outgoing}",
                payload: createInfo);
        }

        public OutgoingEmailTemplateCreateResult SendEmailUsingTemplate(OutgoingEmailTemplateCreateInfo templateCreateInfo)
        {
            GuardHelper.EnsureNotNullOrThrow(nameof(templateCreateInfo), templateCreateInfo);

            return _requestHelper.Execute<OutgoingEmailTemplateCreateResult>(
                method: Method.POST,
                resource: $"{V4_email_outgoing}/sendusingtemplate",
                payload: templateCreateInfo);
        }

        public EmailEventsList<EmailEvent> GetEmailEvents(int limit)
        {
            if (limit <= 0 || limit > 250)
                throw new ArgumentException($"Invalid limit value: {limit}. Value must be between 1 and 250.");

            return _requestHelper.Execute<EmailEventsList<EmailEvent>>(
                method: Method.GET,
                resource: $"{V4_email_outgoing}/events?limit={limit}");
        }
    }
}
