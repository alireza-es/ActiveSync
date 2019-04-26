using System;
using System.Linq;
using System.Xml.Linq;
using ActiveSync.Core.Constants;
using ActiveSync.Core.Exceptions;
using ActiveSync.Core.Helper;
using ActiveSync.Core.Injection;
using ActiveSync.Core.ResponseObjects.FolderSync;
using ActiveSync.Core.StateManagement;
using ActiveSync.SyncContract.Service;

namespace ActiveSync.Core.Requests.Handlers.FolderSync
{
    public class FolderDeleteRequest : ASRequest<FolderDeleteResponse>
    {
        public string SyncKey { get; set; }
        public string ServerId { get; set; }
        public IFolderService FolderService;
        public override eRequestCommand Command
        {
            get { return eRequestCommand.FolderDelete; }
        }
        protected override void Initialize(string xmlRequest)
        {
            FolderService = ServiceResolver.GetService<IFolderService>();
            if (string.IsNullOrWhiteSpace(xmlRequest))
                throw new InvalidRequestException("FolderDelete request content is empty.");

            var root = XDocument.Parse(xmlRequest).Root;
            
            this.SyncKey = root.GetElementValueAsString(FolderHierarchyStrings.SyncKey);
            this.ServerId = root.GetElementValueAsString(FolderHierarchyStrings.ServerId);            
        }
        protected override FolderDeleteResponse HandleRequest()
        {
            #region Parse SyncKey

            SyncKey syncKeyObject;
            if (!StateManagement.SyncKey.TryParse(SyncKey, out syncKeyObject))
            {
                //TODO: Log Exception
                return ErrorResponse(eFolderDeleteStatus.SyncKeyError);
            }

            #endregion

            #region Load Folder State

            var folderState = StateManager.LoadFolderState(syncKeyObject);
            if (folderState == null)
                //TODO: Log Folder State Not Found
                return ErrorResponse(eFolderDeleteStatus.SyncKeyError);

            #endregion

            try
            {
                var result = FolderService.DeleteFolder(StateManager.Credential, ServerId);

                var response = new FolderDeleteResponse
                {
                    SyncKey = this.SyncKey,
                    Status = ParseResult(result)
                };
                if (response.Status == eFolderDeleteStatus.Success)
                {
                    var newSyncKey = StateManager.GetNewSyncKey(syncKeyObject);

                    #region Update FolderState
                    var folderToDelete = folderState.Folders.FirstOrDefault(x => x.ServerId == ServerId);
                    if (folderToDelete == null)
                    {
                        return ErrorResponse(eFolderDeleteStatus.FolderNotExist);
                    }
                    else
                    {
                        folderState.Folders.Remove(folderToDelete);
                        StateManager.SaveFolderState(newSyncKey, folderState);
                    }
                    #endregion

                    response.SyncKey = newSyncKey;
                }

                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(eFolderDeleteStatus.UnknownError);
            }

        }

        private static eFolderDeleteStatus ParseResult(DeleteFolderResult folderResult)
        {
            switch (folderResult)
            {
                case DeleteFolderResult.Success:
                    return eFolderDeleteStatus.Success;
                case DeleteFolderResult.SystemFolder:
                    return eFolderDeleteStatus.SystemFolder;
                case DeleteFolderResult.FolderNotExist:
                    return eFolderDeleteStatus.FolderNotExist;
                case DeleteFolderResult.ServerError:
                    return eFolderDeleteStatus.ServerError;
                default:
                    throw new StatusParseException("DeleteFolderResult: " + folderResult);
            }
        }


        private FolderDeleteResponse ErrorResponse(eFolderDeleteStatus errorStatus)
        {
            return new FolderDeleteResponse
            {
                Status = errorStatus,
                SyncKey = SyncKey
            };
        }

    }
}
