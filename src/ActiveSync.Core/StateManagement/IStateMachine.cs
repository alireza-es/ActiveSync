using ActiveSync.Core.StateManagement.StateObjects;

namespace ActiveSync.Core.StateManagement
{
    public interface IStateMachine
    {
        FolderHierarchyState LoadFolderState(string deviceId, SyncKey syncKey);
        void SaveFolderState(string deviceId, SyncKey syncKey, FolderHierarchyState folderState);
        CollectionState LoadCollectionState(string deviceId, SyncKey syncKey, string folderId);
        void SaveCollectionState(string deviceId, SyncKey syncKey, CollectionState collectionState);
    }
}
