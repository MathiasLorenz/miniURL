using Microsoft.EntityFrameworkCore;
using MiniURL.Domain.Entities;
using System.Reflection;

namespace Infrastructure.Persistence
{
    public class MiniURLDbContext : DbContext
    {
        public MiniURLDbContext(DbContextOptions<MiniURLDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
