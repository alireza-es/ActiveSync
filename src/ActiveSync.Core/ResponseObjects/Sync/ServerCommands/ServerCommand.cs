using ActiveSync.Core.ApplicationData;
using ActiveSync.Core.Constants;
using ActiveSync.SyncContract;

namespace ActiveSync.Core.ResponseObjects.Sync.ServerCommands
{
    public abstract class ServerCommand
    {
        public abstract eServerCommandType CommandType { get; }
        public abstract string CommandName { get; }
    }
    public class AddServerCommand : ServerCommand
    {
        public override eServerCommandType CommandType
        {
            get { return eServerCommandType.Add; }
        }

        public override string CommandName
        {
            get { return AirSyncStrings.Add; }
        }

        public string ServerId { get; set; }
        public AppData AppData { get; set; }
    }
    public class ChangeServerCommand : ServerCommand
    {
        public override eServerCommandType CommandType
        {
            get { return eServerCommandType.Change; }
        }

        public override string CommandName
        {
            get { return AirSyncStrings.Change; }
        }

        public string ServerId { get; set; }
        public eFolderClass FolderClass { get; set; }
        public AppData AppData { get; set; }
    }
    public class DeleteServerCommand : ServerCommand
    {
        public override eServerCommandType CommandType
        {
            get { return eServerCommandType.Delete; }
        }

        public override string CommandName
        {
            get { return AirSyncStrings.Delete; }
        }

        public string ServerId { get; set; }
        public eFolderClass FolderClass { get; set; }
    }
    public class SoftDeleteServerCommand : ServerCommand
    {
        public override eServerCommandType CommandType
        {
            get { return eServerCommandType.SoftDelete; }
        }

        public override string CommandName
        {
            get { return AirSyncStrings.SoftDelete; }
        }

        public string ServerId { get; set; }
    }
}
