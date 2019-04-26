using System.Collections.Generic;
using ActiveSync.SyncContract.Syncables;

namespace ActiveSync.SyncContract.Service
{
    public interface IContactService
    {
        IList<SyncableContact> GetContacts(UserCredential userCredential,string folderId);
        SyncableContact FetchContact(UserCredential userCredential, string id);
        UpdateContactResult UpdateContact(UserCredential userCredential, SyncableContact contact);
        AddContactResult AddContact(UserCredential userCredential, SyncableContact contact, out string serverId);
        DeleteContactResult DeleteContact(UserCredential userCredential, string id);

    }

    #region Command Result

    public enum AddContactResult : byte
    {
        /// <summary>
        /// Server successfully completed command.
        /// => Global
        /// </summary>
        Success = 1,
        /// <summary>
        /// Server misconfiguration, temporary system issue, or bad item. This is frequently a transient condition.
        /// => Global
        /// Retry the synchronization. If continued attempts to synchronization fail, consider returning to synchronization key 0.
        /// </summary>
        ServerError = 5
    }

    public enum UpdateContactResult : byte
    {
        /// <summary>
        /// Server successfully completed command.
        /// => Global
        /// </summary>
        Success = 1,
        /// <summary>
        /// Server misconfiguration, temporary system issue, or bad item. This is frequently a transient condition.
        /// => Global
        /// Retry the synchronization. If continued attempts to synchronization fail, consider returning to synchronization key 0.
        /// </summary>
        ServerError = 5,
        /// <summary>
        /// The client issued a fetch or change operation that has an ItemID (section 2.2.3.84) value that is no longer valid on the server (for example, the item was deleted).
        /// => Item
        /// Issue a synchronization request and prompt the user if necessary.
        /// </summary>
        ObjectNotFound = 8,

    }

    public enum DeleteContactResult : byte
    {
        /// <summary>
        /// Server successfully completed command.
        /// => Global
        /// </summary>
        Success = 1,
        /// <summary>
        /// Server misconfiguration, temporary system issue, or bad item. This is frequently a transient condition.
        /// => Global
        /// Retry the synchronization. If continued attempts to synchronization fail, consider returning to synchronization key 0.
        /// </summary>
        ServerError = 5,
        /// <summary>
        /// The client issued a fetch or change operation that has an ItemID (section 2.2.3.84) value that is no longer valid on the server (for example, the item was deleted).
        /// => Item
        /// Issue a synchronization request and prompt the user if necessary.
        /// </summary>
        ObjectNotFound = 8
    }

    public enum FetchContactResult : byte
    {
        /// <summary>
        /// Server successfully completed command.
        /// => Global
        /// </summary>
        Success = 1,
        /// <summary>
        /// Server misconfiguration, temporary system issue, or bad item. This is frequently a transient condition.
        /// => Global
        /// Retry the synchronization. If continued attempts to synchronization fail, consider returning to synchronization key 0.
        /// </summary>
        ServerError = 5,
        /// <summary>
        /// The client issued a fetch or change operation that has an ItemID (section 2.2.3.84) value that is no longer valid on the server (for example, the item was deleted).
        /// => Item
        /// Issue a synchronization request and prompt the user if necessary.
        /// </summary>
        ObjectNotFound = 8
    }

    #endregion
}
