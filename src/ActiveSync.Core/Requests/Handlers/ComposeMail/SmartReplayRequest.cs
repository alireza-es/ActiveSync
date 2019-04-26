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
    public class SmartReplayRequest: ASRequest<SmartReplayResponse>
    {
        public string ClientId { get; set; }
        public Source Source { get; set; }
        public string FolderId { get; set; }
        public string ItemId { get; set; }
        public string LongId { get; set; }
        public string InstanceId { get; set; }
        public string AccountId { get; set; }
        public bool SaveInSentItems { get; set; }
        public string ReplaceMime { get; set; }
        public byte[] Mime { get; set; }
        public string Status { get; set; }

        public override eRequestCommand Command
        {
            get { return eRequestCommand.SmartReplay; }
        }

        public IEmailService MailService { get; set; }

        protected override void Initialize(string xmlRequest)
        {
            if (string.IsNullOrWhiteSpace(xmlRequest))
                throw new InvalidRequestException("SendMail Request Content is Empty.");

            var root = XDocument.Parse(xmlRequest).Root;
            this.ClientId = root.GetElementValueAsString(ComposeMailStrings.ClientId);

            var sourceEl = root.GetElement(ComposeMailStrings.Source);
            if (sourceEl != null)
            {
                this.Source = new Source()
                {
                    FolderId = sourceEl.GetElementValueAsString(ComposeMailStrings.FolderId),
                    ItemId = sourceEl.GetElementValueAsString(ComposeMailStrings.ItemId),
                    LongId = sourceEl.GetElementValueAsString(ComposeMailStrings.LongId),
                    InstanceId = sourceEl.GetElementValueAsString(ComposeMailStrings.InstanceId)
                };
            }
            this.AccountId = root.GetElementValueAsString(ComposeMailStrings.AccountId);
            this.SaveInSentItems = root.HasChildElement(ComposeMailStrings.SaveInSentItems);

            var mimeString = root.GetElementValueAsString(ComposeMailStrings.Mime);
            mimeString = mimeString.Replace("\n", "\r\n");

            this.Mime = Encoding.UTF8.GetBytes(mimeString);

            MailService = ServiceResolver.GetService<IEmailService>();
        }

        protected override SmartReplayResponse HandleRequest()
        {
            return new SmartReplayResponse()
            {
                Status = eMailStatus.Success
            };
        }

    }
}
