using System.Collections.Generic;
using System.IO;
using System.Xml;
using ActiveSync.Core.Constants;
using ActiveSync.Core.Helper;

namespace ActiveSync.Core.ResponseObjects.Ping
{
    public class PingResponse : ASResponse
    {
        public ePingStatus Status { get; set; }
        public IList<string> FolderServerIds { get; set; }
        public string MaxFolders { get; set; }
        public int HeartbeatInterval { get; set; }

        public override string GetAsXML()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.CreateDecleration();

            var rootNode = xmlDocument.AppendContainerNode(PingStrings.Ping, Namespaces.Ping);

            //Status
            rootNode.AppendValueNode(PingStrings.Status, this.Status.GetHashCode().ToString());

            //MaxFolders
            if (this.Status == ePingStatus.MaxFolderExceed)
                rootNode.AppendValueNode(PingStrings.MaxFolders, this.MaxFolders);

            //HeartbeatInterval
            //rootNode.AppendValueNode(PingStrings.HeartbeatInterval, this.HeartbeatInterval.GetHashCode().ToString());

            if (this.FolderServerIds != null && this.FolderServerIds.Count > 0)
            {
                var foldersNode = rootNode.AppendContainerNode(PingStrings.Folders);
                foreach (var folder in this.FolderServerIds)
                    foldersNode.AppendValueNode(PingStrings.Folder, folder);
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

    public enum ePingStatus : byte
    {
        /// <summary>
        /// The heartbeat interval expired before any changes occurred in the folders being monitored.
        /// Resolution: Reissue the Ping command request.
        /// </summary>
        HeartbeatExpiredWithNoChanges = 1,
        /// <summary>
        /// Changes occurred in at least one of the monitored folders. The response specifies the changed folders.
        /// Resolution: Issue a Sync command request (section 2.2.2.19) for each folder that was specified in the Ping command response to retrieve the server changes. Reissue the Ping command when the Sync command completes to stay up to date.
        /// </summary>
        ChangeOccurred = 2,
        /// <summary>
        /// The Ping command request omitted required parameters.
        /// The Ping command request did not specify all the necessary parameters. The client MUST issue a Ping request that includes both the heartbeat interval and the folder list at least once. The server saves the heartbeat interval value (section 2.2.3.79.1), so only the folder list is required on subsequent requests.
        /// Reissue the Ping command request with the entire XML body.
        /// </summary>
        InvalidParameters = 3,
        /// <summary>
        /// Syntax error in Ping command request. Frequently caused by poorly formatted WBXML.
        /// </summary>
        InvalidPingRequest = 4,
        /// <summary>
        /// The specified heartbeat interval is outside the allowed range. For intervals that were too short, the response contains the shortest allowed interval. For intervals that were too long, the response contains the longest allowed interval.
        /// The client sent a Ping command request with a heartbeat interval that was either too long or too short.
        /// Reissue the Ping command by using a heartbeat interval inside the allowed range. Setting the interval to the value returned in the Ping response will most closely accommodate the original value specified.
        /// </summary>
        InvalidHeartbeat = 5,
        /// <summary>
        /// The Ping command request specified more than the allowed number of folders to monitor. The response indicates the allowed number in the MaxFolders element (section 2.2.3.92).
        /// The client sent a Ping command request that specified more folders than the server is configured to monitor.
        /// Resolution: Direct the user to select fewer folders to monitor. Resend the Ping command request with the new, shorter list.
        /// </summary>
        MaxFolderExceed = 6,
        /// <summary>
        /// The folder hierarchy is out of date; a folder hierarchy sync is required.
        /// Resolution: Issue a FolderSync command (section 2.2.2.4) to get the new hierarchy and prompt the user, if it is necessary, for new folders to monitor. Reissue the Ping command.
        /// </summary>
        FolderSyncRequired = 7,
        /// <summary>
        /// An error occurred on the server.
        /// Server misconfiguration, temporary system issue, or bad item. This is frequently a transient condition. 
        /// </summary>
        ServerError = 8
    }
}
