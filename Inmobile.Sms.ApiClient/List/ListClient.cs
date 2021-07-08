using System;
using System.Collections.Generic;
using InMobile.Sms.ApiClient.List.Lists;
using RestSharp;

namespace InMobile.Sms.ApiClient
{
    public interface IListClient
    {
        List<ListApiModel> GetAllLists();
        ListApiModel CreateList(string name);
        void GetList();
        void DeleteList();
        void UpdateList();

        void GetAllRecipientsInList();
        void CreateRecipient();
        void DeleteAllRecipientsInList();
        void GetRecipient();
        void DeleteRecipient();
        void FindRecipient(); // TODO in api
        void UpdateRecipient();
    }

    internal class ListClient : IListClient
    {
        private readonly IApiRequestHelper _requestHelper;

        public ListClient(IApiRequestHelper requestHelper)
        {
            _requestHelper = requestHelper ?? throw new ArgumentNullException(nameof(requestHelper));
        }

        public ListApiModel CreateList(string name)
        {
            return _requestHelper.Execute<ListApiModel>(
                        method: Method.POST,
                        resource: "/v4/lists",
                        payload: new
                        {
                            name = name
                        });
        }

        public void CreateRecipient()
        {
            throw new NotImplementedException();
        }

        public void DeleteAllRecipientsInList()
        {
            throw new NotImplementedException();
        }

        public void DeleteList()
        {
            throw new NotImplementedException();
        }

        public void DeleteRecipient()
        {
            throw new NotImplementedException();
        }

        public void FindRecipient()
        {
            throw new NotImplementedException();
        }

        public List<ListApiModel> GetAllLists()
        {
            return _requestHelper.ExecuteGetAndIteratePagedResult<ListApiModel>(resource: "/v4/lists?pageLimit=250");
        }

        public void GetAllRecipientsInList()
        {
            throw new NotImplementedException();
        }

        public void GetList()
        {
            throw new NotImplementedException();
        }

        public void GetRecipient()
        {
            throw new NotImplementedException();
        }

        public void UpdateList()
        {
            throw new NotImplementedException();
        }

        public void UpdateRecipient()
        {
            throw new NotImplementedException();
        }
    }
}
