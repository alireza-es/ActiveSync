using System.Collections.Generic;
using ActiveSync.SyncContract.Service;
using ActiveSync.SyncContract.Syncables;

namespace ActiveSync.MockImplementation.Service
{
    public class ContactService:IContactService
    {
        public IList<SyncableContact> GetContacts(UserCredential userCredential, string folderId)
        {
            throw new System.NotImplementedException();
        }

        public SyncableContact FetchContact(UserCredential userCredential, string id)
        {
            throw new System.NotImplementedException();
        }

        public UpdateContactResult UpdateContact(UserCredential userCredential, SyncableContact contact)
        {
            throw new System.NotImplementedException();
        }

        public AddContactResult AddContact(UserCredential userCredential, SyncableContact contact, out string serverId)
        {
            throw new System.NotImplementedException();
        }

        public DeleteContactResult DeleteContact(UserCredential userCredential, string id)
        {
            throw new System.NotImplementedException();
        }
    }
}
