using CloudChat.Core;
using Microsoft.EntityFrameworkCore;

namespace CloudChat.Relational
{
    public class ClientDataStoreDbContext : DbContext
    {
        // LoginCredentials Table
        public DbSet<LoginCredentialsDataModel> LoginCredentials { get; set; }

        public ClientDataStoreDbContext(DbContextOptions<ClientDataStoreDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LoginCredentialsDataModel>().HasKey(a => a.Id);
            // TODO: Setup limits
        }
    }
}
