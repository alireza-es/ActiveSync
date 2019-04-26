using System.Collections.Generic;
using ActiveSync.Core.ApplicationData;
using ActiveSync.SyncContract;

namespace ActiveSync.Core.ResponseObjects.ItemOperations
{
    public class Response
    {
        public List<MoveItemOperation> MoveItemOperations { get; set; }
        public List<FetchItemOperation> FetchItemOperations { get; set; }
        public List<EmptyFolderContent> EmptyFolderContents { get; set; }
    }

    public class MoveItemOperation
    {
        public string Status { get; set; }
        public string ConversationId { get; set; }
    }

    public class FetchItemOperation
    {
        public string Status { get; set; }
        public string CollectionId { get; set; }
        public string ServerId { get; set; }
        public string LongId { get; set; }
        public eFolderClass FolderClass { get; set; }
        public string LinkId { get; set; }
        public AppData Properties { get; set; }
    }

    public class EmptyFolderContent
    {
        public string Status { get; set; }
        public string CollectionId { get; set; }
        
    }
}
