using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    public class RecipientList : IRecipientListUpdateInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
