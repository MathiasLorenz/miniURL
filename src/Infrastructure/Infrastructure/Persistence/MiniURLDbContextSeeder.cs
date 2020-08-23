using MiniURL.Application.Common.Interfaces;
using MiniURL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        }

        private async Task SeedUsers(CancellationToken cancellationToken)
        {
            if (_ctx.Users.Any())
            {
                return;
            }

            _ctx.Users.AddRange(new List<User>
            {
                new User
                {
                    FirstName = "Første",
                    LastName = "Manden",
                    Email = "sup@sup.com"
                },
                new User
                {
                    FirstName = "Anden",
                    LastName = "Drengen",
                    Email = "what@up.com"
                }
            });

            await _ctx.SaveChangesAsync(cancellationToken);
        }
    }
}
