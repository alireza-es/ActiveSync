using ActiveSync.Core.Constants;
using ActiveSync.Core.Requests.Handlers.FolderSync;
using ActiveSync.Core.Requests.Handlers.Sync;

namespace ActiveSync.Core.Requests
{
    public class RequestHandlerFactory
    {
        //TODO: Reflection
        public static IRequestHandler GetHandler(eRequestCommand command)
        {
            switch (command)
            {
                case eRequestCommand.FolderSync:
                    return new FolderSyncRequest();
                case eRequestCommand.FolderCreate:
                    return new FolderCreateRequest();
                case eRequestCommand.FolderDelete:
                    return new FolderDeleteRequest();
                case eRequestCommand.FolderUpdate:
                    return new FolderUpdateRequest();
                case eRequestCommand.Sync:
                    return new SyncRequest();
                //case eRequestCommand.Ping:
                //    return new PingRequest();
                //case eRequestCommand.SendMail:
                //    return new SendMailRequest();
                //case eRequestCommand.SmartReplay:
                //    return new SmartReplayRequest();
                //case eRequestCommand.SmartForward:
                //    return new SmartForwardRequest();
                //case eRequestCommand.GetItemEstimate:
                //    return new GetItemEstimateRequest();
                //case eRequestCommand.ItemOperations:
                //    return new ItemOperationsRequest();
                //case eRequestCommand.Search:
                //    return new SearchRequest();
                default:
                    break;
            }

            return null;
        }

        public static string GetSupportingCommands()
        {
            var supporingCommands = new string[]
            {

                FolderHierarchyStrings.FolderSync,
                FolderHierarchyStrings.FolderCreate,
                FolderHierarchyStrings.FolderDelete,
                FolderHierarchyStrings.FolderUpdate,
                //PingStrings.Ping,
                AirSyncStrings.Sync,
                ComposeMailStrings.SendMail,
                ComposeMailStrings.SmartReply,
                ComposeMailStrings.SmartForward,
                ItemEstimateStrings.GetItemEstimate,
                ItemOperationsStrings.ItemOperations,
                SearchStrings.Search
            };

            return string.Join(",", supporingCommands);
        }
    }
}
