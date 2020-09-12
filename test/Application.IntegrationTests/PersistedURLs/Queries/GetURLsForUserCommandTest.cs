using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniURL.Application.Common.Exceptions;
using MiniURL.Application.PersistedURLs.Queries.GetURLsForUser;
using MiniURL.Domain.Entities;
using MiniURL.Infrastructure.Persistence;
using Shouldly;

namespace MiniURL.Application.IntegrationTests.PersistedURLs.Queries
{
    [TestClass]
    public class GetURLsForUserCommandTest : BaseTest
    {
        [TestMethod]
        public async Task GetURLsForUser_InvalidUser_ThrowsException()
        {
            var userId = await CreateAUser(DbOptions, DateTimeService);

            using (var ctx = new MiniURLDbContext(DbOptions, DateTimeService))
            {
                var query = new GetURLsForUserQuery { UserId = userId + 1 };
                var handler = new GetURLsForUserQueryHandler(ctx);

                await Should.ThrowAsync<NotFoundException>(
                    handler.Handle(query, new System.Threading.CancellationToken()));
            }
        }

        [TestMethod]
        public async Task GetURLsForUser_SeededDb_ReturnsCorrect()
        {
            await SeedDb(DbOptions, DateTimeService);
            var users = SeedData.Users();
            var persistedURLs = SeedData.PersistedURLs();

            using (var ctx = new MiniURLDbContext(DbOptions, DateTimeService))
            {
                await RunUserQuery(users, persistedURLs, ctx);
            }
        }

        private async Task RunUserQuery(List<User> users,
                                        List<PersistedURL> persistedURLs,
                                        MiniURLDbContext ctx)
        {
            for (int i = 0; i < 2; i++)
            {
                bool includedDeleted = i == 0 ? true : false;
                foreach (var user in users)
                {
                    var query = new GetURLsForUserQuery
                    {
                        UserId = user.Id,
                        IncludeDeleted = includedDeleted
                    };
                    var handler = new GetURLsForUserQueryHandler(ctx);

                    var result = await handler.Handle(query, new System.Threading.CancellationToken());

                    result.ShouldNotBeNull();
                    result.ShouldBeOfType<URLsForUserVm>();
                    result.UserId.ShouldBe(user.Id);
                    result.URLs.ShouldBeOfType<List<URLsForUserDto>>();

                    int relevantPersistedURLsCount = includedDeleted
                        ? persistedURLs.Where(x => x.UserId == user.Id).Count()
                        : persistedURLs.Where(x => x.UserId == user.Id && x.Deleted == false).Count();
                    result.URLs.Count.ShouldBe(relevantPersistedURLsCount);
                }
            }
        }
    }
}