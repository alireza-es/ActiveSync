using System;

namespace ActiveSync.Core.StateManagement.StateObjects
{
    public class SyncSessionState : StateObject
    {
        public string SyncKey { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string UserName { get; set; }
        public DateTime SyncDate { get; set; }

    }
}
