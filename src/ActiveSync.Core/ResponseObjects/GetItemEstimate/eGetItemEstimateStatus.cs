namespace ActiveSync.Core.ResponseObjects.GetItemEstimate
{
    public enum eGetItemEstimateStatus
    {
        /// <summary>
        /// Server successfully completed command.
        /// => Global
        /// </summary>
        Success = 1,
        /// <summary>
        /// One or more of the specified folders does not exist or an incorrect folder was requested.
        /// => Global
        /// </summary>
        InvalidCollection = 2,
        SyncStateNotPrimed = 3,
        /// <summary>
        /// Malformed or mismatched synchronization key. or The synchronization state is corrupted on the server.
        /// => Global
        /// </summary>
        InvalidSyncKey = 4
    }
}
