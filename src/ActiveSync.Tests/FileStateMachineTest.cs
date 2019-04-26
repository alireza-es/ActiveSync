using System.Collections.Generic;
using ActiveSync.Core.DeviceManagement;
using ActiveSync.Core.Injection;
using ActiveSync.Core.StateManagement;
using ActiveSync.Core.StateManagement.StateObjects;
using ActiveSync.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActiveSync.Tests
{
    [TestClass]
    public class FileStateMachineTest : BaseTest
    {
        [TestMethod]
        public void LoadFolderStateTest()
        {

            var stateManager = new StateManager(ServiceResolver.GetService<IStateMachine>());
            stateManager.UserDevice = new UserDevice() { DeviceId = "100" };


            var syncKey = stateManager.GetNewSyncKey("0");
            var folderState = new FolderHierarchyState
            {
                Folders = new List<FolderState>
                {
                    new FolderState()
                    {
                        Name = "Contacts",
                        FolderType = 1,
                        ParentId = "0",
                        ServerId =  "1234"
                    },
                    new FolderState()
                    {
                        Name = "Inbox",
                        FolderType = 1,
                        ParentId = "1234",
                        ServerId =  "12345"
                    }

                }
            };
            stateManager.SaveFolderState(syncKey, folderState);
            var savedFolderState = stateManager.LoadFolderState(syncKey);

            Assert.IsNotNull(savedFolderState);
            Assert.IsTrue(savedFolderState.Folders.Count == folderState.Folders.Count);



        }

        [TestMethod]
        public void SaveCollectionStateTest()
        {
            var stateManager = new StateManager(ServiceResolver.GetService<IStateMachine>());
            stateManager.UserDevice = new UserDevice() { DeviceId = "100" };


            var syncKey = stateManager.GetNewSyncKey("0");
            var collState = new CollectionState()
            {
                FolderId = "12321",
                Collections = new List<SyncItemState>()
                {
                    new SyncItemState()
                    {
                        ServerId = "12334",
                        HashKey = "3242342342"
                    },
                    new SyncItemState()
                    {
                         ServerId = "12334",
                         HashKey = "3242342342"
                    }
                }
            };
            stateManager.SaveCollectionState(syncKey, collState);
            var savedCollState = stateManager.LoadCollectionState(syncKey, collState.FolderId);



        }
    }
}
