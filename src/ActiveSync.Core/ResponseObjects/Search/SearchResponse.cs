using System.Collections.Generic;
using System.IO;
using System.Xml;
using ActiveSync.Core.Constants;
using ActiveSync.Core.Helper;
using ActiveSync.SyncContract;

namespace ActiveSync.Core.ResponseObjects.Search
{
    public class SearchResponse:ASResponse
    {
        public eSearchStatus Status { get; set; }
        public Response Response { get; set; }
        public override string GetAsXML()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.CreateDecleration();

            var rootNode = xmlDocument.AppendContainerNode(SearchStrings.Search, Namespaces.Search);
           
            //Status
            rootNode.AppendValueNode(SearchStrings.Status, this.Status.GetHashCode().ToString());
            
            //Response
            if(this.Response != null)
            { 
                var responseNode = rootNode.AppendContainerNode(SearchStrings.Response);
                if (this.Response.Store != null)
                {
                    var storeNode = responseNode.AppendContainerNode(SearchStrings.Store);
                    if (this.Response.Store.Results != null && this.Response.Store.Results.Count > 0)
                    {
                        foreach (var result in this.Response.Store.Results)
                        {
                            var resultNode = storeNode.AppendContainerNode(SearchStrings.Result);
                            resultNode.AppendValueNode(AirSyncStrings.Class, result.FolderClass.GetHashCode().ToString());
                            resultNode.AppendValueNode(SearchStrings.LongId, result.LongId);
                            resultNode.AppendValueNode(AirSyncStrings.CollectionId, result.CollectionId);
                            if (result.Properties != null)
                            {
                                var propertiesNode = resultNode.AppendContainerNode(SearchStrings.Properties);
                                if (result.Properties.EmailFrom != null && result.Properties.EmailFrom.Count > 0)
                                {
                                    foreach (var propertyEmailFrom in result.Properties.EmailFrom)
                                    {
                                        propertiesNode.AppendValueNode(EmailStrings.From, propertyEmailFrom);
                                    }
                                }
                            }
                        }
                       
                    }
                    storeNode.AppendValueNode(SearchStrings.Status, this.Response.Store.Status.GetHashCode().ToString());
                    storeNode.AppendValueNode(SearchStrings.Range, this.Response.Store.Rage);
                    storeNode.AppendValueNode(SearchStrings.Total, this.Response.Store.Total);
                }
            }

            return GetXmlAsString(xmlDocument);
        }

        private string GetXmlAsString(XmlDocument xDoc)
        {
            var sw = new StringWriter();
            var xmlw = new XmlTextWriter(sw);
            xmlw.Formatting = Formatting.Indented;
            xDoc.WriteTo(xmlw);
            xmlw.Flush();

            return sw.ToString();
        }
        
    }
    public class Response
    {
        public class SearchResponseStore
        {
            public class Result
            {
                public class SearchProperties
                {
                    public string ContactFirtName { get; set; }
                    public string ContactLastName { get; set; }
                    public List<string> EmailFrom { get; set; }
                    public List<string> EmailTo { get; set; }
                    public string EmailCc { get; set; }
                    public string EmailSubject { get; set; }

                }

                public eFolderClass FolderClass { get; set; }
                public string LongId { get; set; }
                public string CollectionId { get; set; }
                public SearchProperties Properties { get; set; }
            }

            public eSearchStatus Status { get; set; }
            public List<Result> Results { get; set; }
            public string Rage { get; set; }
            public string Total { get; set; }
        }
        public SearchResponseStore Store { get; set; }
    }
}
