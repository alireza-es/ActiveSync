using System.Collections.Generic;
using System.IO;
using System.Xml;
using ActiveSync.Core.Constants;
using ActiveSync.Core.Helper;
using ActiveSync.SyncContract.Syncables;

namespace ActiveSync.Core.ResponseObjects.FolderSync
{
    public class FolderSyncResponse : ASResponse
    {
        public string SyncKey { get; set; }
        public eFolderSyncStatus Status { get; set; }

        //Changes dataType va Update dataType va Delete dataType va Add DataType moshakhas nistand

        private IList<SyncableFolder> _addedFolders;
        public IList<SyncableFolder> AddedFolders
        {
            get
            {
                if (_addedFolders == null)
                    _addedFolders = new List<SyncableFolder>();

                return _addedFolders;
            }
            set
            {
                _addedFolders = value;
            }
        }

        private IList<string> _deletedFoldersServerIds;
        public IList<string> DeletedFoldersServerIds
        {
            get
            {
                if (_deletedFoldersServerIds == null)
                    _deletedFoldersServerIds = new List<string>();
                return _deletedFoldersServerIds;
            }
            set
            {
                _deletedFoldersServerIds = value;
            }
        }

        private IList<SyncableFolder> _updatedFolders;
        public IList<SyncableFolder> UpdatedFolders
        {
            get
            {
                if (_updatedFolders == null)
                    _updatedFolders = new List<SyncableFolder>();
                return _updatedFolders;
            }
            set
            {
                _updatedFolders = value;
            }
        }
        public int Count
        {
            get
            {
                int addedFoldersCount = AddedFolders != null ? AddedFolders.Count : 0;
                int deletedFoldersCount = DeletedFoldersServerIds != null ? DeletedFoldersServerIds.Count : 0;
                int updatedFoldersCount = UpdatedFolders != null ? UpdatedFolders.Count : 0;
                return addedFoldersCount + deletedFoldersCount + updatedFoldersCount;
            }
        }
        public override string GetAsXML()
        {
            // Otherwise, use the properties to build the XML and then WBXML encode it
            var xmlDocument = new XmlDocument();

            xmlDocument.CreateDecleration();

            var rootNode = xmlDocument.AppendContainerNode(FolderHierarchyStrings.FolderSync, Namespaces.FolderHierarchy);

            if (string.IsNullOrWhiteSpace(SyncKey))
                SyncKey = "0";

            //SyncKey
            rootNode.AppendValueNode(FolderHierarchyStrings.SyncKey, this.SyncKey);

            //Status
            rootNode.AppendValueNode(FolderHierarchyStrings.Status,this.Status.GetHashCode().ToString());

            var changesNode = rootNode.AppendContainerNode(FolderHierarchyStrings.Changes);
            //Count
            changesNode.AppendValueNode(FolderHierarchyStrings.Count, this.Count.ToString());

            //Add
            if (this.AddedFolders != null && this.AddedFolders.Count > 0)
            {
                foreach (var syncableFolder in this.AddedFolders)
                {
                    var addedNode = changesNode.AppendContainerNode(FolderHierarchyStrings.Add);
                    addedNode.AppendValueNode(FolderHierarchyStrings.ServerId, syncableFolder.Id);
                    addedNode.AppendValueNode(FolderHierarchyStrings.ParentId, syncableFolder.ParentId);
                    addedNode.AppendValueNode(FolderHierarchyStrings.DisplayName, syncableFolder.DisplayName);
                    addedNode.AppendValueNode(FolderHierarchyStrings.Type, syncableFolder.Type.GetHashCode().ToString());
                }
            }
            //Update
            if (this.UpdatedFolders != null && this.UpdatedFolders.Count > 0)
            {
                foreach (var syncableFolder in this.UpdatedFolders)
                {
                    var updatedNode = changesNode.AppendContainerNode(FolderHierarchyStrings.Update);
                    updatedNode.AppendValueNode(FolderHierarchyStrings.ServerId, syncableFolder.Id);
                    updatedNode.AppendValueNode(FolderHierarchyStrings.ParentId, syncableFolder.ParentId);
                    updatedNode.AppendValueNode(FolderHierarchyStrings.DisplayName, syncableFolder.DisplayName);
                    updatedNode.AppendValueNode(FolderHierarchyStrings.Type, syncableFolder.Type.GetHashCode().ToString());
                }
            }

            //Delete
            if (this.DeletedFoldersServerIds != null && this.DeletedFoldersServerIds.Count > 0)
            {
                foreach (var deletedFoldersServerId in this.DeletedFoldersServerIds)
                {
                    var deletedNode = changesNode.AppendContainerNode(FolderHierarchyStrings.Delete);
                    deletedNode.AppendValueNode(FolderHierarchyStrings.ServerId, deletedFoldersServerId);
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
