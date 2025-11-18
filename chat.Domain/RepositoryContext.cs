using chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace chat.Domain
{
    public class RepositoryContext<T> : DbContext
    {
        public RepositoryContext(DbContextOptions opt) : base(opt) { }

        public DbSet<App<T>> Apps { get; set; }
        public DbSet<Group<T>> Groups { get; set; }
        public DbSet<User<T>> Users { get; set; }
    }
}
