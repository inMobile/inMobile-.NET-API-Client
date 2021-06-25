using System;
using RestSharp;

namespace InMobile.Sms.ApiClient
{
    public class InmobileApiException : Exception
    {
        public InmobileApiException(IRestResponse response) : base(message: FormatMessage(response: response))
        {

        }

        private static string FormatMessage(IRestResponse response)
        {
            throw new NotImplementedException();
        }
    }
}
