using Microsoft.EntityFrameworkCore;
using Social_App.API.Models.Chats;
using Social_App.API.Models.Identity;

namespace Social_App.API.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserConnection> UsersConnections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>().HasIndex(m => m.ConversationId);
            modelBuilder.Entity<UserConnection>().HasKey(x => new { x.UserId, x.ConnectionId });
            base.OnModelCreating(modelBuilder);
        }
    }
}
