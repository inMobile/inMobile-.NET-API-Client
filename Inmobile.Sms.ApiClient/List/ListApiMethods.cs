using System;
using System.Collections.Generic;
using RestSharp;

namespace InMobile.Sms.ApiClient
{
    public interface IListApiMethods
    {
        ListEntry CreateList(string name);
        List<ListEntry> GetAllLists();
        ListEntry GetListById(string listId);
        /// <summary>
        /// Updates the given list. This call both allows for the client to retrieve a listEntry, update it and pass it to UpdateList. It also allows for updating a list without retrieving it first simply by calling UpdateList(myListId, new { Name = "New list name" });
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="updateObject">
        /// </param>
        /// <returns></returns>
        ListEntry UpdateList(string listId, object updateObject);
        void DeleteListById(string listId);

        Recipient CreateRecipient(string listId, Recipient recipient);
        List<Recipient> GetAllRecipientsInList(string listId);
        Recipient GetRecipientById(string listId, string recipientId);
        Recipient GetRecipientByNumber(string listId, string countryCode, string phoneNumber);
        Recipient UpdateRecipient(string listId, string recipientId, object updateObject);

        void DeleteRecipientById(string listId, string recipientId);
        void DeleteRecipientByNumber(string listId, string countryCode, string phoneNumber);
        void DeleteAllRecipientsInList(string listId);
    }

    internal class ListApiMethods : IListApiMethods
    {
        private readonly IApiRequestHelper _requestHelper;

        public ListApiMethods(IApiRequestHelper requestHelper)
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


        public List<ListEntry> GetAllLists()
        {
            return _requestHelper.ExecuteGetAndIteratePagedResult<ListEntry>(resource: "/v4/lists?pageLimit=250");
        }

        public ListEntry GetListById(string listId)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            return _requestHelper.Execute<ListEntry>(method: Method.GET, resource: $"/v4/lists/{listId}");
        }

        public ListEntry UpdateList(string listId, object updateObject)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            return _requestHelper.Execute<ListEntry>(method: Method.PUT, resource: $"/v4/lists/{listId}", payload: updateObject);
        }

        public void DeleteListById(string listId)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            _requestHelper.ExecuteWithNoContent(method: Method.DELETE, resource: $"/v4/lists/{listId}");
        }

        public Recipient CreateRecipient(string listId, Recipient recipient)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            return _requestHelper.Execute<Recipient>(method: Method.POST, resource: $"/v4/lists/{listId}/recipients", payload: recipient);
        }

        public List<Recipient> GetAllRecipientsInList(string listId)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            return _requestHelper.ExecuteGetAndIteratePagedResult<Recipient>(resource: $"/v4/lists/{listId}/recipients?pageLimit=250");
        }

        public Recipient GetRecipientById(string listId, string recipientId)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            EnsureNonEmptyOrThrow(parameterName: nameof(recipientId), value: recipientId);
            return _requestHelper.Execute<Recipient>(method: Method.GET, resource: $"/v4/lists/{listId}/recipients/{recipientId}");
        }

        public Recipient GetRecipientByNumber(string listId, string countryCode, string phoneNumber)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            EnsureNonEmptyOrThrow(parameterName: nameof(countryCode), value: countryCode);
            EnsureNonEmptyOrThrow(parameterName: nameof(phoneNumber), value: phoneNumber);
            return _requestHelper.Execute<Recipient>(method: Method.GET, resource: $"/v4/lists/{listId}/recipients/bynumber?countryCode={countryCode}&phoneNumber={phoneNumber}");
        }

        public Recipient UpdateRecipient(string listId, string recipientId, object updateObject)
        {
            if (updateObject is null)
            {
                throw new ArgumentNullException(nameof(updateObject));
            }

            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            EnsureNonEmptyOrThrow(parameterName: nameof(recipientId), value: recipientId);
            return _requestHelper.Execute<Recipient>(method: Method.PUT, resource: $"/v4/lists/{listId}/recipients/{recipientId}", payload: updateObject);
        }

        public void DeleteAllRecipientsInList(string listId)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            _requestHelper.ExecuteWithNoContent(method: Method.DELETE, resource: $"/v4/lists/{listId}/recipients/all");
        }

        public void DeleteRecipientById(string listId, string recipientId)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            EnsureNonEmptyOrThrow(parameterName: nameof(recipientId), value: recipientId);
            _requestHelper.ExecuteWithNoContent(method: Method.DELETE, resource: $"/v4/lists/{listId}/recipients/{recipientId}");
        }

        public void DeleteRecipientByNumber(string listId, string countryCode, string phoneNumber)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            EnsureNonEmptyOrThrow(parameterName: nameof(countryCode), value: countryCode);
            EnsureNonEmptyOrThrow(parameterName: nameof(phoneNumber), value: phoneNumber);
            _requestHelper.ExecuteWithNoContent(method: Method.DELETE, resource: $"/v4/lists/{listId}/recipients/bynumber?countryCode={countryCode}&phoneNumber={phoneNumber}");
        }

        

        private void EnsureNonEmptyOrThrow(string parameterName, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"'{parameterName}' cannot be null or empty.", nameof(value));
            }
        }
    }
}
