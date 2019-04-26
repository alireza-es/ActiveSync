namespace ActiveSync.Core.ResponseObjects.FolderSync
{
    public enum eFolderDeleteStatus : byte
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
        ServerError = 6,
        /// <summary>
        /// The client sent a malformed or mismatched synchronization key, or the synchronization state is corrupted on the server.
        /// Resolution: Issue a FolderSync command request with a synchronization key of 0.
        /// </summary>
        SyncKeyError = 9,
        /// <summary>
        /// The client sent a FolderDelete command request (section 2.2.2.3) that contains a semantic or syntactic error.
        /// Resolution: Double-check the request for accuracy.
        /// </summary>
        MalformedRequest = 10,
        /// <summary>
        /// Unusual back-end issue.
        /// </summary>
        UnknownError = 11
    }
}