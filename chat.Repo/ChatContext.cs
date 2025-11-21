using chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace chat.Repo
{
    public class ChatContext : DbContext
    {
        public ChatContext(DbContextOptions<ChatContext> opt) : base(opt) { }

        public DbSet<App> Apps { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Fluent API configurations here if needed
        }
    }
}
