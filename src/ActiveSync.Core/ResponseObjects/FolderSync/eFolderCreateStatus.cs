namespace ActiveSync.Core.ResponseObjects.FolderSync
{
    public enum eFolderCreateStatus : byte
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
        /// <summary>
        /// The client sent a malformed or mismatched synchronization key, or the synchronization state is corrupted on the server.
        /// Resolution: Delete folders added since last synchronization and return to synchronization key to zero (0).
        /// </summary>
        SyncKeyError = 9,
        /// <summary>
        /// The client sent a FolderCreate command request (section 2.2.2.2) that contains a semantic error, or the client attempted to create a default folder, such as the Inbox folder, Outbox folder, or Contacts folder.
        /// Resolution: Double-check the request for accuracy.
        /// </summary>
        MalformedRequest = 10,
        /// <summary>
        /// Unknown
        /// </summary>
        UnknownError = 11,
        /// <summary>
        /// Unusual back-end issue.
        /// </summary>
        CodeUnknown = 12
    }
}