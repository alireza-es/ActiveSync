using System.Linq;
using ActiveSync.SyncContract.Syncables;

namespace ActiveSync.Core.Helper
{
    public static class MimeHelper
    {
        public static SyncableEmail ToSyncableEmail(this byte[] mimeData)
        {
            var email = new SyncableEmail();
            var mime = LumiSoft.Net.Mime.Mime.Parse(mimeData);
            var mimeEntity = mime.MainEntity;

            if (mimeEntity.To != null)
            {
                var emailAddresses = EmailHelper.ToEmailAddress(mimeEntity.To.ToAddressListString());
                var to = emailAddresses
                    .Aggregate("", (current, emailAddress) => current + (emailAddress.ToString() + ";"));
                to = to.Remove(to.Length - 1);
                email.To = to;
            }
            if (mimeEntity.Cc != null)
            {
                var emailAddresses = EmailHelper.ToEmailAddress(mimeEntity.Cc.ToAddressListString());
                var cc = emailAddresses
                    .Aggregate("", (current, emailAddress) => current + (emailAddress.ToString() + ";"));
                cc = cc.Remove(cc.Length - 1);
                email.Cc = cc;
            }

            if (mimeEntity.From != null)
            {
                var emailAddresses = EmailHelper.ToEmailAddress(mimeEntity.From.ToAddressListString());

                var from = emailAddresses
                    .Aggregate("", (current, emailAddress) => current + (emailAddress.ToString() + ";"));
                from = from.Remove(from.Length - 1);
                email.From = from;
            }


            email.Subject = mimeEntity.Subject;
            email.Body = mime.BodyHtml;
            email.ReplyTo = mimeEntity.ReplyTo != null ? mimeEntity.ReplyTo.ToAddressListString() : "";
            email.DateReceived = mimeEntity.Date;
            email.DisplayTo = mimeEntity.From.ToAddressListString();
            //TODO
            email.Importance = EmailImportance.Normal;

            return email;
        }
        public static byte[] ToMime(this SyncableEmail email)
        {
            return new byte[1];
        }

    }
}
