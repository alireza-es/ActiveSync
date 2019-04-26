using ActiveSync.Core.Injection;
using ActiveSync.Core.ResponseObjects.Sync.ClientCommands;
using ActiveSync.Core.StateManagement;
using ActiveSync.SyncContract;
using ActiveSync.SyncContract.Service;

namespace ActiveSync.Core.Requests.Handlers.Sync.ClientCommands
{
    public class ClientDeleteCommand : ClientCommand
    {
        public ClientDeleteCommand(StateManager stateManager) : base(stateManager)
        {
            FolderService = ServiceResolver.GetService<IFolderService>();
            EmailService = ServiceResolver.GetService<IEmailService>();
            ContactService = ServiceResolver.GetService<IContactService>();
        }
        public IEmailService EmailService { get; set; }
        public IContactService ContactService { get; set; }
        public IFolderService FolderService { get; set; }
        public eFolderClass? FolderClass { get; set; }

        public string ServerId { get; set; }
        public override eClientCommandType CommandType
        {
            get { return eClientCommandType.Delete; }
        }

        public override ClientCommandResponse Excecute()
        {
            if (!FolderClass.HasValue)
            {
                var folder = FolderService.GetFolder(StateManager.Credential, FolderId);
                FolderClass = folder.Type == eFolderType.DefaultContacts ? eFolderClass.Contacts : eFolderClass.Email;
            }
            switch (FolderClass)
            {
                case eFolderClass.Contacts:

                    ContactService.DeleteContact(StateManager.Credential, ServerId);

                    break;
                case eFolderClass.Email:
                    break;
            }
            return null;
        }

    }
}