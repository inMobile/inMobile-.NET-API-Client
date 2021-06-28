using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RestSharp.Serialization;
using RestSharp;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Serializer created by https://gist.github.com/alexeyzimarev
    /// Repo: https://gist.github.com/alexeyzimarev/c00b79c11c8cce6f6208454f7933ad24
    /// </summary>
    public class JsonNetSerializer : IRestSerializer
    {
        public string[] SupportedContentTypes { get; } =
        {
            "application/json", "text/json", "text/x-json", "text/javascript", "*+json"
        };

        public string ContentType { get; set; } = "application/json";

        public DataFormat DataFormat { get; } = DataFormat.Json;

        public string Serialize(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return json;
        }

        [Obsolete]
        public string Serialize(Parameter parameter)
        {
            return JsonConvert.SerializeObject(parameter.Value);
        }

        public T Deserialize<T>(IRestResponse response)
        {
            var result = JsonConvert.DeserializeObject<T>(value: response.Content);
            if (result == null)
                throw new Exception("Unexpected null deserialized from response");
            return result;
        }
    }
}
