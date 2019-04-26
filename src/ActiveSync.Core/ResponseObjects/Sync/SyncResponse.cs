using System.Collections.Generic;
using System.IO;
using System.Xml;
using ActiveSync.Core.Constants;
using ActiveSync.Core.Helper;
using ActiveSync.Core.ResponseObjects.Sync.ClientCommands;
using ActiveSync.Core.ResponseObjects.Sync.ServerCommands;

namespace ActiveSync.Core.ResponseObjects.Sync
{
    public class SyncResponse : ASResponse
    {
        //در هیچ کدام از مقال ها ندیدم
        //گفتم احتمالاً اضافی باشد فعلاً آن را برداشتم
        public eSyncStatus Status { get; set; }

        public int? Limit { get; set; }

        public List<ResponseCollection> Collections { get; set; }

        public override string GetAsXML()
        {
           
            var xmlDocumnet = new XmlDocument();
            xmlDocumnet.CreateDecleration();

            var rootNode = xmlDocumnet.AppendContainerNode(AirSyncStrings.Sync, Namespaces.AirSync);

            //Status
            rootNode.AppendValueNode(AirSyncStrings.Status, this.Status.GetHashCode().ToString());

            //Limit
            if (this.Limit.HasValue)
                rootNode.AppendValueNode(AirSyncStrings.Limit, this.Limit.Value.ToString());
            else if (this.Collections != null && this.Collections.Count > 0)
            {
                var collectionsNode = rootNode.AppendContainerNode(AirSyncStrings.Collections, Namespaces.AirSync, true);
                foreach (var collection in this.Collections)
                {
                    var collectionNode = collectionsNode.AppendContainerNode(AirSyncStrings.Collection, Namespaces.AirSync, true);
                    //SyncKey
                    collectionNode.AppendValueNode(AirSyncStrings.SyncKey, collection.SyncKey);
                    //CollectionId
                    collectionNode.AppendValueNode(AirSyncStrings.CollectionId, collection.CollectionId);
                    //Status
                    collectionNode.AppendValueNode(AirSyncStrings.Status, collection.Status.GetHashCode().ToString());
                    //MoreAvaiable
                    if (collection.MoreAvailable)
                        collectionNode.AppendSelfClosingNode(AirSyncStrings.MoreAvailable);
                    //Commands
                    if (collection.Commands != null && collection.Commands.Count > 0)
                    {
                        var commandsNode = collectionNode.AppendContainerNode(AirSyncStrings.Commands, Namespaces.AirSync, true);
                        foreach (var command in collection.Commands)
                        {
                            var commandNode = commandsNode.AppendContainerNode(command.CommandName, Namespaces.AirSync, true);
                            switch (command.CommandType)
                            {
                                case eServerCommandType.Add:
                                    var addCommand = (AddServerCommand)command;
                                    commandNode.AppendValueNode(AirSyncStrings.ServerId, addCommand.ServerId);
                                    commandNode.AppendChild(addCommand.AppData.GetAsXmlNode(xmlDocumnet));

                                    break;
                                case eServerCommandType.Change:
                                    var changeCommand = (ChangeServerCommand)command;
                                    commandNode.AppendValueNode(AirSyncStrings.ServerId, changeCommand.ServerId);
                                    commandNode.AppendValueNode(AirSyncStrings.Class, changeCommand.FolderClass.ToString());
                                    commandNode.AppendChild(changeCommand.AppData.GetAsXmlNode(xmlDocumnet));
                                    break;
                                case eServerCommandType.Delete:
                                    var deleteCommand = (DeleteServerCommand)command;
                                    commandNode.AppendValueNode(AirSyncStrings.ServerId, deleteCommand.ServerId);
                                    commandNode.AppendValueNode(AirSyncStrings.Class, deleteCommand.FolderClass.ToString());

                                    break;
                                case eServerCommandType.SoftDelete:
                                    var softDeleteCommand = (SoftDeleteServerCommand)command;
                                    commandNode.AppendValueNode(AirSyncStrings.ServerId, softDeleteCommand.ServerId);

                                    break;
                            }
                        }
                    }
                    if (collection.Responses != null && collection.Responses.Count > 0)
                    {
                        var responsesNode = collectionNode.AppendContainerNode(AirSyncStrings.Responses);
                        foreach (var response in collection.Responses)
                        {
                            var responseNode = responsesNode.AppendContainerNode(response.ResponseName);
                            switch (response.ClientCommandResponseType)
                            {
                                case eClientCommandResponseType.Add:
                                    var addCommandResponse = (ClientAddCommandResponse)response;
                                    responseNode.AppendValueNode(AirSyncStrings.Class, addCommandResponse.FolderClass.GetHashCode().ToString());
                                    responseNode.AppendValueNode(AirSyncStrings.ClientId, addCommandResponse.ClientId);
                                    responseNode.AppendValueNode(AirSyncStrings.ServerId, addCommandResponse.ServerId);
                                    responseNode.AppendValueNode(AirSyncStrings.Status, addCommandResponse.Status.ToString());

                                    break;
                                case eClientCommandResponseType.Change:
                                    var changeCommandResponse = (ClientChangeCommandResponse)response;
                                    responseNode.AppendValueNode(AirSyncStrings.Class, changeCommandResponse.FolderClass.ToString());
                                    responseNode.AppendValueNode(AirSyncStrings.ServerId, changeCommandResponse.ServerId);
                                    responseNode.AppendValueNode(AirSyncStrings.Status, changeCommandResponse.Status.GetHashCode().ToString());
                                    break;
                                case eClientCommandResponseType.Fetch:
                                    var fetchCommandResponse = (ClientFetchCommandResponse)response;
                                    responseNode.AppendValueNode(AirSyncStrings.ServerId, fetchCommandResponse.ServerId);
                                    responseNode.AppendValueNode(AirSyncStrings.Status, fetchCommandResponse.Status.ToString());
                                    responseNode.AppendChild(fetchCommandResponse.AppData.GetAsXmlNode(xmlDocumnet));
                                    break;
                            }
                        }
                    }
                }
            }

            return GetXmlAsString(xmlDocumnet);
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
