using System;
using System.Collections.Generic;
using RestSharp;

namespace InMobile.Sms.ApiClient
{
    public interface IListApiMethods
    {
        RecipientList CreateList(RecipientListCreateInfo createInfo);
        List<RecipientList> GetAllLists();
        RecipientList GetListById(RecipientListId listId);
        /// <summary>
        /// Updates the given list. This call both allows for the client to retrieve a listEntry, update it and pass it to UpdateList. It also allows for updating a list without retrieving it first simply by calling UpdateList(myListId, new { Name = "New list name" });
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="updateObject">
        /// </param>
        /// <returns></returns>
        RecipientList UpdateList(IRecipientListUpdateInfo list);
        void DeleteListById(RecipientListId listId);

        Recipient CreateRecipient(RecipientCreateInfo recipient);
        List<Recipient> GetAllRecipientsInList(RecipientListId listId);
        Recipient GetRecipientById(RecipientListId listId, RecipientId recipientId);
        Recipient GetRecipientByNumber(RecipientListId listId, NumberInfo numberInfo);

        Recipient UpdateRecipient(IRecipientUpdateInfo recipient);
        
        void DeleteRecipientById(RecipientListId listId, RecipientId recipientId);
        void DeleteRecipientByNumber(RecipientListId listId, NumberInfo numberInfo);
        void DeleteAllRecipientsInList(RecipientListId listId);
    }

    internal class ListApiMethods : IListApiMethods
    {
        private readonly IApiRequestHelper _requestHelper;

        public ListApiMethods(IApiRequestHelper requestHelper)
        {
            _requestHelper = requestHelper ?? throw new ArgumentNullException(nameof(requestHelper));
        }

        public RecipientList CreateList(RecipientListCreateInfo createInfo)
        {
            if (createInfo is null)
            {
                throw new ArgumentNullException(nameof(createInfo));
            }

            return _requestHelper.Execute<RecipientList>(
                        method: Method.POST,
                        resource: "/v4/lists",
                        payload: createInfo);
        }

        public List<RecipientList> GetAllLists()
        {
            return _requestHelper.ExecuteGetAndIteratePagedResult<RecipientList>(resource: "/v4/lists?pageLimit=250");
        }

        public RecipientList GetListById(RecipientListId listId)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            return _requestHelper.Execute<RecipientList>(method: Method.GET, resource: $"/v4/lists/{listId}");
        }

        public RecipientList UpdateList(IRecipientListUpdateInfo list)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(list), value: list);
            return UpdateListInternal(listId: list.Id, updateObject: new { name = list.Name });
        }
        private RecipientList UpdateListInternal(RecipientListId listId, object updateObject)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            return _requestHelper.Execute<RecipientList>(method: Method.PUT, resource: $"/v4/lists/{listId}", payload: updateObject);
        }

        public void DeleteListById(RecipientListId listId)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            _requestHelper.ExecuteWithNoContent(method: Method.DELETE, resource: $"/v4/lists/{listId}");
        }

        public Recipient CreateRecipient(RecipientCreateInfo recipient)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(RecipientCreateInfo), value: recipient);
            return _requestHelper.Execute<Recipient>(method: Method.POST, resource: $"/v4/lists/{recipient.ListId}/recipients", payload: new { NumberInfo = recipient.NumberInfo, Fields = recipient.Fields });
        }

        public List<Recipient> GetAllRecipientsInList(RecipientListId listId)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            return _requestHelper.ExecuteGetAndIteratePagedResult<Recipient>(resource: $"/v4/lists/{listId}/recipients?pageLimit=250");
        }

        public Recipient GetRecipientById(RecipientListId listId, RecipientId recipientId)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            EnsureNonEmptyOrThrow(parameterName: nameof(recipientId), value: recipientId);
            return _requestHelper.Execute<Recipient>(method: Method.GET, resource: $"/v4/lists/{listId}/recipients/{recipientId}");
        }

        public Recipient GetRecipientByNumber(RecipientListId listId, NumberInfo numberInfo)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            EnsureNonEmptyOrThrow(parameterName: nameof(numberInfo), value: numberInfo);
            return _requestHelper.Execute<Recipient>(method: Method.GET, resource: $"/v4/lists/{listId}/recipients/bynumber?countryCode={numberInfo.CountryCode}&phoneNumber={numberInfo.PhoneNumber}");
        }

        public Recipient UpdateRecipient(IRecipientUpdateInfo recipient)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(recipient), value: recipient);
            return UpdateRecipientInternal(listId: recipient.ListId, recipientId: recipient.Id, updateObject: new
            {
                NumberInfo = recipient.NumberInfo,
                Fields = recipient.Fields
            });
        }

        private Recipient UpdateRecipientInternal(RecipientListId listId, RecipientId recipientId, object updateObject)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            EnsureNonEmptyOrThrow(parameterName: nameof(recipientId), value: recipientId);
            EnsureNonEmptyOrThrow(parameterName: nameof(updateObject), value: updateObject);
            return _requestHelper.Execute<Recipient>(method: Method.PUT, resource: $"/v4/lists/{listId}/recipients/{recipientId}", payload: updateObject);
        }

        public void DeleteAllRecipientsInList(RecipientListId listId)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            _requestHelper.ExecuteWithNoContent(method: Method.DELETE, resource: $"/v4/lists/{listId}/recipients/all");
        }

        public void DeleteRecipientById(RecipientListId listId, RecipientId recipientId)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            EnsureNonEmptyOrThrow(parameterName: nameof(recipientId), value: recipientId);
            _requestHelper.ExecuteWithNoContent(method: Method.DELETE, resource: $"/v4/lists/{listId}/recipients/{recipientId}");
        }

        public void DeleteRecipientByNumber(RecipientListId listId, NumberInfo numberInfo)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            EnsureNonEmptyOrThrow(parameterName: nameof(numberInfo), value: numberInfo);
            _requestHelper.ExecuteWithNoContent(method: Method.DELETE, resource: $"/v4/lists/{listId}/recipients/bynumber?countryCode={numberInfo.CountryCode}&phoneNumber={numberInfo.PhoneNumber}");
        }

        private void EnsureNonEmptyOrThrow(string parameterName, object? value)
        {
            if(value == null)
                throw new ArgumentException($"'{parameterName}' cannot be null.", nameof(value));
        }
    }
}
