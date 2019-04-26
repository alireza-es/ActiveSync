using ActiveSync.Core.DeviceManagement;
using ActiveSync.Core.Injection;
using ActiveSync.Core.ResponseObjects;
using ActiveSync.Core.StateManagement;
using ActiveSync.SyncContract.Service;

namespace ActiveSync.Core.Requests
{
    /// <summary>
    /// Active Sync Request
    /// </summary>
    /// <typeparam name="T">Response Object</typeparam>
    public abstract class ASRequest<T> : IRequestHandler where T : ASResponse
    {
        public IAuthenticationService AuthenticationService { get; set; }
        protected StateManager StateManager { get; set; }
        public abstract eRequestCommand Command { get; }
        protected abstract void Initialize(string xmlRequest);
        protected abstract T HandleRequest();
        
        public void Initialize(UserDevice device, string xmlRequest)
        {
            StateManager = new StateManager(ServiceResolver.GetService<IStateMachine>());
            StateManager.UserDevice = device;
            StateManager.Credential = device.Credential;

            Initialize(xmlRequest);
        }
        public ASResponse Handle()
        {
            return HandleRequest();
        }
    }
}
