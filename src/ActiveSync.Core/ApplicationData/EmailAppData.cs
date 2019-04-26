using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using ActiveSync.Core.Constants;
using ActiveSync.Core.Helper;
using ActiveSync.Core.Requests.Handlers.Sync;
using ActiveSync.SyncContract.Syncables;
using LumiSoft.Net.Mime;

namespace ActiveSync.Core.ApplicationData
{
    public class EmailAppData : AppData
    {
        public eBodyContentType BodyContentType { get; set; }

        public EmailAppData()
        {
            Email = new SyncableEmail();
        }
        public EmailAppData(SyncableEmail email)
        {
            Email = email;
        }
        public SyncableEmail Email { get; set; }
        public override XmlNode GetAsXmlNode(XmlDocument xmlDocument)
        {

            var applicationDataNode = xmlDocument.CreateElement(AirSyncStrings.ApplicationData, Namespaces.AirSync);

            applicationDataNode.AppendValueNode(EmailStrings.MessageClass, "IPM.Note", Namespaces.Email);
            applicationDataNode.AppendValueNode(EmailStrings.InternetCPID, "65001", Namespaces.Email);
            applicationDataNode.AppendValueNode(EmailStrings.ContentClass, "urn:content-classes:message", Namespaces.Email);
            applicationDataNode.AppendValueNodeIfNotNull(EmailStrings.To, Email.To, Namespaces.Email);
            //applicationDataNode.AppendValueNodeIfNotNull(EmailStrings.CC, Email.Cc, Namespaces.Email);
            applicationDataNode.AppendValueNodeIfNotNull(EmailStrings.From, Email.From, Namespaces.Email);
            applicationDataNode.AppendValueNodeIfNotNull(EmailStrings.Subject, Email.Subject, Namespaces.Email);

            var emailBodyNode = applicationDataNode.AppendContainerNode(AirSyncBaseStrings.Body, Namespaces.AirSyncBase);
            emailBodyNode.AppendValueNode(AirSyncBaseStrings.Type, this.BodyContentType.GetHashCode().ToString(), Namespaces.AirSyncBase);
            emailBodyNode.AppendValueNode(AirSyncBaseStrings.Truncated, "0", Namespaces.AirSyncBase);

            int bodyDataSize = 0;
            string bodyDataValue = string.Empty;
            if (Email.Body != null)
            {                
                //Email Body
                switch (this.BodyContentType)
                {
                    case eBodyContentType.PlainText:
                    case eBodyContentType.HTML:
                    case eBodyContentType.RTF:
                        bodyDataSize = Encoding.UTF8.GetByteCount(Email.Body);
                        bodyDataValue = Email.Body;
                        break;
                    case eBodyContentType.MIME:
                        bodyDataValue = GetEmailAsMimeString(out bodyDataSize);
                        break;
                }

            }
            emailBodyNode.AppendValueNode(AirSyncBaseStrings.Data, bodyDataValue, Namespaces.AirSyncBase);
            emailBodyNode.AppendValueNode(AirSyncBaseStrings.EstimatedDataSize, bodyDataSize.ToString(), Namespaces.AirSyncBase);

            applicationDataNode.AppendValueNodeIfNotNull(EmailStrings.ReplyTo, Email.ReplyTo, Namespaces.Email);
            applicationDataNode.AppendValueNodeIfNotNull(EmailStrings.DateReceived, Email.DateReceived.ToString("yyyy-MM-ddThh:mm:ss.fffZ", CultureInfo.InvariantCulture), Namespaces.Email);
            applicationDataNode.AppendValueNodeIfNotNull(EmailStrings.DisplayTo, Email.DisplayTo, Namespaces.Email);
            applicationDataNode.AppendValueNodeIfNotNull(EmailStrings.ThreadTopic, Email.ThreadTopic, Namespaces.Email);

            applicationDataNode.AppendValueNode(EmailStrings.Importance, Email.Importance.GetHashCode().ToString(), Namespaces.Email);
            
            applicationDataNode.AppendValueNodeIfNotNull(EmailStrings.Read, Email.Read.ToBitString(), Namespaces.Email);

            applicationDataNode.AppendSelfClosingNode(EmailStrings.Flag);

            if (Email.Categories == null || Email.Categories.Count == 0)
                applicationDataNode.AppendSelfClosingNode(EmailStrings.Categories, Namespaces.Email);
            else
            {
                var categoriesNode = applicationDataNode.AppendContainerNode(EmailStrings.Categories, Namespaces.Email);
                foreach (var category in Email.Categories)
                    categoriesNode.AppendValueNodeIfNotNull(EmailStrings.Category, category, Namespaces.Email);
            }

            applicationDataNode.AppendValueNodeIfNotNull(Email2Strings.ConversationId, Email.ConversationId, Namespaces.Email2);
            applicationDataNode.AppendValueNodeIfNotNull(Email2Strings.ConversationIndex, Email.ConversationIndex, Namespaces.Email2);

            //The ReceivedAsBcc element is not included in the command response if the value is 0 (zero, meaning FALSE).
            if (Email.ReceivedAsBcc)
                applicationDataNode.AppendValueNode(Email2Strings.ReceivedAsBcc, Email.ReceivedAsBcc.ToBitString(), Namespaces.Email2);

            //The email2:Sender element is not sent to the client when the email2:Sender element and the From element have the same value, or when the email2:Sender element value is NULL.
            if (!string.IsNullOrWhiteSpace(Email.Sender) && Email.Sender != Email.From)
                applicationDataNode.AppendValueNode(Email2Strings.Sender, Email.Sender, Namespaces.Email2);

            if (Email.AccountId != Guid.Empty)
                applicationDataNode.AppendValueNodeIfNotNull(Email2Strings.AccountId, Email.AccountId.ToString(), Namespaces.Email2);



            return applicationDataNode;
        }

        private string GetEmailAsMimeString(out int byteCount)
        {
            var mime = new LumiSoft.Net.Mime.Mime();
            var mimeEntity = mime.MainEntity;
            var emailAddressFrom = EmailHelper.ToEmailAddress(Email.From);
            mimeEntity.From = new AddressList();
            foreach (var emailAddress in emailAddressFrom)
                mimeEntity.From.Add(new MailboxAddress(emailAddress.DisplayName, emailAddress.Address));

            var emailAddressTo = EmailHelper.ToEmailAddress(Email.To);
            mimeEntity.To = new AddressList();
            foreach (var emailAddress in emailAddressTo)
                mimeEntity.To.Add(new MailboxAddress(emailAddress.DisplayName, emailAddress.Address));

            var emailAddressCc = EmailHelper.ToEmailAddress(Email.Cc);
            mimeEntity.Cc = new AddressList();
            foreach (var emailAddress in emailAddressCc)
                mimeEntity.Cc.Add(new MailboxAddress(emailAddress.DisplayName, emailAddress.Address));

            mimeEntity.Subject = Email.Subject;
            mimeEntity.ContentType = MediaType_enum.Text_html;
            mimeEntity.ContentTransferEncoding = ContentTransferEncoding_enum.Base64;
            mimeEntity.DataText = Email.Body;

            var mimeString = mime.ToStringData();
            byteCount = mime.ToByteData().Count();

            return mimeString;
        }

        protected override string[] GetHashKeys()
        {
            return new string[]
            {
                Email.From,
                Email.To,
                Email.Subject,
                Email.Body,
                Email.FolderId,
                Email.Importance.ToString()
            };
        }

    }


}
