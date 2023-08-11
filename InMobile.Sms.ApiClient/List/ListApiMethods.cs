using System;
using System.Collections.Generic;


namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// A collection of list- and recipient-specific api endpoints.
    /// </summary>
    public interface IListApiMethods
    {
        /// <summary>
        /// Create a new list.
        /// </summary>
        /// <param name="createInfo"></param>
        /// <returns></returns>
        RecipientList CreateList(RecipientListCreateInfo createInfo);

        /// <summary>
        /// Get all existing list on account.
        /// </summary>
        /// <returns></returns>
        List<RecipientList> GetAllLists();

        /// <summary>
        /// Get a specific list by its id.
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        RecipientList GetListById(RecipientListId listId);

        /// <summary>
        /// Updates the given list. This call both allows for the client to retrieve a listEntry, update it and pass it to UpdateList. It also allows for updating a list without retrieving it first simply by calling UpdateList(new RecipientListUpdateInfo(...) });
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        RecipientList UpdateList(IRecipientListUpdateInfo list);

        /// <summary>
        /// Deletes a list including all of its recipients.
        /// </summary>
        /// <param name="listId"></param>
        void DeleteListById(RecipientListId listId);

        /// <summary>
        /// Create a new recipient in a given list.
        /// </summary>
        /// <param name="recipient"></param>
        /// <returns></returns>
        Recipient CreateRecipient(RecipientCreateInfo recipient);

        /// <summary>
        /// Get all recipients in a given list.
        /// </summary>
        /// <param name="listId">The id of the list of which all recipients should be returned.</param>
        /// <returns></returns>
        List<Recipient> GetAllRecipientsInList(RecipientListId listId);

        /// <summary>
        /// Get a recipient by its id.
        /// </summary>
        /// <param name="listId">The id of the list.</param>
        /// <param name="recipientId">The id of the recipient.</param>
        /// <returns></returns>
        Recipient GetRecipientById(RecipientListId listId, RecipientId recipientId);

        /// <summary>
        /// Get a recipient by its number.
        /// </summary>
        /// <param name="listId">The id of the list.</param>
        /// <param name="numberInfo">The number information.</param>
        /// <returns></returns>
        Recipient GetRecipientByNumber(RecipientListId listId, NumberInfo numberInfo);

        /// <summary>
        /// Update a recipient. It is possible to return a previously fetched recipient object here or make an update without loading the recipient first by calling UpdateRecipient(new RecipientUpdateInfo(...))
        /// </summary>
        /// <param name="recipient"></param>
        /// <returns></returns>
        Recipient UpdateRecipient(IRecipientUpdateInfo recipient);

        /// <summary>
        /// Delet a recipient by its id.
        /// </summary>
        /// <param name="listId">The id of the list in which the recipient belongs.</param>
        /// <param name="recipientId"></param>
        void DeleteRecipientById(RecipientListId listId, RecipientId recipientId);

        /// <summary>
        /// Deletes a recipient by its number information.
        /// </summary>
        /// <param name="listId">The id of the list in which the recipient belongs.</param>
        /// <param name="numberInfo">The number information.</param>
        void DeleteRecipientByNumber(RecipientListId listId, NumberInfo numberInfo);

        /// <summary>
        /// Deletes all recipients in a given list.
        /// </summary>
        /// <param name="listId"></param>
        void DeleteAllRecipientsInList(RecipientListId listId);
    }

    internal class ListApiMethods : IListApiMethods
    {
        private const string V4_lists = "/v4/lists";

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
                        resource: $"{V4_lists}",
                        payload: createInfo);
        }

        public List<RecipientList> GetAllLists()
        {
            return _requestHelper.ExecuteGetAndIteratePagedResult<RecipientList>(resource: $"{V4_lists}?pageLimit=250");
        }

        public RecipientList GetListById(RecipientListId listId)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            return _requestHelper.Execute<RecipientList>(method: Method.GET, resource: $"{V4_lists}/{listId}");
        }

        public RecipientList UpdateList(IRecipientListUpdateInfo list)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(list), value: list);
            return UpdateListInternal(listId: list.Id, updateObject: new { name = list.Name });
        }
        private RecipientList UpdateListInternal(RecipientListId listId, object updateObject)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            return _requestHelper.Execute<RecipientList>(method: Method.PUT, resource: $"{V4_lists}/{listId}", payload: updateObject);
        }

        public void DeleteListById(RecipientListId listId)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            _requestHelper.ExecuteWithNoContent(method: Method.DELETE, resource: $"{V4_lists}/{listId}");
        }

        public Recipient CreateRecipient(RecipientCreateInfo recipient)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(RecipientCreateInfo), value: recipient);
            return _requestHelper.Execute<Recipient>(method: Method.POST, resource: $"{V4_lists}/{recipient.ListId}/recipients", payload: new { NumberInfo = recipient.NumberInfo, Fields = recipient.Fields });
        }

        public List<Recipient> GetAllRecipientsInList(RecipientListId listId)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            return _requestHelper.ExecuteGetAndIteratePagedResult<Recipient>(resource: $"{V4_lists}/{listId}/recipients?pageLimit=250");
        }

        public Recipient GetRecipientById(RecipientListId listId, RecipientId recipientId)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            EnsureNonEmptyOrThrow(parameterName: nameof(recipientId), value: recipientId);
            return _requestHelper.Execute<Recipient>(method: Method.GET, resource: $"{V4_lists}/{listId}/recipients/{recipientId}");
        }

        public Recipient GetRecipientByNumber(RecipientListId listId, NumberInfo numberInfo)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            EnsureNonEmptyOrThrow(parameterName: nameof(numberInfo), value: numberInfo);
            return _requestHelper.Execute<Recipient>(method: Method.GET, resource: $"{V4_lists}/{listId}/recipients/bynumber?countryCode={numberInfo.CountryCode}&phoneNumber={numberInfo.PhoneNumber}");
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
            return _requestHelper.Execute<Recipient>(method: Method.PUT, resource: $"{V4_lists}/{listId}/recipients/{recipientId}", payload: updateObject);
        }

        public void DeleteAllRecipientsInList(RecipientListId listId)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            _requestHelper.ExecuteWithNoContent(method: Method.DELETE, resource: $"{V4_lists}/{listId}/recipients/all");
        }

        public void DeleteRecipientById(RecipientListId listId, RecipientId recipientId)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            EnsureNonEmptyOrThrow(parameterName: nameof(recipientId), value: recipientId);
            _requestHelper.ExecuteWithNoContent(method: Method.DELETE, resource: $"{V4_lists}/{listId}/recipients/{recipientId}");
        }

        public void DeleteRecipientByNumber(RecipientListId listId, NumberInfo numberInfo)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(listId), value: listId);
            EnsureNonEmptyOrThrow(parameterName: nameof(numberInfo), value: numberInfo);
            _requestHelper.ExecuteWithNoContent(method: Method.DELETE, resource: $"{V4_lists}/{listId}/recipients/bynumber?countryCode={numberInfo.CountryCode}&phoneNumber={numberInfo.PhoneNumber}");
        }

        private void EnsureNonEmptyOrThrow(string parameterName, object? value)
        {
            if(value == null)
                throw new ArgumentException($"'{parameterName}' cannot be null.", nameof(value));
        }
    }
}
