using ActiveSync.Core.ApplicationData;
using ActiveSync.Core.Injection;
using ActiveSync.Core.ResponseObjects.Sync;
using ActiveSync.Core.ResponseObjects.Sync.ClientCommands;
using ActiveSync.Core.StateManagement;
using ActiveSync.Core.StateManagement.StateObjects;
using ActiveSync.SyncContract;
using ActiveSync.SyncContract.Service;

namespace ActiveSync.Core.Requests.Handlers.Sync.ClientCommands
{
    public class ClientAddCommand : ClientCommand
    {
        public ClientAddCommand(StateManager stateManager) : base(stateManager)
        {
            FolderService = ServiceResolver.GetService<IFolderService>();
            EmailService = ServiceResolver.GetService<IEmailService>();
            ContactService = ServiceResolver.GetService<IContactService>();
        }

        public IEmailService EmailService { get; set; }
        public IContactService ContactService { get; set; }
        public IFolderService FolderService { get; set; }
        public eFolderClass? FolderClass { get; set; }
        public string ClientId { get; set; }
        public AppData ApplicationData { get; set; }

        public override eClientCommandType CommandType
        {
            get
            {
                return eClientCommandType.Add;
            }
        }

        public override ClientCommandResponse Excecute()
        {
            if (!FolderClass.HasValue)
            {
                var folder = FolderService.GetFolder(StateManager.Credential, FolderId);
                FolderClass = folder.Type == eFolderType.DefaultContacts ? eFolderClass.Contacts : eFolderClass.Email;
            }
            var response = new ClientAddCommandResponse
            {
                FolderClass = FolderClass.Value,
                ClientId = ClientId
            };
            switch (FolderClass)
            {
                    case eFolderClass.Contacts:
                    string serverId;
                    var contact = ((ContactAppData) ApplicationData).Contact;

                    var serverAddResponse = ContactService.AddContact(StateManager.Credential, contact, out serverId);
                    response.Status = (eSyncStatus) serverAddResponse;
                    response.ServerId = serverId;

                    if (response.Status == eSyncStatus.Success)
                    {
                        #region Load & Save Collection State

                        var collectionState = StateManager.LoadCollectionState((SyncKey)SyncKey, FolderId) ?? new CollectionState();
                        collectionState.AddItem(new SyncItemState{ServerId = serverId, HashKey = ApplicationData.GenerateHash()});

                        StateManager.SaveCollectionState(SyncKey,collectionState);
                        response.ServerId = serverId;

                        #endregion

                    }
                    break;
                    case eFolderClass.Email:
                    break;
            }
            return response;
        }
    }
}