using System.Collections.Generic;
using System.Xml.Linq;
using ActiveSync.Core.Constants;
using ActiveSync.Core.Exceptions;
using ActiveSync.Core.Helper;
using ActiveSync.Core.Requests.Handlers.Sync;
using ActiveSync.Core.ResponseObjects.Search;

namespace ActiveSync.Core.Requests.Handlers.Search
{
    public class SearchRequest:ASRequest<SearchResponse>
    {
        public List<Store> Stores { get; set; }

        public override eRequestCommand Command
        {
            get { return eRequestCommand.Search; }
        }

        protected override void Initialize(string xmlRequest)
        {
            if(string.IsNullOrWhiteSpace(xmlRequest))
                throw new InvalidRequestException("Search request content is empty.");

            var root = XDocument.Parse(xmlRequest).Root;
            var storeEls = root.GetDescendants(SearchStrings.Store);
            this.Stores = new List<Store>(storeEls.Count);
            foreach (var storeEl in storeEls)
            {
                var newStore = new Store()
                {
                    Name = storeEl.GetElementValueAsString(SearchStrings.Name),
                    Query = storeEl.GetElementValueAsString(SearchStrings.Query)
                };
                var optionEl = storeEl.GetElement(SearchStrings.Options);
                if (optionEl != null)
                {
                    newStore.Options = new Options()
                    {
                        MIMESupport = (eMIMESendSupport)optionEl.GetElementValueAsInt(AirSyncStrings.MIMESupport),
                        Range = optionEl.GetElementValueAsString(SearchStrings.Range),
                        UserName = optionEl.GetElementValueAsString(SearchStrings.UserName),
                        Password = optionEl.GetElementValueAsString(SearchStrings.Password),
                        DeepTraversal = optionEl.HasChildElement(SearchStrings.DeepTraversal),
                        RebuildResults = optionEl.HasChildElement(SearchStrings.RebuildResults)
                    };
                    var bodyPreferenceEl = optionEl.GetElement(AirSyncBaseStrings.BodyPreference, Namespaces.AirSyncBase);
                    if (bodyPreferenceEl != null)
                    {
                        newStore.Options.BodyPreference = new BodyPreference
                        {
                            Type = (eBodyContentType)bodyPreferenceEl.GetElementValueAsInt(AirSyncBaseStrings.Type)
                        };
                        if (bodyPreferenceEl.HasChildElement(AirSyncBaseStrings.TruncationSize))
                        {
                            newStore.Options.BodyPreference.TruncationSize =
                                bodyPreferenceEl.GetElementValueAsInt(AirSyncBaseStrings.TruncationSize);
                        }
                        if (bodyPreferenceEl.HasChildElement(AirSyncBaseStrings.AllOrNone))
                        {
                            newStore.Options.BodyPreference.AllOrNone =
                                bodyPreferenceEl.GetElementValueAsBoolean(AirSyncBaseStrings.AllOrNone);
                        }
                        if (bodyPreferenceEl.HasChildElement(AirSyncBaseStrings.Preview))
                        {
                            newStore.Options.BodyPreference.Preview =
                                bodyPreferenceEl.GetElementValueAsInt(AirSyncBaseStrings.Preview);
                        }
                    }
                    var bodyPartPreferenceEl = optionEl.GetElement(AirSyncBaseStrings.BodyPartPreference, Namespaces.AirSyncBase);
                    if (bodyPartPreferenceEl != null)
                    {
                        newStore.Options.BodyPartPreference = new BodyPartPreference()
                        {
                            Type = (eBodyContentType)bodyPartPreferenceEl.GetElementValueAsInt(AirSyncBaseStrings.Type)
                        };
                        if (bodyPartPreferenceEl.HasChildElement(AirSyncBaseStrings.TruncationSize))
                        {
                            newStore.Options.BodyPartPreference.TruncationSize =
                                bodyPartPreferenceEl.GetElementValueAsInt(AirSyncBaseStrings.TruncationSize);
                        }
                        if (bodyPartPreferenceEl.HasChildElement(AirSyncBaseStrings.AllOrNone))
                        {
                            newStore.Options.BodyPartPreference.AllOrNone =
                                bodyPartPreferenceEl.GetElementValueAsBoolean(AirSyncBaseStrings.AllOrNone);
                        }
                        if (bodyPartPreferenceEl.HasChildElement(AirSyncBaseStrings.Preview))
                        {
                            newStore.Options.BodyPartPreference.Preview =
                                bodyPartPreferenceEl.GetElementValueAsInt(AirSyncBaseStrings.Preview);
                        }
                    }
                    //Picture
                    var pictureEl = optionEl.GetElement(SearchStrings.Picture);
                    if (pictureEl != null)
                    {
                        newStore.Options.Picture = new Picture()
                        {
                            MaxSize = pictureEl.GetElementValueAsInt(SearchStrings.MaxSize),
                            MaxPictures = pictureEl.GetElementValueAsInt(SearchStrings.MaxPictures)
                        };
                    }
                }
            }
        }

        protected override SearchResponse HandleRequest()
        {
            return new SearchResponse()
            {

            };
        }
    }
}
