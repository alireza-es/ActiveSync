using System.Collections.Generic;
using ActiveSync.SyncContract.Syncables;

namespace ActiveSync.SyncContract.Service
{
    public interface IFolderService
    {
        CreateFolderResult CreateFolder(UserCredential userCredential, SyncableFolder syncFolder, out string createdFolderId);
        UpdateFolderResult UpdateFolder(UserCredential userCredential, SyncableFolder syncFolder);
        DeleteFolderResult DeleteFolder(UserCredential userCredential, string id);
        SyncableFolder GetFolder(UserCredential userCredential, string id);
        IList<SyncableFolder> GetAllFolders(UserCredential userCredential);
        void EmptyFolderContents(UserCredential userCredential, string folderId);
    }

    #region Command Results

    public enum CreateFolderResult : byte
    {
        /// <summary>
        /// Server successfully completed command.
        /// </summary>
        Success = 1,
        /// <summary>
        /// The parent folder already contains a folder that has this name.
        /// Resolution: Prompt user to supply a unique name.
        /// </summary>
        FolderAlreadyExist = 2,
        /// <summary>
        /// The specified parent folder is the Recipient information folder.
        /// Create the folder under a different parent.
        /// </summary>
        SystemFolder = 3,
        /// <summary>
        /// The parent folder does not exist on the server, possibly because it has been deleted or renamed.
        /// Resolution: Issue a FolderSync command (section 2.2.2.4) for the new hierarchy and prompt the user for a new parent folder.
        /// </summary>
        ParentNotFound = 5,
        /// <summary>
        /// Server misconfiguration, temporary system issue, or bad item. This is frequently a transient condition.
        /// Resolution: Retry the FolderSync command. If continued attempts to synchronization fail, consider returning to synchronization key zero (0).
        /// </summary>
        ServerError = 6,
    }

    public enum UpdateFolderResult : byte
    {
        /// <summary>
        /// Server successfully completed command.
        /// </summary>
        Success = 1,
        /// <summary>
        /// A folder with that name already exists or the specified folder is a special folder, such as the Inbox, Outbox, Contacts, or Drafts folders. Special folders cannot be updated.
        /// </summary>
        FolderAlreadyExist = 2,
        /// <summary>
        /// The client specified the Recipient information folder, which is a special folder. Special folders cannot be updated.
        /// </summary>
        SystemFolder = 3,
        /// <summary>
        /// Client specified a nonexistent folder in a FolderUpdate command request.
        /// Resolution: Issue a FolderSync command (section 2.2.2.4) for the new hierarchy.
        /// </summary>
        FolderNotExist = 4,
        /// <summary>
        /// Client specified a nonexistent folder in a FolderUpdate command request.
        /// Resolution: Issue a FolderSync command for the new hierarchy.
        /// </summary>
        ParentNotFound = 5,
        /// <summary>
        /// Server misconfiguration, temporary system issue, or bad item. This is frequently a transient condition.
        /// Resolution: Retry the FolderUpdate command. If continued attempts to synchronization fail, consider returning to synchronization key 0.
        /// </summary>
        ServerError = 6
    }

    public enum DeleteFolderResult : byte
    {
        /// <summary>
        /// Server successfully completed command.
        /// </summary>
        Success = 1,
        /// <summary>
        /// The specified folder is a special system folder, such as the Inbox folder, Outbox folder, Contacts folder, Recipient information, or Drafts folder, and cannot be deleted by the client.
        /// The client specified a special folder in a FolderDelete command request (section 2.2.2.3). special folders cannot be deleted.
        /// </summary>
        SystemFolder = 3,
        /// <summary>
        /// The client specified a nonexistent folder in a FolderDelete command request.
        /// Resolution: Issue a FolderSync command (section 2.2.2.4) for the new hierarchy.
        /// </summary>
        FolderNotExist = 4,
        /// <summary>
        /// Server misconfiguration, temporary system issue, or bad item. This is frequently a transient condition.
        /// Resolution: Retry the FolderDelete command. If continued attempts to synchronization fail, consider returning to synchronization key zero (0).
        /// </summary>
        ServerError = 6
    }
    #endregion
}
