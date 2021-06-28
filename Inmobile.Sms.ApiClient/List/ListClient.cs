using System;

namespace InMobile.Sms.ApiClient
{
    public interface IListClient
    {
        void GetAllLists();
        void CreateList();
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

        public void CreateList()
        {
            throw new NotImplementedException();
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

        public void GetAllLists()
        {
            throw new NotImplementedException();
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
