namespace ActiveSync.SyncContract.Service
{
    public interface IAuthenticationService
    {
        bool Authenticate(string username, string password);
    }
}
