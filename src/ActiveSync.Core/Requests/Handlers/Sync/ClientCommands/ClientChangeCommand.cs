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
    public class ClientChangeCommand : ClientCommand
    {
        public ClientChangeCommand(StateManager stateManager)
            : base(stateManager)
        {
            FolderService = ServiceResolver.GetService<IFolderService>();
            EmailService = ServiceResolver.GetService<IEmailService>();
            ContactService = ServiceResolver.GetService<IContactService>();
        }

        public IEmailService EmailService { get; set; }
        public IContactService ContactService { get; set; }
        public IFolderService FolderService { get; set; }
        public eFolderClass? FolderClass { get; set; }
        public AppData ApplicationData { get; set; }
        public string ServerId { get; set; }

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
            var response = new ClientChangeCommandResponse
            {
                FolderClass = FolderClass.Value,
                ServerId = ApplicationData.ServerId
            };
            switch (FolderClass)
            {
                case eFolderClass.Contacts:
                    var contact = ((ContactAppData)ApplicationData).Contact;
                    contact.Id = ServerId;

                    var serverChangeResponse = ContactService.UpdateContact(StateManager.Credential, contact);
                    response.Status = (eSyncStatus)serverChangeResponse;

                    if (response.Status == eSyncStatus.Success)
                    {
                        #region Load & Save Collection State

                        var collectionState = StateManager.LoadCollectionState((SyncKey)SyncKey, FolderId) ?? new CollectionState();
                        collectionState.UpdateItem(ServerId, ApplicationData.GenerateHash());

                        StateManager.SaveCollectionState(SyncKey, collectionState);

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