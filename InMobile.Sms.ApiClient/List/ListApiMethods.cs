using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        /// Create a new list (async).
        /// </summary>
        /// <param name="createInfo"></param>
        /// <returns></returns>
        Task<RecipientList> CreateListAsync(RecipientListCreateInfo createInfo);

        /// <summary>
        /// Get all existing list on account.
        /// </summary>
        /// <returns></returns>
        List<RecipientList> GetAllLists();

        /// <summary>
        /// Get all existing list on account (async).
        /// </summary>
        /// <returns></returns>
        Task<List<RecipientList>> GetAllListsAsync();

        /// <summary>
        /// Get a specific list by its id.
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        RecipientList GetListById(RecipientListId listId);

        /// <summary>
        /// Get a specific list by its id (async).
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        Task<RecipientList> GetListByIdAsync(RecipientListId listId);

        /// <summary>
        /// Updates the given list. This call both allows for the client to retrieve a listEntry, update it and pass it to UpdateList.
        /// It also allows for updating a list without retrieving it first simply by calling UpdateList(new RecipientListUpdateInfo(...) });
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        RecipientList UpdateList(IRecipientListUpdateInfo list);

        /// <summary>
        /// Updates the given list. This call both allows for the client to retrieve a listEntry, update it and pass it to UpdateList.
        /// It also allows for updating a list without retrieving it first simply by calling UpdateList(new RecipientListUpdateInfo(...) }) (async);
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<RecipientList> UpdateListAsync(IRecipientListUpdateInfo list);

        /// <summary>
        /// Deletes a list including all of its recipients.
        /// </summary>
        /// <param name="listId"></param>
        void DeleteListById(RecipientListId listId);

        /// <summary>
        /// Deletes a list including all of its recipients (async).
        /// </summary>
        /// <param name="listId"></param>
        Task DeleteListByIdAsync(RecipientListId listId);

        /// <summary>
        /// Create a new recipient in a given list.
        /// </summary>
        /// <param name="recipient"></param>
        /// <returns></returns>
        Recipient CreateRecipient(RecipientCreateInfo recipient);

        /// <summary>
        /// Create a new recipient in a given list (async).
        /// </summary>
        /// <param name="recipient"></param>
        /// <returns></returns>
        Task<Recipient> CreateRecipientAsync(RecipientCreateInfo recipient);

        /// <summary>
        /// Get all recipients in a given list.
        /// </summary>
        /// <param name="listId">The id of the list of which all recipients should be returned.</param>
        /// <returns></returns>
        List<Recipient> GetAllRecipientsInList(RecipientListId listId);

        /// <summary>
        /// Get all recipients in a given list (async).
        /// </summary>
        /// <param name="listId">The id of the list of which all recipients should be returned.</param>
        /// <returns></returns>
        Task<List<Recipient>> GetAllRecipientsInListAsync(RecipientListId listId);

        /// <summary>
        /// Get a recipient by its id.
        /// </summary>
        /// <param name="listId">The id of the list.</param>
        /// <param name="recipientId">The id of the recipient.</param>
        /// <returns></returns>
        Recipient GetRecipientById(RecipientListId listId, RecipientId recipientId);

        /// <summary>
        /// Get a recipient by its id (async).
        /// </summary>
        /// <param name="listId">The id of the list.</param>
        /// <param name="recipientId">The id of the recipient.</param>
        /// <returns></returns>
        Task<Recipient> GetRecipientByIdAsync(RecipientListId listId, RecipientId recipientId);

        /// <summary>
        /// Get a recipient by its number.
        /// </summary>
        /// <param name="listId">The id of the list.</param>
        /// <param name="numberInfo">The number information.</param>
        /// <returns></returns>
        Recipient GetRecipientByNumber(RecipientListId listId, NumberInfo numberInfo);

        /// <summary>
        /// Get a recipient by its number (async).
        /// </summary>
        /// <param name="listId">The id of the list.</param>
        /// <param name="numberInfo">The number information.</param>
        /// <returns></returns>
        Task<Recipient> GetRecipientByNumberAsync(RecipientListId listId, NumberInfo numberInfo);

        /// <summary>
        /// Update a recipient. It is possible to return a previously fetched recipient object here or make an update without loading the recipient first by calling UpdateRecipient(new RecipientUpdateInfo(...)).
        /// </summary>
        /// <param name="recipient"></param>
        /// <returns></returns>
        Recipient UpdateRecipient(IRecipientUpdateInfo recipient);

        /// <summary>
        /// Update a recipient. It is possible to return a previously fetched recipient object here or make an update without loading the recipient first by calling UpdateRecipient(new RecipientUpdateInfo(...)) (async).
        /// </summary>
        /// <param name="recipient"></param>
        /// <returns></returns>
        Task<Recipient> UpdateRecipientAsync(IRecipientUpdateInfo recipient);

        /// <summary>
        /// Deletes a recipient by its id.
        /// </summary>
        /// <param name="listId">The id of the list in which the recipient belongs.</param>
        /// <param name="recipientId"></param>
        void DeleteRecipientById(RecipientListId listId, RecipientId recipientId);

        /// <summary>
        /// Deletes a recipient by its id (async).
        /// </summary>
        /// <param name="listId">The id of the list in which the recipient belongs.</param>
        /// <param name="recipientId"></param>
        Task DeleteRecipientByIdAsync(RecipientListId listId, RecipientId recipientId);

        /// <summary>
        /// Deletes a recipient by its number information.
        /// </summary>
        /// <param name="listId">The id of the list in which the recipient belongs.</param>
        /// <param name="numberInfo">The number information.</param>
        void DeleteRecipientByNumber(RecipientListId listId, NumberInfo numberInfo);

        /// <summary>
        /// Deletes a recipient by its number information (async).
        /// </summary>
        /// <param name="listId">The id of the list in which the recipient belongs.</param>
        /// <param name="numberInfo">The number information.</param>
        Task DeleteRecipientByNumberAsync(RecipientListId listId, NumberInfo numberInfo);

        /// <summary>
        /// Deletes all recipients in a given list.
        /// </summary>
        /// <param name="listId"></param>
        void DeleteAllRecipientsInList(RecipientListId listId);

        /// <summary>
        /// Deletes all recipients in a given list (async).
        /// </summary>
        /// <param name="listId"></param>
        Task DeleteAllRecipientsInListAsync(RecipientListId listId);
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
            => CreateListInternal(createInfo: createInfo, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<RecipientList> CreateListAsync(RecipientListCreateInfo createInfo)
            => CreateListInternal(createInfo: createInfo, mode: SyncMode.Async);

        private async Task<RecipientList> CreateListInternal(RecipientListCreateInfo createInfo, SyncMode mode)
        {
            if (createInfo is null)
                throw new ArgumentNullException(nameof(createInfo));

            const Method method = Method.POST;
            const string resource = V4_lists;

            return mode == SyncMode.Sync
                ? _requestHelper.Execute<RecipientList>(method: method, resource: resource, payload: createInfo)
                : await _requestHelper.ExecuteAsync<RecipientList>(method: method, resource: resource, payload: createInfo);
        }

        public List<RecipientList> GetAllLists()
            => GetAllListsInternal(mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<List<RecipientList>> GetAllListsAsync()
            => GetAllListsInternal(mode: SyncMode.Async);

        private async Task<List<RecipientList>> GetAllListsInternal(SyncMode mode)
        {
            var resource = $"{V4_lists}?pageLimit=250";

            return mode == SyncMode.Sync
                ? _requestHelper.ExecuteGetAndIteratePagedResult<RecipientList>(resource: resource)
                : await _requestHelper.ExecuteGetAndIteratePagedResultAsync<RecipientList>(resource: resource);
        }

        public RecipientList GetListById(RecipientListId listId)
            => GetListByIdInternal(listId: listId, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<RecipientList> GetListByIdAsync(RecipientListId listId)
            => GetListByIdInternal(listId: listId, mode: SyncMode.Async);

        private async Task<RecipientList> GetListByIdInternal(RecipientListId listId, SyncMode mode)
        {
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(listId), value: listId);

            const Method method = Method.GET;
            var resource = $"{V4_lists}/{listId}";

            return mode == SyncMode.Sync
                ? _requestHelper.Execute<RecipientList>(method: method, resource: resource)
                : await _requestHelper.ExecuteAsync<RecipientList>(method: method, resource: resource);
        }

        public RecipientList UpdateList(IRecipientListUpdateInfo list)
            => UpdateListInternal(list: list, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<RecipientList> UpdateListAsync(IRecipientListUpdateInfo list)
            => UpdateListInternal(list: list, mode: SyncMode.Async);

        private async Task<RecipientList> UpdateListInternal(IRecipientListUpdateInfo list, SyncMode mode)
        {
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(list), value: list);
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(list.Id), value: list.Id);

            const Method method = Method.PUT;
            var resource = $"{V4_lists}/{list.Id}";
            var payload = new { name = list.Name };

            return mode == SyncMode.Sync
                ? _requestHelper.Execute<RecipientList>(method: method, resource: resource, payload: payload)
                : await _requestHelper.ExecuteAsync<RecipientList>(method: method, resource: resource, payload: payload);
        }

        public void DeleteListById(RecipientListId listId)
            => DeleteListByIdInternal(listId: listId, SyncMode.Sync).GetAwaiter().GetResult();

        public Task DeleteListByIdAsync(RecipientListId listId)
            => DeleteListByIdInternal(listId: listId, SyncMode.Async);

        private async Task DeleteListByIdInternal(RecipientListId listId, SyncMode mode)
        {
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(listId), value: listId);

            const Method method = Method.DELETE;
            var resource = $"{V4_lists}/{listId}";

            if (mode == SyncMode.Sync)
                _requestHelper.ExecuteWithNoContent(method: method, resource: resource);
            else
                await _requestHelper.ExecuteWithNoContentAsync(method: method, resource: resource);
        }

        public Recipient CreateRecipient(RecipientCreateInfo recipient)
            => CreateRecipientInternal(recipient: recipient, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<Recipient> CreateRecipientAsync(RecipientCreateInfo recipient)
            => CreateRecipientInternal(recipient: recipient, mode: SyncMode.Async);

        private async Task<Recipient> CreateRecipientInternal(RecipientCreateInfo recipient, SyncMode mode)
        {
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(recipient), value: recipient);

            const Method method = Method.POST;
            var resource = $"{V4_lists}/{recipient.ListId}/recipients";
            var payload = new { NumberInfo = recipient.NumberInfo, Fields = recipient.Fields, ExternalCreated = recipient.ExternalCreated?.ToUniversalTime() };

            return mode == SyncMode.Sync
                ? _requestHelper.Execute<Recipient>(method: method, resource: resource, payload: payload)
                : await _requestHelper.ExecuteAsync<Recipient>(method: method, resource: resource, payload: payload);
        }

        public List<Recipient> GetAllRecipientsInList(RecipientListId listId)
            => GetAllRecipientsInListInternal(listId: listId, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<List<Recipient>> GetAllRecipientsInListAsync(RecipientListId listId)
            => GetAllRecipientsInListInternal(listId: listId, mode: SyncMode.Async);

        private async Task<List<Recipient>> GetAllRecipientsInListInternal(RecipientListId listId, SyncMode mode)
        {
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(listId), value: listId);

            var resource = $"{V4_lists}/{listId}/recipients?pageLimit=250";

            return mode == SyncMode.Sync
                ? _requestHelper.ExecuteGetAndIteratePagedResult<Recipient>(resource: resource)
                : await _requestHelper.ExecuteGetAndIteratePagedResultAsync<Recipient>(resource: resource);
        }

        public Recipient GetRecipientById(RecipientListId listId, RecipientId recipientId)
            => GetRecipientByIdInternal(listId: listId, recipientId: recipientId, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<Recipient> GetRecipientByIdAsync(RecipientListId listId, RecipientId recipientId)
            => GetRecipientByIdInternal(listId: listId, recipientId: recipientId, mode: SyncMode.Async);

        private async Task<Recipient> GetRecipientByIdInternal(RecipientListId listId, RecipientId recipientId, SyncMode mode)
        {
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(listId), value: listId);
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(recipientId), value: recipientId);

            const Method method = Method.GET;
            var resource = $"{V4_lists}/{listId}/recipients/{recipientId}";

            return mode == SyncMode.Sync
                ? _requestHelper.Execute<Recipient>(method: method, resource: resource)
                : await _requestHelper.ExecuteAsync<Recipient>(method: method, resource: resource);
        }

        public Recipient GetRecipientByNumber(RecipientListId listId, NumberInfo numberInfo)
            => GetRecipientByNumberInternal(listId: listId, numberInfo: numberInfo, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<Recipient> GetRecipientByNumberAsync(RecipientListId listId, NumberInfo numberInfo)
            => GetRecipientByNumberInternal(listId: listId, numberInfo: numberInfo, mode: SyncMode.Async);

        private async Task<Recipient> GetRecipientByNumberInternal(RecipientListId listId, NumberInfo numberInfo, SyncMode mode)
        {
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(listId), value: listId);
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(numberInfo), value: numberInfo);

            const Method method = Method.GET;
            var resource = $"{V4_lists}/{listId}/recipients/bynumber?countryCode={numberInfo.CountryCode}&phoneNumber={numberInfo.PhoneNumber}";

            return mode == SyncMode.Sync
                ? _requestHelper.Execute<Recipient>(method: method, resource: resource)
                : await _requestHelper.ExecuteAsync<Recipient>(method: method, resource: resource);
        }

        public Recipient UpdateRecipient(IRecipientUpdateInfo recipient)
            => UpdateRecipientInternal(recipient: recipient, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<Recipient> UpdateRecipientAsync(IRecipientUpdateInfo recipient)
            => UpdateRecipientInternal(recipient: recipient, mode: SyncMode.Async);

        private async Task<Recipient> UpdateRecipientInternal(IRecipientUpdateInfo recipient, SyncMode mode)
        {
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(recipient), value: recipient);
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(recipient.ListId), value: recipient.ListId);
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(recipient.Id), value: recipient.Id);

            const Method method = Method.PUT;
            var resource = $"{V4_lists}/{recipient.ListId}/recipients/{recipient.Id}";
            var payload = new { NumberInfo = recipient.NumberInfo, Fields = recipient.Fields };

            return mode == SyncMode.Sync
                ? _requestHelper.Execute<Recipient>(method: method, resource: resource, payload: payload)
                : await _requestHelper.ExecuteAsync<Recipient>(method: method, resource: resource, payload: payload);
        }

        public void DeleteAllRecipientsInList(RecipientListId listId)
            => DeleteAllRecipientsInList(listId: listId, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task DeleteAllRecipientsInListAsync(RecipientListId listId)
            => DeleteAllRecipientsInList(listId: listId, mode: SyncMode.Async);

        private async Task DeleteAllRecipientsInList(RecipientListId listId, SyncMode mode)
        {
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(listId), value: listId);

            const Method method = Method.DELETE;
            var resource = $"{V4_lists}/{listId}/recipients/all";

            if (mode == SyncMode.Sync)
                _requestHelper.ExecuteWithNoContent(method: method, resource: resource);
            else
                await _requestHelper.ExecuteWithNoContentAsync(method: method, resource: resource);
        }

        public void DeleteRecipientById(RecipientListId listId, RecipientId recipientId)
            => DeleteRecipientByIdInternal(listId: listId, recipientId: recipientId, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task DeleteRecipientByIdAsync(RecipientListId listId, RecipientId recipientId)
            => DeleteRecipientByIdInternal(listId: listId, recipientId: recipientId, mode: SyncMode.Async);

        private async Task DeleteRecipientByIdInternal(RecipientListId listId, RecipientId recipientId, SyncMode mode)
        {
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(listId), value: listId);
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(recipientId), value: recipientId);

            const Method method = Method.DELETE;
            var resource = $"{V4_lists}/{listId}/recipients/{recipientId}";

            if (mode == SyncMode.Sync)
                _requestHelper.ExecuteWithNoContent(method: method, resource: resource);
            else
                await _requestHelper.ExecuteWithNoContentAsync(method: method, resource: resource);
        }

        public void DeleteRecipientByNumber(RecipientListId listId, NumberInfo numberInfo)
            => DeleteRecipientByNumberInternal(listId: listId, numberInfo: numberInfo, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task DeleteRecipientByNumberAsync(RecipientListId listId, NumberInfo numberInfo)
            => DeleteRecipientByNumberInternal(listId: listId, numberInfo: numberInfo, mode: SyncMode.Async);

        private async Task DeleteRecipientByNumberInternal(RecipientListId listId, NumberInfo numberInfo, SyncMode mode)
        {
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(listId), value: listId);
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(numberInfo), value: numberInfo);

            const Method method = Method.DELETE;
            var resource = $"{V4_lists}/{listId}/recipients/bynumber?countryCode={numberInfo.CountryCode}&phoneNumber={numberInfo.PhoneNumber}";

            if (mode == SyncMode.Sync)
                _requestHelper.ExecuteWithNoContent(method: method, resource: resource);
            else
                await _requestHelper.ExecuteWithNoContentAsync(method: method, resource: resource);
        }
    }
}