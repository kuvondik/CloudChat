using CloudChat.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudChat.Server
{
    public class ChatMessageDbContext : DbContext
    {
        #region Entities

        public DbSet<DevicesEntity> Devices { get; set; }
        public DbSet<AccessEntity> Access { get; set; }
        public DbSet<BlockListEntity> BlockList { get; set; }
        public DbSet<ParticipantsEntity> Participants { get; set; }
        public DbSet<UsersEntity> Users { get; set; }
        public DbSet<UserProfilePictureEntity> UserProfilePicture { get; set; }
        public DbSet<ContactsEntity> Contacts { get; set; }
        public DbSet<ConversationsEntity> Conversations { get; set; }
        public DbSet<ConversationPictureEntity> ConversationPicture { get; set; }
        public DbSet<MessagesEntity> Messages { get; set; }
        public DbSet<ReportsEntity> Reports { get; set; }
        public DbSet<DeletedConversationsEntity> DeletedConversations { get; set; }
        public DbSet<DeletedMessagesEntity> DeletedMessages { get; set; }
        #endregion

        #region Constructor

        public ChatMessageDbContext(DbContextOptions<ChatMessageDbContext> options) : base(options)
        {
        }
        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Fluent API
            modelBuilder.Entity<ConversationsEntity>().HasIndex(c => c.ChannelId);
        }
    }
}
