namespace ActiveSync.Core.ResponseObjects.Sync
{
    public enum eSyncStatus : byte
    {
        /// <summary>
        /// Server successfully completed command.
        /// => Global
        /// </summary>
        Success = 1,
        /// <summary>
        /// Invalid or mismatched synchronization key. —or— Synchronization state corrupted on server.
        /// => Global
        /// MUST return to SyncKey element value (section 2.2.3.166.4) of 0 for the collection. The client SHOULD either delete any items that were added since the last successful Sync or the client MUST add those items back to the server after completing the full resynchronization.
        /// </summary>
        InvalidSyncKey = 3,
        /// <summary>
        /// There was a semantic error in the synchronization request. The client is issuing a request that does not comply with the specification requirements.
        /// => Item or Global
        /// Double-check the request for accuracy and retry the Sync command.
        /// </summary>
        ProtocolError = 4,
        /// <summary>
        /// Server misconfiguration, temporary system issue, or bad item. This is frequently a transient condition.
        /// => Global
        /// Retry the synchronization. If continued attempts to synchronization fail, consider returning to synchronization key 0.
        /// </summary>
        ServerError = 5,
        /// <summary>
        /// The client has sent a malformed or invalid item.
        /// => Item
        /// Stop sending the item. This is not a transient condition.
        /// </summary>
        ConversionError = 6,
        /// <summary>
        /// The client has changed an item for which the conflict policy indicates that the server's changes take precedence.
        /// => Item
        /// If it is necessary, inform the user that the change they made to the item has been overwritten by a server change.
        /// </summary>
        ConflictMatchingError = 7,
        /// <summary>
        /// The client issued a fetch or change operation that has an ItemID (section 2.2.3.84) value that is no longer valid on the server (for example, the item was deleted).
        /// => Item
        /// Issue a synchronization request and prompt the user if necessary.
        /// </summary>
        ObjectNotFound = 8,
        /// <summary>
        /// User account could be out of disk space.
        /// => Item
        /// Free space in the user's mailbox and retry the Sync command.
        /// </summary>
        UserMailboxOutOfDiskError = 9,
        /// <summary>
        /// Mailbox folders are not synchronized.
        /// => Global
        /// Perform a FolderSync command (section 2.2.2.4) and then retry the Sync command.
        /// </summary>
        FolderHierarchyHasChanged = 12,
        /// <summary>
        /// An empty or partial Sync command request is received and the cached set of notify-able collections is missing.
        /// => Item
        /// Resend a full Sync command request.
        /// </summary>
        NeedFullSyncError = 13,
        /// <summary>
        /// The Sync request was processed successfully but the wait interval (Wait element value (section 2.2.3.182)) or heartbeat interval (HeartbeatInterval element value (section 2.2.3.79.2)) that is specified by the client is outside the range set by the server administrator.
        /// If the HeartbeatInterval element value or Wait element value included in the Sync request is larger than the maximum allowable value, the response contains a Limit element that specifies the maximum allowed value.
        /// If the HeartbeatInterval element value or Wait value included in the Sync request is smaller than the minimum allowable value, the response contains a Limit element that specifies the minimum allowed value.
        /// => Item
        /// Update the Wait element value according to the Limit element and then resend the Sync command request.
        /// </summary>
        InvalidWaitOrHeartbeatInterval = 14,
        /// <summary>
        /// Too many collections are included in the Sync request.
        /// => Item
        /// Notify the user and synchronize fewer folders within one request.
        /// </summary>
        TooManyCollectionError = 15,
        /// <summary>
        /// Something on the server caused a retriable error.
        /// => Global
        /// Resend the request.
        /// </summary>
        Retry = 16

    }
}
