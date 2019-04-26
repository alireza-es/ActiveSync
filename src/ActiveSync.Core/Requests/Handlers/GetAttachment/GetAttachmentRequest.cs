using ActiveSync.Core.ResponseObjects.GetAttachment;

namespace ActiveSync.Core.Requests.Handlers.GetAttachment
{
    public class GetAttachmentRequest : ASRequest<GetAttachmentResponse>
    {
        public override eRequestCommand Command
        {
            get { return eRequestCommand.GetAttachment; }
        }

        protected override GetAttachmentResponse HandleRequest()
        {
            throw new System.NotImplementedException();
        }

        protected override void Initialize(string xmlRequest)
        {
            throw new System.NotImplementedException();
        }
    }
}
