using System;
using System.Linq;
using ActiveSync.Core.DeviceManagement;
using ActiveSync.Core.StateManagement.StateObjects;
using ActiveSync.SyncContract.Service;

namespace ActiveSync.Core.StateManagement
{
    public class StateManager
    {

        public IStateMachine StateMachine { get; set; }

        public UserDevice UserDevice { get; set; }

        public UserCredential Credential { get; set; }
        public StateManager(IStateMachine stateMachine)
        {
            this.StateMachine = stateMachine;
        }

        #region Load States

        public FolderHierarchyState LoadFolderState(SyncKey syncKey)
        {
            return StateMachine.LoadFolderState(UserDevice.DeviceId, syncKey);
        }

        public CollectionState LoadCollectionState(SyncKey syncKey, string folderId)
        {
            return StateMachine.LoadCollectionState(UserDevice.DeviceId, syncKey, folderId);
        }

        #endregion

        #region Save States

        public void SaveFolderState(SyncKey syncKey, FolderHierarchyState folderState)
        {
            StateMachine.SaveFolderState(UserDevice.DeviceId, syncKey, folderState);
        }

        public void SaveCollectionState(SyncKey syncKey, CollectionState collectionState)
        {
            StateMachine.SaveCollectionState(UserDevice.DeviceId, syncKey, collectionState);
        }

        #endregion

        #region State Key

        /// <summary>
        /// The SyncKey element is a required child element of the Collection element in Sync command requests and responses that contains a value that is used by the server to mark the synchronization state of a collection.
        /// A synchronization key of value 0 (zero) initializes the synchronization state on the server and causes a full synchronization of the collection. The server sends a response that includes a new synchronization key value. The client MUST store this synchronization key value until the client requires the key value for the next synchronization request for that collection. When the client uses this synchronization key value to do the next synchronization of the collection, the client sends this synchronization key value to the server in a Sync request. If the synchronization is successful, the server responds by sending all objects in the collection. The response includes a new synchronization key value that the client MUST use on the next synchronization of the collection.
        /// </summary>
        /// <returns></returns>
        public string GetNewSyncKey(string synckey)
        {
            SyncKey synckeyObject;

            if (string.IsNullOrWhiteSpace(synckey) || synckey == "0")
            {
                //First Initilize of Sync Key
                synckeyObject = new SyncKey
                {
                    Key = GenerateNewUniqueKey(),
                    Counter = 0
                };
            }
            else
            {
                synckeyObject = (SyncKey)synckey;
            }

            synckeyObject.Counter += 1;

            return synckeyObject.ToString();
        }

        /// <summary>
        /// This Method is not duplicate in 10 Milion Times,so It is OK for us. It should be unique for each device
        /// </summary>
        /// <returns></returns>
        private string GenerateNewUniqueKey()
        {
            var i = Guid.NewGuid().ToByteArray().Aggregate<byte, long>(1, (current, b) => current * (b + 1));

            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        #endregion

    }
}
