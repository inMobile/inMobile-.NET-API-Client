using System.Collections.Generic;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Information for creating a new outgoing message using a template.
    /// </summary>
    public class OutgoingSmsTemplateCreateInfo
    {
        /// <summary>
        /// The id of the template to use.
        /// </summary>
        public TemplateId TemplateId { get; }

        /// <summary>
        /// A list of the sms messages to be sent. Allowed to contain between 1 and 250 elements.
        /// </summary>
        public List<OutgoingSmsTemplateMessageCreateInfo> Messages { get; }

        /// <summary>
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="messages"></param>
        public OutgoingSmsTemplateCreateInfo(TemplateId templateId, List<OutgoingSmsTemplateMessageCreateInfo> messages)
        {
            TemplateId = templateId;
            Messages = messages;
        }
    }
}
