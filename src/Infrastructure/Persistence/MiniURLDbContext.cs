using Microsoft.EntityFrameworkCore;
using MiniURL.Application.Common.Interfaces;
using MiniURL.Domain.Common;
using MiniURL.Domain.Entities;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

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

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.Now; // Todo: Create service to remove this dependency.
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.Now; // Todo: Create service to remove this dependency.
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
