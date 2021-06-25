using System;
using System.Collections.Generic;
using System.Text;

namespace InMobile.Sms.ApiClient
{
    public class MessageCancelInfo
    {
        public string MessageId { get; set; }

        public MessageCancelInfo(string messageId)
        {
            MessageId = messageId;
        }
    }
}
