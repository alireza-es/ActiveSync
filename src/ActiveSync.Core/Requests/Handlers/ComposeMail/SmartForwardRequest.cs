using System.Text;
using System.Xml.Linq;
using ActiveSync.Core.Constants;
using ActiveSync.Core.Exceptions;
using ActiveSync.Core.Helper;
using ActiveSync.Core.ResponseObjects.ComposeMail;

namespace ActiveSync.Core.Requests.Handlers.ComposeMail
{
    public class SmartForwardRequest: ASRequest<SmartForwardResponse>
    {
        public string ClientId { get; set; }
        public Source Source { get; set; }
        public string AccountId { get; set; }
        public bool SaveInSentItems { get; set; }
        public int ReplaceMime { get; set; }
        public byte[] Mime { get; set; }
        //TODO
        public string TemplateID { get; set; }
        public override eRequestCommand Command
        {
            get { return eRequestCommand.SmartForward; }
        }

        protected override void Initialize(string xmlRequest)
        {
            if (string.IsNullOrWhiteSpace(xmlRequest))
                throw new InvalidRequestException("SmartForward request content is empty.");

            var root = XDocument.Parse(xmlRequest).Root;
            this.ClientId = root.GetElementValueAsString(ComposeMailStrings.ClientId);
            this.AccountId = root.GetElementValueAsString(ComposeMailStrings.AccountId);
            this.SaveInSentItems = root.HasChildElement(ComposeMailStrings.SaveInSentItems);
            this.ReplaceMime = root.GetElementValueAsInt(ComposeMailStrings.ReplaceMime);
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

            var mimeString = root.GetElementValueAsString(ComposeMailStrings.Mime);
            mimeString = mimeString.Replace("\n", "\r\n");

            this.Mime = Encoding.UTF8.GetBytes(mimeString);

        }

        protected override SmartForwardResponse HandleRequest()
        {
            return new SmartForwardResponse()
            {

            };
        }
    }
}
