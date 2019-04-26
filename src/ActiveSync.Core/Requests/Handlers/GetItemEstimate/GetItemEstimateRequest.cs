using System;
using System.Collections.Generic;
using System.Xml.Linq;
using ActiveSync.Core.Constants;
using ActiveSync.Core.Exceptions;
using ActiveSync.Core.Helper;
using ActiveSync.Core.ResponseObjects.GetItemEstimate;
using ActiveSync.SyncContract;

namespace ActiveSync.Core.Requests.Handlers.GetItemEstimate
{
    public class GetItemEstimateRequest : ASRequest<GetItemEstimateResponse>
    {
        public List<ItemEstimateCollection> ItemEstimateCollections { get; set; }

        public override eRequestCommand Command
        {
            get { return eRequestCommand.GetItemEstimate; }
        }
        protected override void Initialize(string xmlRequest)
        {
            if (string.IsNullOrWhiteSpace(xmlRequest))
                throw new InvalidRequestException("ItemOperations Request Content is Empty.");

            var root = XDocument.Parse(xmlRequest).Root;
            var itemEstimateCollectionEls = root.GetDescendants(ItemEstimateStrings.Collection);
            
            this.ItemEstimateCollections = new List<ItemEstimateCollection>(itemEstimateCollectionEls.Count);
            foreach (var collectionEl in itemEstimateCollectionEls)
            {
                var newCollection = new ItemEstimateCollection();
                newCollection.SyncKey = collectionEl.GetElementValueAsString(ItemEstimateStrings.SyncKey, Namespaces.AirSync);
                newCollection.CollectionId =
                    collectionEl.GetElementValueAsString(ItemEstimateStrings.CollectionId, Namespaces.GetItemEstimate);

                newCollection.ConversationMode =
                    collectionEl.GetElementValueAsBoolean(AirSyncStrings.ConversationMode, Namespaces.AirSync);
                newCollection.FilterType = collectionEl.GetElementValueAsInt(AirSyncStrings.FilterType,
                    Namespaces.AirSync);
                var optionsEl = collectionEl.GetElement(AirSyncStrings.Options, Namespaces.AirSync);
                if (optionsEl != null)
                {
                    string folderClass = optionsEl.GetElementValueAsString(AirSyncStrings.Class);
                    newCollection.Options = new ItemEstimateOption()
                    {
                        FilterType = optionsEl.GetElementValueAsInt(AirSyncStrings.FilterType, Namespaces.AirSync),
                        FolderClass = (eFolderClass)Enum.Parse(typeof(eFolderClass), folderClass)
                    };
                }
            }
        }
        protected override GetItemEstimateResponse HandleRequest()
        {
            return new GetItemEstimateResponse()
            {
                Status = eGetItemEstimateStatus.Success,
                Responses = new List<Response>()
                {
                    new Response()
                    {
                        Status = 1,
                        Collection = new ResponseObjects.GetItemEstimate.ItemEstimateCollection()
                        {
                            CollectionId =  "coll",
                            Estimate = 1
                        }
                    }
                }
            };
        }
    }

    public class ItemEstimateCollection
    {
        public string CollectionId { get; set; }
        public string SyncKey { get; set; }
        public int FilterType { get; set; }
        public ItemEstimateOption Options { get; set; }
        public Boolean ConversationMode { get; set; }
    }

    public class ItemEstimateOption
    {
        public int FilterType { get; set; }
        public eFolderClass FolderClass { get; set; }
    }

}
