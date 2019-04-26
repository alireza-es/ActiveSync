using ActiveSync.Core.DeviceManagement;
using ActiveSync.Core.ResponseObjects;

namespace ActiveSync.Core.Requests
{
    public interface IRequestHandler
    {
        void Initialize(UserDevice device, string xmlRequest);
        ASResponse Handle();
    }
}
