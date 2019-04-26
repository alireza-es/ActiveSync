namespace ActiveSync.Core.Requests.Handlers.ItemOperations
{
    public class EmptyFolderContent
    {
        public string CollectionId { get; set; }
        public EmptyFolderContentOption EmptyFolderContentOption { get; set; }
    }

    public class EmptyFolderContentOption
    {
        public bool DeleteSubFolders { get; set; }
    }
}
