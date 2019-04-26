using System.Collections.Generic;
using System.IO;
using System.Xml;
using ActiveSync.Core.Constants;
using ActiveSync.Core.Helper;

namespace ActiveSync.Core.ResponseObjects.Autodiscover
{
    public class AutodiscoverResponse : ASResponse
    {
        public Response Response { get; set; }

        public decimal Status { get; set; }
       

        public override string GetAsXML()
        {

            var xmlDocument = new XmlDocument();
            xmlDocument.CreateDecleration();

            var rootNode = xmlDocument.AppendContainerNode(AutodiscoverStrings.Autodiscover, Namespaces.Autodiscover);
            var responseNode = rootNode.AppendContainerNode(AutodiscoverStrings.Response);

            //Culture
            responseNode.AppendValueNode(AutodiscoverStrings.Culture, this.Response.Culture);

            //User
            if (this.Response.User != null)
            {
                var userNode = responseNode.AppendContainerNode(AutodiscoverStrings.EMailAddress);
                userNode.AppendValueNode(AutodiscoverStrings.DisplayName, this.Response.User.DisplayName);
                userNode.AppendValueNode(AutodiscoverStrings.EMailAddress, this.Response.User.EMailAddress);
            }
            if (this.Response.Action != null)
            {
                var actionNode = responseNode.AppendContainerNode(AutodiscoverStrings.Action);
                actionNode.AppendValueNode(AutodiscoverStrings.Redirect, this.Response.Action.Redirect);
                if (this.Response.Action.Settings != null && this.Response.Action.Settings.Servers != null && this.Response.Action.Settings.Servers.Count > 0)
                {
                    var settingsNode = actionNode.AppendContainerNode(AutodiscoverStrings.Settings);
                    foreach (var server in this.Response.Action.Settings.Servers)
                    {
                        var serverNode = settingsNode.AppendContainerNode(AutodiscoverStrings.Server);
                        serverNode.AppendValueNode(AutodiscoverStrings.Type, server.Type);
                        serverNode.AppendValueNode(AutodiscoverStrings.Url, server.Url);
                        serverNode.AppendValueNode(AutodiscoverStrings.Name, server.Name);
                        serverNode.AppendValueNode(AutodiscoverStrings.ServerData, server.ServerData);
                    }
                }
            }
            if (this.Response.Error != null)
            {
                var errorNode = responseNode.AppendContainerNode(AutodiscoverStrings.Error);
                errorNode.AppendValueNode(AutodiscoverStrings.ErrorCode, this.Response.Error.ErrorCode.GetHashCode().ToString());
                errorNode.AppendValueNode(AutodiscoverStrings.Message, this.Response.Error.Message);
                errorNode.AppendValueNode(AutodiscoverStrings.DebugData, this.Response.Error.DebugData);
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
        public string Culture { get; set; }
        public User User { get; set; }
        public Action Action { get; set; }
        public Error Error { get; set; }

    }

    public class User
    {
        public string DisplayName { get; set; }
        public string EMailAddress { get; set; }
    }

    public class Action
    {
        public string Redirect { get; set; }
        public Settings Settings { get; set; }
        public Error Error { get; set; }
    }


    public class Settings
    {
        public class Server
        {
            public string Type { get; set; }
            public string Url { get; set; }
            public string Name { get; set; }
            public string ServerData { get; set; }
        }
        public List<Server> Servers { get; set; }

    }
    public class Error
    {
        public decimal ErrorCode { get; set; }
        public string Message { get; set; }
        public string DebugData { get; set; }

    }

}
