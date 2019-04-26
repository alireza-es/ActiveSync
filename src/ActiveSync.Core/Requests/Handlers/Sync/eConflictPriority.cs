namespace ActiveSync.Core.Requests.Handlers.Sync
{
    public enum eConflictPriority : byte
    {
        /// <summary>
        /// Client object replaces server object
        /// </summary>
        ClientPriority = 0,
        /// <summary>
        /// Server object replaces client object.
        /// </summary>
        ServerPriority = 1
    }
}