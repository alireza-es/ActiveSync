using ActiveSync.Core.ResponseObjects.Sync.ClientCommands;
using ActiveSync.Core.StateManagement;

namespace ActiveSync.Core.Requests.Handlers.Sync.ClientCommands
{
    public abstract class ClientCommand
    {
        public string FolderId { get; set; }
        public string SyncKey { get; set; }
        public abstract eClientCommandType CommandType { get; }
        public abstract ClientCommandResponse Excecute();
        protected StateManager StateManager { get; set; }

        protected ClientCommand(StateManager stateManager)
        {
            this.StateManager = stateManager;
        }
    }
}
