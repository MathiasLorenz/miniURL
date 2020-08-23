using Microsoft.EntityFrameworkCore;
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
            await SeedPersistedURLs(cancellationToken);
        }

        private async Task SeedPersistedURLs(CancellationToken cancellationToken)
        {
            if (_ctx.PersistedURLs.Any())
            {
                return;
            }

            var users = await _ctx.Users.ToListAsync();

            var list = new List<PersistedURL>
            {
                new PersistedURL
                {
                    URL = "www.google.com",
                    ShortURL = "abcdefgh",
                },
                new PersistedURL
                {
                    URL = "https://www.dr.dk/drtv/",
                    ShortURL = "objuyhgl",
                    User = users[0]
                },
                new PersistedURL
                {
                    URL = "https://www.google.com/search?q=ef+core+renames+table&oq=ef+core+renames+table&aqs=chrome..69i57j0l7.10922j0j7&sourceid=chrome&ie=UTF-8",
                    ShortURL = "objurhgi",
                    Deleted = true
                }
            };

            _ctx.PersistedURLs.AddRange(list);

            await _ctx.SaveChangesAsync(cancellationToken);
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
