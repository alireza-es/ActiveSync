namespace ActiveSync.SyncContract
{
    public enum eFolderType:byte
    {
        DefaultInbox = 2,
        DefaultDrafts = 3,
        DefaultDeletedItems = 4,
        DefaultSentItems = 5,
        DefaultOutbox = 6,
        DefaultContacts = 9,
        UserCreatedMailFolder = 12
    }
}
