using System.Collections.Generic;
using System.Text;

namespace InMobile.Sms.ApiClient
{
    public class RecipientList : IRecipientListUpdateInfo
    {
        public string ListId { get; set; }
        public string Name { get; set; }
    }
}
