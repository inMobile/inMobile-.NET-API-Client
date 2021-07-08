using System;
using System.Collections.Generic;
using System.Text;

namespace InMobile.Sms.ApiClient
{
    public class CancelMessagesRequest
    {
        public string MessageId { get; set; }

        public CancelMessagesRequest(string messageId)
        {
            MessageId = messageId;
        }
    }
}
