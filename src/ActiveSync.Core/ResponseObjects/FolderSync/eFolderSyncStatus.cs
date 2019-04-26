namespace ActiveSync.Core.ResponseObjects.FolderSync
{
    public enum eFolderSyncStatus : byte
    {
        /// <summary>
        /// Server successfully completed command.
        /// </summary>
        Success = 1,
        /// <summary>
        /// Server misconfiguration, temporary system issue, or bad item. This is frequently a transient condition.
        /// Resolution: Retry the FolderUpdate command. If continued attempts to synchronization fail, consider returning to synchronization key 0.
        /// </summary>
        ServerError = 6,
        /// <summary>
        /// he client sent a malformed or mismatched synchronization key, or the synchronization state is corrupted on the server.
        /// Resolution: Delete items added since last synchronization and return to synchronization key zero (0).
        /// </summary>
        SyncKeyError = 9,
        /// <summary>
        /// The client sent a FolderSync command request that contains a semantic or syntactic error.
        /// Resolution: Double-check the request for accuracy.
        /// </summary>
        MalformedRequest = 10,
        /// <summary>
        /// Server misconfiguration, temporary system issue, or bad item. This is frequently a transient condition.
        /// Retry the FolderSync command. If continued attempts to synchronization fail, consider returning to synchronization key zero (0).
        /// </summary>
        UnknownError = 11,
        /// <summary>
        /// Unusual back-end issue.
        /// </summary>
        CodeUnknown = 12
    }
}