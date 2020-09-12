using Microsoft.EntityFrameworkCore;
using MiniURL.Application.Common.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniURL.Infrastructure.Persistence
{
    public class MiniURLDbContextSeeder
    {
        private readonly IMiniURLDbContext _ctx;

        public MiniURLDbContextSeeder(IMiniURLDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task SeedAllAsync(CancellationToken cancellationToken)
        {
            await SeedUsers(cancellationToken);
            await SeedPersistedURLs(cancellationToken);
        }

        private async Task SeedPersistedURLs(CancellationToken cancellationToken)
        {
            if (_ctx.PersistedURLs.Any())
            {
                return;
            }

            var users = await _ctx.Users.ToListAsync();
            var entities = SeedData.PersistedURLs(users);
            await _ctx.PersistedURLs.AddRangeAsync(entities);
            await _ctx.SaveChangesAsync(cancellationToken);
        }

        private async Task SeedUsers(CancellationToken cancellationToken)
        {
            if (_ctx.Users.Any())
            {
                return;
            }

            var entities = SeedData.Users();
            await _ctx.Users.AddRangeAsync(entities);
            await _ctx.SaveChangesAsync(cancellationToken);
        }
    }
}
