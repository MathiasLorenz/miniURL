using Microsoft.EntityFrameworkCore;
using MiniURL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiniURL.Application.Common.Interfaces
{
    // This introduced a dependency on both EF Core and the Domain project...
    public interface IMiniURLDbContext
    {
        DbSet<User> Users { get; }
        DbSet<PersistedURL> PersistedURLs { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
