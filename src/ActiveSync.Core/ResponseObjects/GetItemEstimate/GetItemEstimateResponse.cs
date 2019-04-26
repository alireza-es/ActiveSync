using System.Collections.Generic;
using System.IO;
using System.Xml;
using ActiveSync.Core.Constants;
using ActiveSync.Core.Helper;

namespace ActiveSync.Core.ResponseObjects.GetItemEstimate
{
    public class GetItemEstimateResponse : ASResponse
    {
        public eGetItemEstimateStatus Status { get; set; }
        public List<Response> Responses { get; set; }

        public override string GetAsXML()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.CreateDecleration();

            var rootNode = xmlDocument.AppendContainerNode(ItemEstimateStrings.GetItemEstimate, Namespaces.GetItemEstimate);

            //Status
            rootNode.AppendValueNode(ItemEstimateStrings.Status, this.Status.GetHashCode().ToString());

            if (this.Responses != null && this.Responses.Count > 0)
            {
                foreach (var response in this.Responses)
                {
                    var responseNode = rootNode.AppendContainerNode(ItemEstimateStrings.Response);
                   
                    responseNode.AppendValueNode(ItemEstimateStrings.Status,
                        response.Status.GetHashCode().ToString());
                    var collectionNode = responseNode.AppendContainerNode(ItemEstimateStrings.Collection);
                    collectionNode.AppendValueNode(ItemEstimateStrings.CollectionId, response.Collection.CollectionId);
                    collectionNode.AppendValueNode(ItemEstimateStrings.Estimate, response.Collection.Estimate.ToString());

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
}
