using System.Collections.Generic;
using ActiveSync.Core.ResponseObjects.Sync.ClientCommands;
using ActiveSync.Core.ResponseObjects.Sync.ServerCommands;

namespace ActiveSync.Core.ResponseObjects.Sync
{
    public class ResponseCollection
    {
        public ResponseCollection()
        {
            Commands = new List<ServerCommand>();
            Responses = new List<ClientCommandResponse>();
        }
        public string SyncKey { get; set; }
        public string CollectionId { get; set; }
        public eSyncStatus Status { get; set; }
        public bool ConversationMode { get; set; }
        public List<ServerCommand> Commands { get; set; }
        public List<ClientCommandResponse> Responses { get; set; }
        public bool MoreAvailable { get; set; }
    }
}
