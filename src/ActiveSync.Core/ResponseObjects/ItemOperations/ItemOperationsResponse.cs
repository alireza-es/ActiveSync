using System.IO;
using System.Xml;
using ActiveSync.Core.Constants;
using ActiveSync.Core.Helper;

namespace ActiveSync.Core.ResponseObjects.ItemOperations
{
    public class ItemOperationsResponse: ASResponse
    {
        public string Status { get; set; }
        public Response Response { get; set; }

        public override string GetAsXML()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.CreateDecleration();

            var rootNode = xmlDocument.AppendContainerNode(ItemOperationsStrings.ItemOperations, Namespaces.ItemOperations);

            //Status
            rootNode.AppendValueNode(ItemOperationsStrings.Status, this.Status.GetHashCode().ToString());

            //  Response
            rootNode.AppendContainerNode(ItemOperationsStrings.Response);

            if (this.Response.MoveItemOperations != null && this.Response.MoveItemOperations.Count > 0)
            {
                foreach (var moveItemOperation in this.Response.MoveItemOperations)
                {
                    var moveNode = rootNode.AppendContainerNode(ItemOperationsStrings.Move);
                    moveNode.AppendValueNode(ItemOperationsStrings.Status, moveItemOperation.Status);
                    moveNode.AppendValueNode(ItemOperationsStrings.ConversationId, moveItemOperation.ConversationId);

                }
            }
            if (this.Response.FetchItemOperations != null && this.Response.FetchItemOperations.Count > 0)
            {
                foreach (var fetchItemOperation in this.Response.FetchItemOperations)
                {
                    var fetchNode = rootNode.AppendContainerNode(ItemOperationsStrings.Fetch);
                    fetchNode.AppendValueNode(ItemOperationsStrings.Status, fetchItemOperation.Status);
                    fetchNode.AppendValueNode(AirSyncStrings.CollectionId, fetchItemOperation.CollectionId);
                    fetchNode.AppendValueNode(AirSyncStrings.ServerId, fetchItemOperation.ServerId);
                    fetchNode.AppendValueNode(SearchStrings.LongId, fetchItemOperation.LongId);
                    fetchNode.AppendValueNode(DocumentLibraryStrings.LinkId, fetchItemOperation.LinkId);
                    //fetchNode.AppendValueNode(ItemOperationsStrings.Properties, fetchItemOperation.Properties);
                }
            }
            if (this.Response.EmptyFolderContents != null && this.Response.EmptyFolderContents.Count > 0)
            {
                foreach (var emptyFolderContent in this.Response.EmptyFolderContents)
                {
                    var emptyFolderContentNode = rootNode.AppendContainerNode(ItemOperationsStrings.EmptyFolderContents);
                    emptyFolderContentNode.AppendValueNode(ItemOperationsStrings.Status, emptyFolderContent.Status);
                    emptyFolderContentNode.AppendValueNode(AirSyncStrings.CollectionId, emptyFolderContent.CollectionId);
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
