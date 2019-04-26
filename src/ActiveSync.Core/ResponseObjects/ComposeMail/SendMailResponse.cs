using System.IO;
using System.Xml;
using ActiveSync.Core.Constants;
using ActiveSync.Core.Helper;

namespace ActiveSync.Core.ResponseObjects.ComposeMail
{
    public class SendMailResponse: ASResponse
    {
        public eMailStatus Status { get; set; }
        public override string GetAsXML()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.CreateDecleration();

            var rootNode = xmlDocument.AppendContainerNode(ComposeMailStrings.SendMail, Namespaces.ComposeMail);

            //Status
            rootNode.AppendValueNode(ComposeMailStrings.Status, this.Status.GetHashCode().ToString());

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
