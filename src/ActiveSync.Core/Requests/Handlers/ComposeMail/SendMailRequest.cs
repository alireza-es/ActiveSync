using System.Text;
using System.Xml.Linq;
using ActiveSync.Core.Constants;
using ActiveSync.Core.Exceptions;
using ActiveSync.Core.Helper;
using ActiveSync.Core.Injection;
using ActiveSync.Core.ResponseObjects.ComposeMail;
using ActiveSync.SyncContract.Service;

namespace ActiveSync.Core.Requests.Handlers.ComposeMail
{
    public class SendMailRequest:ASRequest<SendMailResponse>
    {
        public string ClientId { get; set; }
        public string AccountId { get; set; }
        public bool SaveInSentItems { get; set; }
        public byte[] Mime { get; set; }
        
        //TODO
        //public string TemplateID { get; set; }

        public override eRequestCommand Command
        {
            get { return eRequestCommand.SendMail; }
        }

        public IEmailService MailService { get; set; }

        protected override void Initialize(string xmlRequest)
        {
            if (string.IsNullOrWhiteSpace(xmlRequest))
                throw new InvalidRequestException("SendMail Request Content is Empty.");

            var root = XDocument.Parse(xmlRequest).Root;
            this.ClientId = root.GetElementValueAsString(ComposeMailStrings.ClientId);
            this.AccountId = root.GetElementValueAsString(ComposeMailStrings.AccountId);
            this.SaveInSentItems = root.HasChildElement(ComposeMailStrings.SaveInSentItems);

            var mimeString = root.GetElementValueAsString(ComposeMailStrings.Mime);
            mimeString = mimeString.Replace("\n", "\r\n");

            this.Mime = Encoding.UTF8.GetBytes(mimeString);

            MailService = ServiceResolver.GetService<IEmailService>();

        }
        protected override SendMailResponse HandleRequest()
        {
            var email = this.Mime.ToSyncableEmail();

            MailService.SendMail(StateManager.Credential, email);

            return new SendMailResponse()
            {
                Status = eMailStatus.Success
            };
        }

    }
}
