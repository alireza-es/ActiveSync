using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ActiveSync.Core.ApplicationData;
using ActiveSync.Core.Constants;
using ActiveSync.Core.Exceptions;
using ActiveSync.Core.Helper;
using ActiveSync.Core.Injection;
using ActiveSync.Core.Requests.Handlers.Sync.ClientCommands;
using ActiveSync.Core.ResponseObjects.Sync;
using ActiveSync.Core.ResponseObjects.Sync.ServerCommands;
using ActiveSync.Core.StateManagement;
using ActiveSync.Core.StateManagement.StateObjects;
using ActiveSync.SyncContract;
using ActiveSync.SyncContract.Service;
using ActiveSync.SyncContract.Syncables;

namespace ActiveSync.Core.Requests.Handlers.Sync
{
    public class SyncRequest : ASRequest<SyncResponse>
    {
        public List<RequestCollection> Collections { get; set; }

        // Valid values for the Wait element are 1 through 59.
        public int? Wait { get; set; }
        public int? HeartbeatInterval { get; set; }
        public int WindowSize { get; set; }
        public bool Partial { get; set; }
        public IEmailService EmailService { get; set; }
        public IContactService ContactService { get; set; }
        public IFolderService FolderService { get; set; }

        public override eRequestCommand Command
        {
            get { return eRequestCommand.Sync; }
        }

        protected override void Initialize(string xmlRequest)
        {
            if (string.IsNullOrWhiteSpace(xmlRequest))
                throw new InvalidRequestException("Sync Request Content is Empty.");

            FolderService = ServiceResolver.GetService<IFolderService>();
            EmailService = ServiceResolver.GetService<IEmailService>();
            ContactService = ServiceResolver.GetService<IContactService>();

            var root = XDocument.Parse(xmlRequest).Root;

            var collectionEls = root.GetDescendants(AirSyncStrings.Collection);

            this.Collections = new List<RequestCollection>(collectionEls.Count);
            foreach (var collectionEl in collectionEls)
            {
                var newCollection = new RequestCollection();

                newCollection.SyncKey = collectionEl.GetElementValueAsString(AirSyncStrings.SyncKey);
                newCollection.CollectionId = collectionEl.GetElementValueAsString(AirSyncStrings.CollectionId);

                var deletesAsMoves = collectionEl.HasChildElement(AirSyncStrings.DeletesAsMoves);
                newCollection.DeletesAsMoves = !deletesAsMoves || collectionEl.GetElementValueAsBoolean(AirSyncStrings.DeletesAsMoves);

                var getChanges = collectionEl.HasChildElement(AirSyncStrings.GetChanges);
                if (getChanges)
                    newCollection.GetChanges = collectionEl.GetElementValueAsBoolean(AirSyncStrings.GetChanges);
                else
                {
                    newCollection.GetChanges = newCollection.SyncKey != "0";
                }

                newCollection.WindowSize = collectionEl.GetElementValueAsInt(AirSyncStrings.WindowSize, defaultValue: 100);
                newCollection.ConversationMode = collectionEl.GetElementValueAsBoolean(AirSyncStrings.ConversationMode);
                var optionsEl = collectionEl.GetElement(AirSyncStrings.Options);
                if (optionsEl != null)
                {
                    newCollection.Options = new Options
                    {
                        FilterType = (eFilterType)optionsEl.GetElementValueAsInt(AirSyncStrings.FilterType),
                        Conflict = (eConflictPriority)optionsEl.GetElementValueAsInt(AirSyncStrings.Conflict),
                        MIMETruncation = (eMIMETruncation)optionsEl.GetElementValueAsInt(AirSyncStrings.MIMETruncation),
                        MIMESupport = (eMIMESendSupport)optionsEl.GetElementValueAsInt(AirSyncStrings.MIMESupport)
                    };
                    var bodyPreferenceEl = optionsEl.GetElement(AirSyncBaseStrings.BodyPreference, Namespaces.AirSyncBase);
                    if (bodyPreferenceEl != null)
                    {
                        newCollection.Options.BodyPreference = new BodyPreference
                        {
                            Type = (eBodyContentType)bodyPreferenceEl.GetElementValueAsInt(AirSyncBaseStrings.Type)
                        };
                        if (bodyPreferenceEl.HasChildElement(AirSyncBaseStrings.TruncationSize))
                        {
                            newCollection.Options.BodyPreference.TruncationSize =
                                bodyPreferenceEl.GetElementValueAsInt(AirSyncBaseStrings.TruncationSize);
                        }
                        if (bodyPreferenceEl.HasChildElement(AirSyncBaseStrings.AllOrNone))
                        {
                            newCollection.Options.BodyPreference.AllOrNone =
                                bodyPreferenceEl.GetElementValueAsBoolean(AirSyncBaseStrings.AllOrNone);
                        }
                        if (bodyPreferenceEl.HasChildElement(AirSyncBaseStrings.Preview))
                        {
                            newCollection.Options.BodyPreference.Preview =
                                bodyPreferenceEl.GetElementValueAsInt(AirSyncBaseStrings.Preview);
                        }

                    }
                }
                var addCommandEls = collectionEl.GetDescendants(AirSyncStrings.Add);
                foreach (var addCommandEl in addCommandEls)
                {
                    var addCommand = new ClientAddCommand(StateManager)
                    {
                        ClientId = addCommandEl.GetElementValueAsString(AirSyncStrings.ClientId),
                        FolderId = collectionEl.GetElementValueAsString(AirSyncStrings.CollectionId)
                    };
                    if (addCommandEl.HasChildElement(AirSyncStrings.Class))
                        addCommand.FolderClass = (eFolderClass)Enum.Parse(typeof(eFolderClass),
                            addCommandEl.GetElementValueAsString(AirSyncStrings.Class));


                    var appDataEl = addCommandEl.GetElement(AirSyncStrings.ApplicationData);

                    var childs = appDataEl.Elements().ToList();

                    if (childs.Any(x => x.Name.Namespace == Namespaces.PoomContacts) ||
                        childs.Any(x => x.Name.Namespace == Namespaces.Contacts2))
                    {
                        addCommand.ApplicationData = new ContactAppData();
                        appDataEl.UpdateContactAppData((ContactAppData)addCommand.ApplicationData,
                            AirSyncStrings.ApplicationData);
                    }
                    else if (childs.Any(x => x.Name.Namespace == Namespaces.Email) ||
                             childs.Any(x => x.Name.Namespace == Namespaces.Email2))
                    {
                        addCommand.ApplicationData = new EmailAppData();
                        appDataEl.UpdateEmailAppData((EmailAppData)addCommand.ApplicationData, AirSyncStrings.ApplicationData);
                    }
                    else
                    {
                        continue;
                    }


                    newCollection.ClientCommands.Add(addCommand);
                }

                var changeCommandEls = collectionEl.GetDescendants(AirSyncStrings.Change);
                foreach (var changeCommandEl in changeCommandEls)
                {
                    var changeCommand = new ClientChangeCommand(StateManager)
                    {
                        ServerId = changeCommandEl.GetElementValueAsString(AirSyncStrings.ServerId),
                        FolderId = collectionEl.GetElementValueAsString(AirSyncStrings.CollectionId)
                    };

                    var appDataEl = changeCommandEl.GetElement(AirSyncStrings.ApplicationData);

                    var childs = appDataEl.Elements();
                    var xElements = childs as IList<XElement> ?? childs.ToList();

                    if (xElements.Any(x => x.Name.Namespace == Namespaces.PoomContacts) ||
                        xElements.Any(x => x.Name.Namespace == Namespaces.Contacts2))
                    {
                        var syncableContact = ContactService.FetchContact(StateManager.Credential,
                            changeCommand.ServerId);
                        changeCommand.ApplicationData = new ContactAppData(syncableContact);
                        changeCommand.ApplicationData.ServerId = changeCommand.ServerId;

                        appDataEl.UpdateContactAppData((ContactAppData)changeCommand.ApplicationData, AirSyncStrings.ApplicationData);
                    }
                    else if (xElements.Any(x => x.Name.Namespace == Namespaces.Email) ||
                             xElements.Any(x => x.Name.Namespace == Namespaces.Email2))
                    {
                        var syncableEmail = EmailService.FetchEmail(StateManager.Credential,
                          changeCommand.ServerId);
                        changeCommand.ApplicationData = new EmailAppData(syncableEmail);
                        changeCommand.ApplicationData.ServerId = changeCommand.ServerId;

                        appDataEl.UpdateEmailAppData((EmailAppData)changeCommand.ApplicationData, AirSyncStrings.ApplicationData);
                    }


                    newCollection.ClientCommands.Add(changeCommand);
                }
                var deleteCommandEls = collectionEl.GetDescendants(AirSyncStrings.Delete);
                foreach (var deleteCommandEl in deleteCommandEls)
                {
                    newCollection.ClientCommands.Add(new ClientDeleteCommand(StateManager)
                    {
                        ServerId = deleteCommandEl.GetElementValueAsString(AirSyncStrings.ServerId),
                        FolderId = collectionEl.GetElementValueAsString(AirSyncStrings.CollectionId)

                    });
                }
                var fetchCommandEls = collectionEl.GetDescendants(AirSyncStrings.Fetch);
                foreach (var fetchCommandEl in fetchCommandEls)
                {
                    newCollection.ClientCommands.Add(new ClientFetchCommand(StateManager)
                    {
                        ServerId = fetchCommandEl.GetElementValueAsString(AirSyncStrings.ServerId)
                    });
                }

                this.Collections.Add(newCollection);
            }


            this.Wait = root.GetElementValueAsNullableInt(AirSyncStrings.Wait);
            this.HeartbeatInterval = root.GetElementValueAsNullableInt(AirSyncStrings.HeartbeatInterval);
            this.WindowSize = root.GetElementValueAsInt(AirSyncStrings.WindowSize, defaultValue: 100);
            this.Partial = root.GetElementValueAsBoolean(AirSyncStrings.Partial);

        }

        protected override SyncResponse HandleRequest()
        {

            var responseCollections = new List<ResponseCollection>();
            foreach (var collection in this.Collections)
            {
                var responseCollection = new ResponseCollection
                {
                    CollectionId = collection.CollectionId,
                };

                if (collection.SyncKey == "0")//Init Sync
                {
                    responseCollection.SyncKey = StateManager.GetNewSyncKey(collection.SyncKey);

                    responseCollection.Status = eSyncStatus.Success;
                    responseCollections.Add(responseCollection);
                    continue;
                }
                else
                {

                    SyncableFolder folder;
                    try
                    {
                        folder = FolderService.GetFolder(StateManager.Credential, collection.CollectionId);
                    }
                    catch (Exception)
                    {
                        responseCollection.Status = eSyncStatus.FolderHierarchyHasChanged;
                        responseCollections.Add(responseCollection);
                        continue;
                    }
                    if (folder == null)
                    {
                        responseCollection.Status = eSyncStatus.FolderHierarchyHasChanged;
                        responseCollections.Add(responseCollection);
                        continue;
                    }

                    #region Load Collection State

                    var collectionState = StateManager.LoadCollectionState((SyncKey)collection.SyncKey, collection.CollectionId) ?? new CollectionState() { FolderId = collection.CollectionId };

                    #endregion

                    #region Run Client Commands

                    foreach (var commandResponse in collection.ClientCommands.Select(clientCommand => clientCommand.Excecute()))
                    {
                        if (commandResponse != null)
                            responseCollection.Responses.Add(commandResponse);
                    }

                    #endregion

                    var allServerAppData = new List<AppData>();
                    var folderClass = eFolderClass.Email;

                    if (folder.Type == eFolderType.DefaultContacts)
                    {
                        folderClass = eFolderClass.Contacts;
                        var contacts = ContactService.GetContacts(StateManager.Credential, collection.CollectionId);

                        allServerAppData.AddRange((contacts.Select(contact => new ContactAppData(contact) { ServerId = contact.Id })));
                    }
                    else
                    {
                        var emails = EmailService.GetEmails(StateManager.Credential, collection.CollectionId);
                        var bodyContentType = eBodyContentType.HTML;
                        if (collection.Options != null && collection.Options.BodyPreference != null)
                            bodyContentType = collection.Options.BodyPreference.Type;

                        allServerAppData.AddRange(emails.Select(email => new EmailAppData(email) { ServerId = email.Id, BodyContentType = bodyContentType }));
                    }

                    responseCollection.SyncKey = collection.SyncKey;

                    var newAddedAppdata = GetNewDataItems(collectionState, allServerAppData.ToArray());
                    var newDeletedServerIds = GetNewDeleteDataItems(collectionState, allServerAppData.ToArray());
                    var newChangedAppData = GetChangeDataItems(collectionState, allServerAppData.ToArray());
                    //var newSoftDeleteAppData = GetNewSoftDeleteDataItems(oldCollectionState, allServerAppData.ToArray());

                    responseCollection.Commands = new List<ServerCommand>();
                    foreach (var appData in newAddedAppdata)
                    {
                        responseCollection.Commands.Add(new AddServerCommand
                        {
                            ServerId = appData.ServerId,
                            AppData = appData
                        });

                        collectionState.AddItem(new SyncItemState
                        {
                            ServerId = appData.ServerId,
                            HashKey = appData.GenerateHash()
                        });
                    }
                    foreach (var deletedServerId in newDeletedServerIds)
                    {
                        responseCollection.Commands.Add(new DeleteServerCommand()
                        {
                            ServerId = deletedServerId,
                            FolderClass = folderClass
                        });
                        collectionState.DeleteItem(deletedServerId);
                    }
                    foreach (var appData in newChangedAppData)
                    {
                        responseCollection.Commands.Add(new ChangeServerCommand()
                        {
                            ServerId = appData.ServerId,
                            AppData = appData
                        });
                        collectionState.UpdateItem(appData.ServerId, appData.GenerateHash());
                    }
                    //foreach (var appData in newSoftDeleteAppData)
                    //{
                    //    responseCollection.Commands.Add(new SoftDeleteServerCommand()
                    //    {
                    //        ServerId = appData.ServerId
                    //    });
                    //    collectionState.DeleteItem(appData.ServerId);
                    //}
                    responseCollection.Status = eSyncStatus.Success;

                    StateManager.SaveCollectionState(responseCollection.SyncKey, collectionState);

                    //if (responseCollection.Commands.Count > 0)
                    responseCollections.Add(responseCollection);
                }


            }

            var syncResponse = new SyncResponse
            {
                Status = eSyncStatus.Success,
                Collections = responseCollections
            };

            return syncResponse;
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

        private List<AppData> GetNewSoftDeleteDataItems(CollectionState collectionState, AppData[] allAppData)
        {
            var clientIds = collectionState.Collections.Select(x => x.ServerId).ToList();
            var serverIds = allAppData.Select(x => x.ServerId).ToList();

            var newDeletedIds = serverIds.Except(clientIds).ToList();

            var newItems = (from appData in allAppData
                            let newIds = newDeletedIds
                            where newIds.Contains(appData.ServerId)
                            select appData)
                            .ToList();

            return newItems;
        }
        private ResponseCollection ErrorResponseCollection(string collectionId, eSyncStatus errorStatus)
        {
            return new ResponseCollection()
            {
                CollectionId = collectionId,
                Status = errorStatus
            };
        }
    }
}
