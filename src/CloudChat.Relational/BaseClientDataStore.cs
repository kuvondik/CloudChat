using CloudChat.Core;
using System.Linq;
using System.Threading.Tasks;

namespace CloudChat.Relational
{
    public class BaseClientDataStore : IClientDataStore
    {
        #region Protected Members

        protected ClientDataStoreDbContext mClientDataStoreDbContext;
        #endregion

        #region Constructor
        public BaseClientDataStore(ClientDataStoreDbContext dbContext)
        {
            mClientDataStoreDbContext = dbContext;
        }
        #endregion

        #region Implemented Methods

        public async Task<bool> HasCredentialsAsync()
        {
            return await GetLoginCredentialsAsync() != null;
        }

        public async Task<bool> EnsureDataStoreAsync()
        {
            return await mClientDataStoreDbContext.Database.EnsureCreatedAsync();
        }

        public async Task<LoginCredentialsDataModel> GetLoginCredentialsAsync()
        {
            return await Task.FromResult(mClientDataStoreDbContext.LoginCredentials.FirstOrDefault());
        }

        public async Task SaveLoginCredentialsAsync(LoginCredentialsDataModel loginCredentials)
        {
            mClientDataStoreDbContext.LoginCredentials.RemoveRange(mClientDataStoreDbContext.LoginCredentials);
            mClientDataStoreDbContext.LoginCredentials.Add(loginCredentials);
            await mClientDataStoreDbContext.SaveChangesAsync();
        }
        public async Task ClearDataStoreAsync()
        {
            mClientDataStoreDbContext.RemoveRange(mClientDataStoreDbContext.LoginCredentials);
            await mClientDataStoreDbContext.SaveChangesAsync();
        }

        #endregion
    }
}
