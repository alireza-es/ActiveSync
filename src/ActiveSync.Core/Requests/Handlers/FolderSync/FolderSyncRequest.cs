using System;
using System.Linq;
using System.Xml.Linq;
using ActiveSync.Core.Comparers;
using ActiveSync.Core.Constants;
using ActiveSync.Core.Exceptions;
using ActiveSync.Core.Helper;
using ActiveSync.Core.Injection;
using ActiveSync.Core.ResponseObjects.FolderSync;
using ActiveSync.Core.StateManagement;
using ActiveSync.Core.StateManagement.StateObjects;
using ActiveSync.SyncContract;
using ActiveSync.SyncContract.Service;
using ActiveSync.SyncContract.Syncables;

namespace ActiveSync.Core.Requests.Handlers.FolderSync
{
    /// <summary>
    /// The FolderSync command synchronizes the collection hierarchy but does not synchronize the items in the collections themselves.
    /// </summary>
    public class FolderSyncRequest : ASRequest<FolderSyncResponse>
    {
        private IFolderService FolderService;
        public string SyncKey { get; set; }
        public override eRequestCommand Command
        {
            get { return eRequestCommand.FolderSync; }
        }

        protected override FolderSyncResponse HandleRequest()
        {
            var newSyncKey = (SyncKey)StateManager.GetNewSyncKey(SyncKey);
            try
            {
                var allServerFolders = FolderService.GetAllFolders(StateManager.UserDevice.Credential);
                var newFolderState = new FolderHierarchyState
                {
                    Folders = allServerFolders.Select(syncFolder => new FolderState
                    {
                        ServerId = syncFolder.Id,
                        ParentId = syncFolder.ParentId,
                        FolderType = (int)syncFolder.Type,
                        Name = syncFolder.DisplayName
                    }).ToList()
                };
                //MS-ASCMD: 2.2.2.4 
                //All folders MUST be returned to the client when initial folder synchronization is done with a synchronization key of 0 (zero)
                if (SyncKey == "0")
                {
                    //May be Sync Key of Zero need to send headers(Supporting Versions and Supporting Commands) to Client

                    StateManager.SaveFolderState(newSyncKey, newFolderState);

                    var response = new FolderSyncResponse
                    {
                        SyncKey = newSyncKey,
                        AddedFolders = allServerFolders,
                        Status = eFolderSyncStatus.Success
                    };
                    return response;

                }
                else
                {
                    var folderState = StateManager.LoadFolderState(SyncKey);
                    if (folderState == null)
                        throw new StateNotFoundException("State Not Found. SyncKey:" + SyncKey);

                    var deviceFolders = folderState.Folders.Select(fs => new SyncableFolder
                    {
                        Id = fs.ServerId,
                        ParentId = fs.ParentId,
                        DisplayName = fs.Name,
                        Type = (eFolderType)fs.FolderType
                    }).ToList();

                    var newFolders = allServerFolders.Except(deviceFolders, new FolderComparerByServerId()).ToList();
                    var deletedFolders = deviceFolders.Except(allServerFolders, new FolderComparerByServerId()).ToList();

                    var intersectFolders = allServerFolders.Intersect(deviceFolders, new FolderComparerByServerId());
                    var folderValueComparer = new FolderComparerByValue();
                    var changedFolders = (from serverFolder in intersectFolders let deviceFolder = deviceFolders.FirstOrDefault(x => x.Id == serverFolder.Id) where !folderValueComparer.Equals(serverFolder, deviceFolder) select serverFolder).ToList();

                    StateManager.SaveFolderState(newSyncKey, newFolderState);
                    var response = new FolderSyncResponse
                    {
                        Status = eFolderSyncStatus.Success,
                        SyncKey = newSyncKey,
                        AddedFolders = newFolders,
                        UpdatedFolders = changedFolders,
                        DeletedFoldersServerIds = deletedFolders.Select(x => x.Id).ToList()
                    };

                    return response;
                }

            }
            catch (StateNotFoundException ex)
            {
                return ErrorResponse(eFolderSyncStatus.SyncKeyError);
            }
            catch (Exception ex)
            {
                return ErrorResponse(eFolderSyncStatus.ServerError);
            }
        }

        protected override void Initialize(string xmlRequest)
        {
            FolderService = ServiceResolver.GetService<IFolderService>();
            if (string.IsNullOrWhiteSpace(xmlRequest))
                throw new InvalidRequestException("FolderCreate Request Content is Empty.");

            var root = XDocument.Parse(xmlRequest).Root;
            this.SyncKey = root.GetElementValueAsString(FolderHierarchyStrings.SyncKey);

        }
        private FolderSyncResponse ErrorResponse(eFolderSyncStatus errorStatus)
        {
            return new FolderSyncResponse
            {
                Status = errorStatus,
                SyncKey = SyncKey
            };
        }
    }
}
