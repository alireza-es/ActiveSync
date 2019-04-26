using System.Collections.Generic;
using System.Xml.Linq;
using ActiveSync.Core.Constants;
using ActiveSync.Core.Exceptions;
using ActiveSync.Core.Helper;
using ActiveSync.Core.ResponseObjects.ItemOperations;

namespace ActiveSync.Core.Requests.Handlers.ItemOperations
{
    public class ItemOperationsRequest: ASRequest<ItemOperationsResponse>
    {
        public EmptyFolderContent EmptyFolderContents { get; set; }
        public List<FetchItemOperation> FetchItemOperations { get; set; }
        public MoveItemOperation MoveItemOperations { get; set; }

        public override eRequestCommand Command
        {
            get { return eRequestCommand.ItemOperations; }
        }
        protected override void Initialize(string xmlRequest)
        {
            if (string.IsNullOrWhiteSpace(xmlRequest))
                throw new InvalidRequestException("Ping ItemOperations Content is Empty.");

            var root = XDocument.Parse(xmlRequest).Root;
            var emptyFolderContentsEls = root.GetDescendants(ItemOperationsStrings.EmptyFolderContents);
            foreach (var emptyFolderContentsEl in emptyFolderContentsEls)
            {
                this.EmptyFolderContents = new EmptyFolderContent();
                
            }
            var fetchItemOperationsEls = root.GetDescendants(ItemOperationsStrings.Fetch);
            this.FetchItemOperations = new List<FetchItemOperation>(fetchItemOperationsEls.Count);
            foreach (var fetchItemOperationsEl in fetchItemOperationsEls)
            {
                var fetchItemOperation = new FetchItemOperation();
                fetchItemOperation.ServerId = fetchItemOperationsEl.GetElementValueAsString(AirSyncStrings.ServerId, Namespaces.AirSync);
                fetchItemOperation.CollectionId =
                    fetchItemOperationsEl.GetElementValueAsString(AirSyncStrings.CollectionId, Namespaces.AirSync);
                fetchItemOperation.Store = fetchItemOperationsEl.GetElementValueAsString(ItemEstimateStrings.Store);
            }
            var moveItemOperations = root.GetDescendants(ItemOperationsStrings.Move);
            foreach (var moveItemOperation in moveItemOperations)
            {
                this.MoveItemOperations = new MoveItemOperation();                
            }

        }
        protected override ItemOperationsResponse HandleRequest()
        {
            return new ItemOperationsResponse()
            {
            };
        }
    }
}
