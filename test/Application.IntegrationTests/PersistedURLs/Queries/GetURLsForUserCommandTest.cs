using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniURL.Application.Common.Exceptions;
using MiniURL.Application.PersistedURLs.Queries.GetURLsForUser;
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
        public async Task GetURLsForUser_DoNotIncludeDeleted_ReturnsCorrect()
        {
            // I'll be dependent on the db seed.
            // Todo: Should maybe refactor the entities out of the seed,
            // such that I could see beforehand how many entries were added?
            await SeedDb(DbOptions, DateTimeService);

            using (var ctx = new MiniURLDbContext(DbOptions, DateTimeService))
            {
                var user = await ctx.Users.FirstAsync();
                var query = new GetURLsForUserQuery
                {
                    UserId = user.Id,
                    IncludeDeleted = false
                };
                var handler = new GetURLsForUserQueryHandler(ctx);

                var result = await handler.Handle(query, new System.Threading.CancellationToken());

                result.ShouldNotBeNull();
                result.ShouldBeOfType<URLsForUserVm>();
                result.UserId.ShouldBe(user.Id);
                result.URLs.ShouldBeOfType<List<URLsForUserDto>>();
                result.URLs.Count.ShouldBe(1); // Todo: This must be fixed after seeding has been updated
            }
        }

        // [TestMethod]
        // public async Task GetURLsForUser_DoIncludeDeleted_ReturnsCorrect()
        // {

        // }
    }
}