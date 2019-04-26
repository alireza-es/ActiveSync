using System.Collections.Generic;
using ActiveSync.SyncContract.Service;
using ActiveSync.SyncContract.Syncables;

namespace ActiveSync.MockImplementation.Service
{
    public class EmailService:IEmailService
    {
        public SendMailResult SendMail(UserCredential userCredential, SyncableEmail email)
        {
            throw new System.NotImplementedException();
        }
        public void ReadMail(UserCredential credential, string id)
        {

        }
        public void UnReadMail(UserCredential credential, string id)
        {

        }
        public SyncableEmail EditMail(UserCredential credential, SyncableEmail email)
        {
            return new SyncableEmail();
        }
        public IList<SyncableEmail> GetEmails(UserCredential userCredential, string folderId)
        {
            throw new System.NotImplementedException();
        }

        public void ReplyEmail(UserCredential userCredential, SyncableEmail email)
        {
            throw new System.NotImplementedException();
        }

        public void ForwardEmail(UserCredential userCredential, SyncableEmail email)
        {
            throw new System.NotImplementedException();
        }

        public SyncableEmail FetchEmail(UserCredential userCredential, string id)
        {
            throw new System.NotImplementedException();
        }

        public void MoveConversation(UserCredential userCredential, string converstationId, string folderId)
        {
            throw new System.NotImplementedException();
        }
    }
}
