using System;
using System.Linq;
using System.Xml.Linq;
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
    public class FolderCreateRequest : ASRequest<FolderCreateResponse>
    {
        public IFolderService FolderService;
        public string SyncKey { get; set; }
        public string ParentId { get; set; }
        public string DisplayName { get; set; }
        public eFolderType Type { get; set; }
        public override eRequestCommand Command
        {
            get { return eRequestCommand.FolderCreate; }
        }
        protected override void Initialize(string xmlRequest)
        {
            FolderService = ServiceResolver.GetService<IFolderService>();

            if (string.IsNullOrWhiteSpace(xmlRequest))
                throw new InvalidRequestException("FolderCreate Request Content is Empty.");

            var root = XDocument.Parse(xmlRequest).Root;
            this.SyncKey = root.GetElementValueAsString(FolderHierarchyStrings.SyncKey);
            this.ParentId = root.GetElementValueAsString(FolderHierarchyStrings.ParentId);
            this.DisplayName = root.GetElementValueAsString(FolderHierarchyStrings.DisplayName);
            this.Type = (eFolderType)root.GetElementValueAsInt(FolderHierarchyStrings.Type);

        }
        protected override FolderCreateResponse HandleRequest()
        {
            #region Parse SyncKey

            SyncKey syncKeyObject;
            if(!StateManagement.SyncKey.TryParse(SyncKey, out syncKeyObject))
            {
                //TODO: Log Exception
                return ErrorResponse(eFolderCreateStatus.SyncKeyError);
            }

            #endregion

            #region Load Folder State

            var folderState = StateManager.LoadFolderState(syncKeyObject);
            if (folderState == null)
                //TODO: Log Folder State Not Found
                return ErrorResponse(eFolderCreateStatus.SyncKeyError);

            #endregion

            #region Export Changes To Server

            try
            {
                var folderInfo = new SyncableFolder
                {
                    DisplayName = this.DisplayName,
                    ParentId = this.ParentId,
                    Type = this.Type
                };

                string createdFolderId;

                var result = FolderService.CreateFolder(StateManager.Credential, folderInfo, out createdFolderId);

                var response = new FolderCreateResponse
                {
                    SyncKey = this.SyncKey,
                    Status = ParseResult(result)
                };
                if (response.Status == eFolderCreateStatus.Success)
                {
                    var newSyncKey = StateManager.GetNewSyncKey(syncKeyObject);

                    #region Update FolderState
                    var parentFolderState = folderState.Folders.FirstOrDefault(x => x.ServerId == ParentId);
                    if (parentFolderState == null)
                    {
                        return ErrorResponse(eFolderCreateStatus.ParentNotFound);
                    }
                    else
                    {
                        folderState.Folders.Add(new FolderState
                        {
                            ParentId = ParentId,
                            ServerId = createdFolderId,
                            FolderType = (int)Type,
                            Name = DisplayName
                        });
                        StateManager.SaveFolderState(newSyncKey, folderState);
                    }
                    #endregion

                    response.SyncKey = newSyncKey;
                    response.ServerId = createdFolderId;
                }

                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(eFolderCreateStatus.UnknownError);
            }

            #endregion

        }

        private static eFolderCreateStatus ParseResult(CreateFolderResult folderResult)
        {
            switch (folderResult)
            {
                case CreateFolderResult.Success:
                    return eFolderCreateStatus.Success;
                case CreateFolderResult.FolderAlreadyExist:
                    return eFolderCreateStatus.FolderAlreadyExist;
                case CreateFolderResult.SystemFolder:
                    return eFolderCreateStatus.SystemFolder;
                case CreateFolderResult.ParentNotFound:
                    return eFolderCreateStatus.ParentNotFound;
                case CreateFolderResult.ServerError:
                    return eFolderCreateStatus.ServerError;
                default:
                    throw new StatusParseException("CreateFolderResult: " + folderResult);
            }

        }
        private FolderCreateResponse ErrorResponse(eFolderCreateStatus errorStatus)
        {
            return new FolderCreateResponse
            {
                Status = errorStatus,
                SyncKey = SyncKey
            };
        }
    }
}
