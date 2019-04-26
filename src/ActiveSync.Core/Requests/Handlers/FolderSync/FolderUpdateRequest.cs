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
using ActiveSync.SyncContract.Syncables;

namespace ActiveSync.Core.Requests.Handlers.FolderSync
{
    public class FolderUpdateRequest : ASRequest<FolderUpdateResponse>
    {
        public string ServerId { get; set; }
        public string ParentId { get; set; }
        public string DisplayName { get; set; }
        public string SyncKey { get; set; }

        public IFolderService FolderService;
        public override eRequestCommand Command
        {
            get { return eRequestCommand.FolderUpdate; }
        }
        protected override void Initialize(string xmlRequest)
        {
            FolderService = ServiceResolver.GetService<IFolderService>();
            if (string.IsNullOrWhiteSpace(xmlRequest))
                throw new InvalidRequestException("FolderUpdate Request Content is Empty.");

            var root = XDocument.Parse(xmlRequest).Root;

            this.SyncKey = root.GetElementValueAsString(FolderHierarchyStrings.SyncKey);
            this.ServerId = root.GetElementValueAsString(FolderHierarchyStrings.ServerId);
            this.ParentId = root.GetElementValueAsString(FolderHierarchyStrings.ParentId);
            this.DisplayName = root.GetElementValueAsString(FolderHierarchyStrings.DisplayName);
            
        }
        protected override FolderUpdateResponse HandleRequest()
        {
            #region Parse SyncKey

            SyncKey syncKeyObject;
            if (!StateManagement.SyncKey.TryParse(SyncKey, out syncKeyObject))
            {
                //TODO: Log Exception
                return ErrorResponse(eFolderUpdateStatus.SyncKeyError);
            }

            #endregion

            #region Load Folder State

            var folderState = StateManager.LoadFolderState(syncKeyObject);
            if (folderState == null)
                //TODO: Log Folder State Not Found
                return ErrorResponse(eFolderUpdateStatus.SyncKeyError);

            #endregion

            var folderInfo = new SyncableFolder
            {
                DisplayName = this.DisplayName,
                ParentId = this.ParentId,
                Id = this.ServerId
            };
            try
            {
                var result = FolderService.UpdateFolder(StateManager.Credential, folderInfo);

                var response = new FolderUpdateResponse
                {
                    SyncKey = this.SyncKey,
                    Status = ParseResult(result)
                };
                if (response.Status == eFolderUpdateStatus.Success)
                {
                    var newSyncKey = StateManager.GetNewSyncKey(syncKeyObject);

                    #region Update FolderState
                    var folderToUpdate = folderState.Folders.FirstOrDefault(x => x.ServerId == ServerId);
                    if (folderToUpdate == null)
                    {
                        return ErrorResponse(eFolderUpdateStatus.FolderNotExist);
                    }
                    else
                    {
                        folderToUpdate.Name = this.DisplayName;
                        folderToUpdate.ParentId = this.ParentId;

                        StateManager.SaveFolderState(newSyncKey, folderState);
                    }
                    #endregion

                    response.SyncKey = newSyncKey;
                }

                return response;
            }
            catch (Exception)
            {
                return ErrorResponse(eFolderUpdateStatus.UnknownError);
            }

        }

        private static eFolderUpdateStatus ParseResult(UpdateFolderResult folderResult)
        {
            switch (folderResult)
            {
                case UpdateFolderResult.Success:
                    return eFolderUpdateStatus.Success;
                case UpdateFolderResult.FolderAlreadyExist:
                    return eFolderUpdateStatus.FolderAlreadyExist;
                case UpdateFolderResult.SystemFolder:
                    return eFolderUpdateStatus.SystemFolder;
                case UpdateFolderResult.FolderNotExist:
                    return eFolderUpdateStatus.FolderNotExist;
                case UpdateFolderResult.ParentNotFound:
                    return eFolderUpdateStatus.ParentNotFound;
                case UpdateFolderResult.ServerError:
                    return eFolderUpdateStatus.ServerError;
                default:
                    throw new StatusParseException("UpdateFolderResult: " + folderResult);
            }
        }


        private FolderUpdateResponse ErrorResponse(eFolderUpdateStatus errorStatus)
        {
            return new FolderUpdateResponse
            {
                Status = errorStatus,
                SyncKey = SyncKey
            };
        }
    }
}
