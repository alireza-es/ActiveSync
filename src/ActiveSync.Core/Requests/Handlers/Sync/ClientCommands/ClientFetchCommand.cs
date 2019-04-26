using ActiveSync.Core.ResponseObjects.Sync.ClientCommands;
using ActiveSync.Core.StateManagement;

namespace ActiveSync.Core.Requests.Handlers.Sync.ClientCommands
{
    public class ClientFetchCommand : ClientCommand
    {
        public ClientFetchCommand(StateManager stateManager) : base(stateManager)
        {
        }

        public string ServerId { get; set; }
        public override eClientCommandType CommandType
        {
            get { return eClientCommandType.Fetch; }
        }

        public override ClientCommandResponse Excecute()
        {
            throw new System.NotImplementedException();
        }



    }
}