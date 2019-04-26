using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using ActiveSync.Core.StateManagement.StateObjects;

namespace ActiveSync.Core.StateManagement
{
    public class FileStateMachine : IStateMachine
    {
        private String rootFolder = ConfigurationManager.AppSettings["SyncStatesPath"];
        //File Path (Folders):$/syncdata/{deviceid}/folders/fld-{synckey}.xml
        //File Path (Collections):$/syncdata/{deviceid}/collections/coll-{folderid}-{synckey}.xml
        private const bool DeleteOldData = true;
        public FolderHierarchyState LoadFolderState(string deviceId, SyncKey syncKey)
        {
            var folderPath = String.Format("{0}\\folders", deviceId);
            folderPath = Path.Combine(rootFolder, folderPath);
            var filePath = String.Format("fld-{0}.xml", syncKey);
            filePath = Path.Combine(folderPath, filePath);

            if (!File.Exists(filePath)) return null;

            var serializer = new XmlSerializer(typeof(FolderHierarchyState));

            using (var reader = XmlReader.Create(filePath))
            {
                return (FolderHierarchyState)serializer.Deserialize(reader);
            }

        }

        public void SaveFolderState(string deviceId, SyncKey syncKey, StateObjects.FolderHierarchyState folderState)
        {
            var folderPath = String.Format("{0}\\folders", deviceId);
            folderPath = Path.Combine(rootFolder, folderPath);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = String.Format("fld-{0}.xml", syncKey);
            filePath = Path.Combine(folderPath, filePath);

            var dsFolderState = new DataSet();

            var xmlSerializer = new XmlSerializer(folderState.GetType());
            var writer = new StringWriter();
            xmlSerializer.Serialize(writer, folderState);
            var reader = new StringReader(writer.ToString());

            dsFolderState.ReadXml(reader);

            dsFolderState.WriteXml(filePath);

            if (DeleteOldData)
            {
                foreach (var oldFilePath in from oldFilePath in Directory.GetFiles(folderPath) let oldFileName = Path.GetFileName(oldFilePath) where oldFileName != null && oldFileName.StartsWith("fld-") && oldFileName != Path.GetFileName(filePath) select oldFilePath)
                {
                    File.Delete(oldFilePath);
                }
            }
        }

        public CollectionState LoadCollectionState(string deviceId, SyncKey syncKey, string folderId)
        {
            var folderPath = String.Format("{0}\\collections", deviceId);
            folderPath = Path.Combine(rootFolder, folderPath);

            var filePath = String.Format("coll-{0}-{1}.xml", folderId, syncKey);
            filePath = Path.Combine(folderPath, filePath);

            if (!File.Exists(filePath)) return null;

            var serializer = new XmlSerializer(typeof(CollectionState));

            using (var reader = XmlReader.Create(filePath))
            {
                return (CollectionState)serializer.Deserialize(reader);
            }
        }

        public void SaveCollectionState(string deviceId, SyncKey syncKey, CollectionState collectionState)
        {
            FileStateMachine.LastSyncKey = syncKey;
            var folderPath = String.Format("{0}\\collections", deviceId);
            folderPath = Path.Combine(rootFolder, folderPath);

            var collectionPrefix = "coll-" + collectionState.FolderId;
            var filePath = String.Format("{0}-{1}.xml", collectionPrefix, syncKey);
            filePath = Path.Combine(folderPath, filePath);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var dsCollectionState = new DataSet();
            var xmlSerializer = new XmlSerializer(collectionState.GetType());
            var writer = new StringWriter();
            xmlSerializer.Serialize(writer, collectionState);
            var reader = new StringReader(writer.ToString());

            dsCollectionState.ReadXml(reader);

            dsCollectionState.Tables[0].TableName = "CollectionState";
            dsCollectionState.DataSetName = "CollectionStates";

            dsCollectionState.WriteXml(filePath);


            if (DeleteOldData)
            {
                foreach (var oldFilePath in from oldFilePath in Directory.GetFiles(folderPath) let oldFileName = Path.GetFileName(oldFilePath) where oldFileName != null && oldFileName.StartsWith(collectionPrefix) && oldFileName != Path.GetFileName(filePath) select oldFilePath)
                {
                    File.Delete(oldFilePath);
                }
            }
        }

        public static string LastSyncKey = "";
    }
}
