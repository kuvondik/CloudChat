using System.Threading.Tasks;

namespace CloudChat.Core
{
    public interface IClientDataStore
    {
        Task<bool> HasCredentialsAsync();
        Task<bool> EnsureDataStoreAsync();
        Task<LoginCredentialsDataModel> GetLoginCredentialsAsync();
        Task SaveLoginCredentialsAsync(LoginCredentialsDataModel loginCredentials);
        Task ClearDataStoreAsync();
    }
}
