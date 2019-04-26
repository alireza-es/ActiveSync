using System.Xml.Linq;
using ActiveSync.Core.Constants;
using ActiveSync.Core.Exceptions;
using ActiveSync.Core.Helper;
using ActiveSync.Core.ResponseObjects.Autodiscover;

namespace ActiveSync.Core.Requests.Handlers.Autodiscover
{
    public class AutodiscoverRequest : ASRequest<AutodiscoverResponse>
    {
        public RequestType Request { get; set; }
        public override eRequestCommand Command
        {
            get { return eRequestCommand.Autodiscover; }
        }

        protected override void Initialize(string xmlRequest)
        {
            if (string.IsNullOrWhiteSpace(xmlRequest))
                throw new InvalidRequestException("Autodiscover request content is empty.");

            var root = XDocument.Parse(xmlRequest).Root;
            
            var requestEl = root.GetElement(AutodiscoverStrings.Request);
            if (requestEl != null)
            {
                this.Request = new RequestType()
                {
                    EMailAddress = requestEl.GetElementValueAsString(AutodiscoverStrings.EMailAddress),
                    AcceptableResponseSchema = requestEl.GetElementValueAsString(AutodiscoverStrings.AcceptableResponseSchema)
                };
            }
          
        }
        protected override AutodiscoverResponse HandleRequest()
        {
            return new AutodiscoverResponse()
            {

            };
        }

    }
}
