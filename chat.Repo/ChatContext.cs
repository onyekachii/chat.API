using chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace chat.Repo
{
    public class ChatContext : DbContext
    {
        public ChatContext(DbContextOptions opt) : base(opt) { }

        public DbSet<App> Apps { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
