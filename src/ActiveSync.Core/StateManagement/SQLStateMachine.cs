//using System;
//using System.Linq;
//using ActiveSync.Core.StateManagement.StateObjects;
//using ActiveSync.SQLState.Domain;
//using ActiveSync.SQLState.Service;

//namespace ActiveSync.Core.StateManagement
//{
//    public class SQLStateMachine:IStateMachine
//    {
//        public FolderHierarchyState LoadFolderState(string deviceId, SyncKey syncKey)
//        {
//            var sessionService = new SyncSessionService();
//            var syncSession = sessionService.Load(deviceId, syncKey.ToString());

//            if (syncSession == null || syncSession.SyncFolders == null)
//                return null;

//            var folderHierarchy = new FolderHierarchyState();
//            folderHierarchy.Folders = syncSession.SyncFolders.Select(folder => new FolderState
//            {
//                ServerId = folder.ServerId,
//                Name = folder.Name,
//                ParentId = folder.ParentId,
//                FolderType = folder.FolderType
//            }).ToList();
//            folderHierarchy.SyncSession = new SyncSessionState
//            {
//                DeviceId = syncSession.UserDevice.DeviceUniqueId,
//                DeviceName = syncSession.UserDevice.DeviceName,
//                SyncKey = syncSession.SyncKey,
//                UserName = syncSession.UserDevice.UserName,
//                SyncDate = syncSession.SyncDate
//            };

//            return folderHierarchy;
//        }

//        public void SaveFolderState(string deviceId, SyncKey syncKey, FolderHierarchyState folderState)
//        {
//            var sessionService = new SyncSessionService();
//            var syncSession = sessionService.Load(deviceId, syncKey.ToString());

//            var folders = folderState.Folders.Select(fs => new SyncFolder
//            {
//                FolderType = fs.FolderType,
//                ServerId = fs.ServerId,
//                ParentId = fs.ParentId,
//                Name = fs.Name
//            }).ToList();

//            if (syncSession == null)
//            {
//                var deviceService = new UserDeviceService();
//                var device = deviceService.GetByDeviceId(deviceId) ?? new UserDevice
//                {
//                    DeviceUniqueId = deviceId
//                };
//                syncSession = new SyncSession
//                {
//                    SyncDate = DateTime.Now,
//                    SyncKey = syncKey,
//                    SyncFolders = folders,
//                    UserDevice = device
//                };

//                sessionService.Add(syncSession);
//            }
//            else
//            {
//                sessionService.Update(syncSession);
//            }
//        }

//        public CollectionState LoadCollectionState(string deviceId, SyncKey syncKey)
//        {
//            throw new System.NotImplementedException();
//        }

//        public CollectionState LoadCollectionState(string deviceId, SyncKey syncKey, string folderId)
//        {
//            throw new System.NotImplementedException();
//        }

//        public void SaveCollectionState(string deviceId, SyncKey syncKey, CollectionState collectionState)
//        {
//            throw new System.NotImplementedException();
//        }
//    }
//}
