using System.Collections.Generic;
using ActiveSync.Core.Requests.Handlers.Sync.ClientCommands;

namespace ActiveSync.Core.Requests.Handlers.Sync
{
    public class RequestCollection
    {
        public RequestCollection()
        {
            this.ClientCommands = new List<ClientCommand>();
        }
        public string SyncKey { get; set; }
        public string CollectionId { get; set; }

        //TODO: Supported Emelemt

        //Set default to 1
        public bool DeletesAsMoves { get; set; }
        public bool GetChanges { get; set; }
        /// <summary>
        /// The maximum value for the WindowSize element is 512.
        /// </summary>
        public int WindowSize { get; set; }
        public bool ConversationMode { get; set; }
        public Options Options { get; set; }
        public List<ClientCommand> ClientCommands { get; set; }
    }
}