using System;
using System.Collections.Generic;
using InMobile.Sms.ApiClient.List.Lists;
using RestSharp;

namespace InMobile.Sms.ApiClient
{
    public interface IListClient
    {
        List<ListEntry> GetAllLists();
        ListEntry CreateList(string name);
        ListEntry GetListById(string listId);
        void DeleteListById(string listId);
        void UpdateList();

        List<Recipient> GetAllRecipientsInList(string listId);
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

        public ListEntry CreateList(string name)
        {
            return _requestHelper.Execute<ListEntry>(
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

        public void DeleteListById(string listId)
        {
            _requestHelper.ExecuteWithNoContent(method: Method.DELETE, resource: $"/v4/lists/{listId}");
        }

        public void DeleteRecipient()
        {
            throw new NotImplementedException();
        }

        public void FindRecipient()
        {
            throw new NotImplementedException();
        }

        public List<ListEntry> GetAllLists()
        {
            return _requestHelper.ExecuteGetAndIteratePagedResult<ListEntry>(resource: "/v4/lists?pageLimit=250");
        }

        public List<Recipient> GetAllRecipientsInList(string listId)
        {
            return _requestHelper.ExecuteGetAndIteratePagedResult<Recipient>(resource: $"/v4/lists/{listId}/recipients?pageLimit=250");
        }

        public ListEntry GetListById(string listId)
        {
            return _requestHelper.Execute<ListEntry>(method: Method.GET, resource: $"/v4/lists/{listId}");
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
