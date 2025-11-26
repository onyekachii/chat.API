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
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>()
            .HasIndex(oi => new { oi.Name, oi.AppId }).IsUnique();

            modelBuilder.Entity<Group>()
                .HasMany(g => g.Users)
                .WithMany(u => u.Groups);
        }
    }
}
