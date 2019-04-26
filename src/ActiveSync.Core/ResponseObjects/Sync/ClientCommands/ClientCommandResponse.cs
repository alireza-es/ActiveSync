using ActiveSync.Core.ApplicationData;
using ActiveSync.Core.Constants;
using ActiveSync.SyncContract;

namespace ActiveSync.Core.ResponseObjects.Sync.ClientCommands
{
    public abstract class ClientCommandResponse
    {
        public abstract eClientCommandResponseType ClientCommandResponseType { get; }
        public abstract string ResponseName { get; }
    }
    public class ClientAddCommandResponse : ClientCommandResponse
    {
        public override eClientCommandResponseType ClientCommandResponseType
        {
            get { return eClientCommandResponseType.Add; }
        }

        public override string ResponseName
        {
            get { return AirSyncStrings.Add; }
        }

        public eFolderClass FolderClass { get; set; }
        public string ClientId { get; set; }
        public string ServerId { get; set; }
        public eSyncStatus Status { get; set; }
    }
    public class ClientChangeCommandResponse : ClientCommandResponse
    {
        public override eClientCommandResponseType ClientCommandResponseType
        {
            get { return eClientCommandResponseType.Change; }
        }

        public override string ResponseName
        {
            get { return AirSyncStrings.Change; }
        }
        public eFolderClass FolderClass { get; set; }
        public string ServerId { get; set; }
        public eSyncStatus Status { get; set; }
    }
    public class ClientFetchCommandResponse : ClientCommandResponse
    {
        public override eClientCommandResponseType ClientCommandResponseType
        {
            get { return eClientCommandResponseType.Fetch; }
        }

        public override string ResponseName
        {
            get { return AirSyncStrings.Fetch; }
        }
        public string ServerId { get; set; }
        public eSyncStatus Status { get; set; }
        public AppData AppData { get; set; }
    }
}
