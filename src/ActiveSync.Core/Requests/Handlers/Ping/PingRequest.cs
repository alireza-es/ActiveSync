using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ActiveSync.Core.ApplicationData;
using ActiveSync.Core.Constants;
using ActiveSync.Core.Exceptions;
using ActiveSync.Core.Helper;
using ActiveSync.Core.Injection;
using ActiveSync.Core.ResponseObjects.Ping;
using ActiveSync.Core.StateManagement;
using ActiveSync.Core.StateManagement.StateObjects;
using ActiveSync.SyncContract;
using ActiveSync.SyncContract.Service;
using ActiveSync.SyncContract.Syncables;

namespace ActiveSync.Core.Requests.Handlers.Ping
{
    /// <summary>
    /// The Ping command is used to request that the server monitor specified folders for changes that would require the client to resynchronize.
    /// </summary>
    public class PingRequest : ASRequest<PingResponse>
    {
        public int HeartbeatInterval { get; set; }
        public List<PingRequestFolder> Folders { get; set; }
        public IEmailService EmailService { get; set; }
        public IContactService ContactService { get; set; }
        public IFolderService FolderService { get; set; }

        public override eRequestCommand Command
        {
            get { return eRequestCommand.Ping; }
        }
        protected override void Initialize(string xmlRequest)
        {
            if (string.IsNullOrWhiteSpace(xmlRequest))
                throw new InvalidRequestException("Ping Request Content is Empty.");

            FolderService = ServiceResolver.GetService<IFolderService>();
            EmailService = ServiceResolver.GetService<IEmailService>();
            ContactService = ServiceResolver.GetService<IContactService>();

            var root = XDocument.Parse(xmlRequest).Root;
            this.HeartbeatInterval = root.GetElementValueAsInt(PingStrings.HeartbeatInterval);
            var folderEls = root.GetDescendants(PingStrings.Folder);

            this.Folders = new List<PingRequestFolder>(folderEls.Count);
            foreach (var folderEl in folderEls)
            {
                var newFolder = new PingRequestFolder()
                {
                    Id = folderEl.GetElementValueAsString(PingStrings.Id)
                };
                var folderClass = folderEl.GetElementValueAsString(PingStrings.Class);
                newFolder.Class = (eFolderClass)Enum.Parse(typeof(eFolderClass), folderClass);

                this.Folders.Add(newFolder);
            }
        }
        protected override PingResponse HandleRequest()
        {
            return new PingResponse
            {
                Status = ePingStatus.HeartbeatExpiredWithNoChanges
            };

            var foundChanges = false;
            var changedFolderServerIds = new List<string>();

            foreach (var pingRequestFolder in this.Folders)
            {
                SyncableFolder folder = null;
                try
                {
                    folder = FolderService.GetFolder(StateManager.Credential, pingRequestFolder.Id);
                }
                catch (Exception)
                {
                    //TODO: Add Fail Satus Collection
                    continue;
                }
                if (folder == null)
                {
                    //TODO: Add Fail Satus Collection
                    continue;
                }

                var allServerAppData = new List<AppData>();
                var newAddedAppdata = new List<AppData>();
                var newDeletedServerIds = new List<string>();
                var newChangedAppData = new List<AppData>();

                if (folder.Type == eFolderType.DefaultContacts)
                {
                    var contacts = ContactService.GetContacts(StateManager.Credential, folder.Id);

                    allServerAppData.AddRange((contacts.Select(contact => new ContactAppData(contact) { ServerId = contact.Id })));

                }
                else
                {
                    var emails = EmailService.GetEmails(StateManager.Credential, folder.Id);

                    allServerAppData.AddRange(emails.Select(email => new EmailAppData(email) { ServerId = email.Id }));
                }
                string syncKey = FileStateMachine.LastSyncKey;
                var oldCollectionState = StateManager.LoadCollectionState(syncKey,
                    folder.Id) ?? new CollectionState();

                newAddedAppdata = GetNewDataItems(oldCollectionState, allServerAppData.ToArray());
                newDeletedServerIds = GetNewDeleteDataItems(oldCollectionState, allServerAppData.ToArray());
                newChangedAppData = GetChangeDataItems(oldCollectionState, allServerAppData.ToArray());

                if(newAddedAppdata.Count > 0 || newDeletedServerIds.Count > 0 || newChangedAppData.Count > 0)
                    changedFolderServerIds.Add(folder.Id);
            }


            var pingResponse = new PingResponse()
            {              
            };

            if (changedFolderServerIds.Count == 0) 
                return pingResponse;

            pingResponse.Status = ePingStatus.ChangeOccurred;
            pingResponse.FolderServerIds = changedFolderServerIds;

            return pingResponse;
        }

        private List<AppData> GetNewDataItems(CollectionState collectionState, AppData[] allAppData)
        {
            var clientIds = collectionState.Collections.Select(x => x.ServerId).ToList();
            var serverIds = allAppData.Select(x => x.ServerId).ToList();

            var newAddedIds = serverIds.Except(clientIds).ToList();
            var newItems = (from appData in allAppData
                            let newIds = newAddedIds
                            where newIds.Contains(appData.ServerId)
                            select appData).ToList();

            return newItems;
        }

        private List<AppData> GetChangeDataItems(CollectionState collectionState, AppData[] allAppData)
        {
            var clientIds = collectionState.Collections.Select(x => x.ServerId).ToList();
            var serverIds = allAppData.Select(x => x.ServerId).ToList();

            var newChangedIds = serverIds.Intersect(clientIds).ToList();
            var oldHashKeys = (from collection in collectionState.Collections
                               let clientId = newChangedIds
                               where clientIds.Contains(collection.ServerId)
                               select collection.HashKey).ToList();

            var newItems = (from appData in allAppData
                            let newIds = newChangedIds
                            let hashKey = oldHashKeys
                            where newIds.Contains(appData.ServerId) && !hashKey.Contains(appData.GenerateHash())
                            select appData)
                            .ToList();

            return newItems;
        }

        private List<string> GetNewDeleteDataItems(CollectionState collectionState, AppData[] allAppData)
        {
            var clientIds = collectionState.Collections.Select(x => x.ServerId).ToList();
            var serverIds = allAppData.Select(x => x.ServerId).ToList();

            var newDeletedIds = clientIds.Except(serverIds).ToList();

            return newDeletedIds;
        }

    }

    public class PingRequestFolder
    {
        public string Id { get; set; }
        public eFolderClass Class { get; set; }
    }
}
