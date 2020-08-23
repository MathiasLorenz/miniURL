using Microsoft.EntityFrameworkCore;
using MiniURL.Application.Common.Interfaces;
using MiniURL.Domain.Entities;
using System.Reflection;

namespace MiniURL.Infrastructure.Persistence
{
    public class MiniURLDbContext : DbContext, IMiniURLDbContext
    {
        public MiniURLDbContext(DbContextOptions<MiniURLDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<PersistedURL> PersistedURLs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
