using System;
using System.Collections.Generic;
using RestSharp;

namespace InMobile.Sms.ApiClient
{
    public interface IListApiMethods
    {
        RecipientList CreateList(string name);
        List<RecipientList> GetAllLists();
        RecipientList GetListById(string listId);
        /// <summary>
        /// Updates the given list. This call both allows for the client to retrieve a listEntry, update it and pass it to UpdateList. It also allows for updating a list without retrieving it first simply by calling UpdateList(myListId, new { Name = "New list name" });
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="updateObject">
        /// </param>
        /// <returns></returns>
        RecipientList UpdateList(IRecipientListUpdateInfo list);
        void DeleteListById(string listId);

        Recipient CreateRecipient(RecipientCreateInfo recipient);
        List<Recipient> GetAllRecipientsInList(string listId);
        Recipient GetRecipientById(string listId, string recipientId);
        Recipient GetRecipientByNumber(string listId, string countryCode, string phoneNumber);

        Recipient UpdateRecipient(IRecipientUpdateInfo recipient);
        
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

        public RecipientList CreateList(string name)
        {
            return _requestHelper.Execute<RecipientList>(
                        method: Method.POST,
                        resource: "/v4/lists",
                        payload: new
                        {
                            name = name
                        });
        }


        public List<RecipientList> GetAllLists()
        {
            return _requestHelper.ExecuteGetAndIteratePagedResult<RecipientList>(resource: "/v4/lists?pageLimit=250");
        }

        public RecipientList GetListById(string listId)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            return _requestHelper.Execute<RecipientList>(method: Method.GET, resource: $"/v4/lists/{listId}");
        }

        public RecipientList UpdateList(IRecipientListUpdateInfo list)
        {
            return UpdateListInternal(listId: list.ListId, updateObject: new { name = list.Name });
        }
        private RecipientList UpdateListInternal(string listId, object updateObject)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            return _requestHelper.Execute<RecipientList>(method: Method.PUT, resource: $"/v4/lists/{listId}", payload: updateObject);
        }

        public void DeleteListById(string listId)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            _requestHelper.ExecuteWithNoContent(method: Method.DELETE, resource: $"/v4/lists/{listId}");
        }

        public Recipient CreateRecipient(RecipientCreateInfo recipient)
        {
            return _requestHelper.Execute<Recipient>(method: Method.POST, resource: $"/v4/lists/{recipient.ListId}/recipients", payload: new { NumberInfo = recipient.NumberInfo, Fields = recipient.Fields });
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

        public Recipient UpdateRecipient(IRecipientUpdateInfo recipient)
        {
            return UpdateRecipientInternal(listId: recipient.ListId, recipientId: recipient.RecipientId, updateObject: new {
                NumberInfo = recipient.NumberInfo,
                Fields = recipient.Fields
            });
        }

        private Recipient UpdateRecipientInternal(string listId, string recipientId, object updateObject)
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
