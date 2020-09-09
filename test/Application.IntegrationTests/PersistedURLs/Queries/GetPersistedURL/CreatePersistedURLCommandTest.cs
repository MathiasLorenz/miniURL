using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniURL.Application.Common.Exceptions;
using MiniURL.Application.PersistedURLs.Queries.GetPersistedURL;
using MiniURL.Infrastructure.Persistence;
using Shouldly;

namespace MiniURL.Application.IntegrationTests.PersistedURLs.Queries.GetPersistedURL
{
    [TestClass]
    public class CreatePersistedURLCommandTest : BaseTest
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
            var shortURL = "iodfetry";
            await SeedDb(DbOptions, DateTimeService);

            using (var ctx = new MiniURLDbContext(DbOptions, DateTimeService))
            {
                var handler = new GetPersistedURLQueryHandler(ctx);
                
                var result = await handler.Handle(new GetPersistedURLQuery { ShortURL = shortURL },
                                                  new System.Threading.CancellationToken());

                result.ShouldNotBeNull();
                result.ShouldBeOfType<PersistedURLVm>();
                result.URL.ShouldBe("https://github.com/MathiasLorenz");
                result.ShortURL.ShouldBe(shortURL);
            }
        }
    }
}