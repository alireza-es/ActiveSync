namespace ActiveSync.Core.Requests
{
    public enum eRequestCommand
    {
        FolderSync = 1,
        FolderCreate,
        FolderDelete,
        FolderUpdate,
        Sync,
        Ping,
        Autodiscover,
        GetItemEstimate,
        GetAttachment,
        SendMail,
        SmartReplay,
        SmartForward,
        ItemOperations,
        Search
    }
}
