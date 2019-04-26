namespace ActiveSync.SyncContract.Syncables
{
    public class SyncableFolder
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string DisplayName { get; set; }
        public eFolderType Type { get; set; }
    }
}
