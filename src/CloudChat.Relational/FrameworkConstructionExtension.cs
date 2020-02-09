using Dna;
using CloudChat.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudChat.Relational
{
    public static class FrameworkConstructionExtension
    {
        public static FrameworkConstruction AddClientDataStore(this FrameworkConstruction construction)
        {
            construction.Services.AddDbContext<ClientDataStoreDbContext>(options =>
            {
                options.UseSqlite(construction.Configuration.GetConnectionString("ClientDataStoreConnection"));
            }, contextLifetime: ServiceLifetime.Transient);

            construction.Services.AddTransient<IClientDataStore>(provider => new BaseClientDataStore(provider.GetService<ClientDataStoreDbContext>()));
            return construction;
        }
    }
}
