namespace ActiveSync.Core.ResponseObjects.FolderSync
{
    public enum eFolderUpdateStatus : byte
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
        ServerError = 6,
        /// <summary>
        /// The client sent a malformed or mismatched synchronization key, or the synchronization state is corrupted on the server.
        /// Resolution: Issue a FolderSync command request with a synchronization key of 0.
        /// </summary>
        SyncKeyError = 9,
        /// <summary>
        /// The client sent a FolderUpdate command request that contains a semantic error.
        /// Resolution: Double-check the request for accuracy.
        /// </summary>
        MalformedRequest = 10,
        /// <summary>
        /// Unusual back-end issue.
        /// </summary>
        UnknownError = 11

    }
}