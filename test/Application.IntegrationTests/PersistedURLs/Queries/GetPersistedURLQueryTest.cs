using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniURL.Application.Common.Exceptions;
using MiniURL.Application.PersistedURLs.Queries.GetPersistedURL;
using MiniURL.Domain.Entities;
using MiniURL.Infrastructure.Persistence;
using Shouldly;

namespace MiniURL.Application.IntegrationTests.PersistedURLs.Queries
{
    [TestClass]
    public class GetPersistedURLQueryTest : BaseTest
    {
        [TestMethod]
        public async Task Handle_RequestUnknownShortURL_ThrowsException()
        {
            var unknwnShortURL = "ThisShortURLIsNotKnown";
            await SeedDb(DbOptions, DateTimeService);

            using (var ctx = new MiniURLDbContext(DbOptions, DateTimeService))
            {
                var handler = new GetPersistedURLQueryHandler(ctx);

                await Should.ThrowAsync<NotFoundException>(
                    handler.Handle(new GetPersistedURLQuery { ShortURL = unknwnShortURL },
                                   new System.Threading.CancellationToken()));
            }
        }

        [TestMethod]
        public async Task Handle_RequestKnownShortURL_ReturnsCorrectly()
        {
            var originalURL = "https://github.com/MathiasLorenz";
            var shortURL = "iodfetry";

            using (var ctx = new MiniURLDbContext(DbOptions, DateTimeService))
            {
                ctx.PersistedURLs.Add(new PersistedURL
                {
                    URL = originalURL,
                    ShortURL = shortURL,
                });
                await ctx.SaveChangesAsync();
            }

            using (var ctx = new MiniURLDbContext(DbOptions, DateTimeService))
            {
                var handler = new GetPersistedURLQueryHandler(ctx);
                var query = new GetPersistedURLQuery { ShortURL = shortURL };
                
                var result = await handler.Handle(query, new System.Threading.CancellationToken());

                result.ShouldNotBeNull();
                result.ShouldBeOfType<PersistedURLVm>();
                result.URL.ShouldBe(originalURL);
                result.ShortURL.ShouldBe(shortURL);
            }
        }
    }
}