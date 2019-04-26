namespace ActiveSync.Core.Requests.Handlers.ItemOperations
{
    public enum eMIMESendSupport:byte
    {
        /// <summary>
        /// Never send MIME data.
        /// </summary>
        Never = 0,
        /// <summary>
        /// Send MIME data for S/MIME messages only. Send regular body for all other messages.
        /// </summary>
        SMIMEMessageOnly = 1,
        /// <summary>
        /// Send MIME data for all messages. This flag could be used by clients to build a more rich and complete Inbox solution.
        /// </summary>
        Allways = 2
    }
}