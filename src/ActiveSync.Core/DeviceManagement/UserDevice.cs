using ActiveSync.SyncContract.Service;

namespace ActiveSync.Core.DeviceManagement
{
    public class UserDevice
    {
        public string DeviceId { get; set; }
        public string DeviceType { get; set; }
        public string User { get; set; }
        public UserCredential Credential { get; set; }
    }

}