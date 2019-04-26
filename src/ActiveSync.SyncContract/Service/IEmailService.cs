using System.Collections.Generic;
using ActiveSync.SyncContract.Syncables;

namespace ActiveSync.SyncContract.Service
{
    public interface IEmailService
    {
        SendMailResult SendMail(UserCredential userCredential,SyncableEmail email);
        IList<SyncableEmail> GetEmails(UserCredential userCredential, string folderId);
        void ReadMail(UserCredential credential, string id);
        void UnReadMail(UserCredential credential, string id);
        SyncableEmail EditMail(UserCredential credential, SyncableEmail manipulatedEmail);
        void ReplyEmail(UserCredential userCredential, SyncableEmail email);
        void ForwardEmail(UserCredential userCredential, SyncableEmail email);
        SyncableEmail FetchEmail(UserCredential userCredential, string id);
        void MoveConversation(UserCredential userCredential, string converstationId, string folderId);
    }

    #region Command Results

    public enum SendMailResult:byte
    {
        /// <summary>
        /// Server successfully completed command.
        /// => Global
        /// </summary>
        Success = 1,
        AccessDenied = 3,
        ServerUnavailable = 4,
        DeniedByPolicy = 7        
    }

    #endregion
}
