using System.IO;
using System.Xml;
using ActiveSync.Core.Constants;
using ActiveSync.Core.Helper;

namespace ActiveSync.Core.ResponseObjects.FolderSync
{
    public class FolderUpdateResponse : ASResponse
    {
        public string SyncKey { get; set; }
        public eFolderUpdateStatus Status { get; set; }

        public override string GetAsXML()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.CreateDecleration();

            var rootNode = xmlDocument.AppendContainerNode(FolderHierarchyStrings.FolderUpdate, Namespaces.FolderHierarchy);

            //Status
            rootNode.AppendValueNode(FolderHierarchyStrings.Status, this.Status.GetHashCode().ToString());

            //SyncKey
            rootNode.AppendValueNode(FolderHierarchyStrings.SyncKey, this.SyncKey);

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
