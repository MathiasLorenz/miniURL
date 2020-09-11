using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniURL.Application.Common.Exceptions;
using MiniURL.Application.Common.Interfaces;
using MiniURL.Application.PersistedURLs.Commands.Post;
using MiniURL.Domain.Entities;
using MiniURL.Infrastructure.Persistence;
using Moq;
using Shouldly;

namespace MiniURL.Application.IntegrationTests.PersistedURLs.Commands.Post
{
    [TestClass]
    public class CreatePersistedURLCommandTest : BaseTest
    {
        [TestMethod]
        public async Task CreatePersistedURL_InvalidUserId_ThrowsException()
        {
            var mockedShortURL = "";
            var originalURL = "";

            var userId = await SetupUser();
            
            await Should.ThrowAsync<BadRequestException>(
                () => SetupHandlerAndHandle(mockedShortURL, originalURL, userId + 1)
            );
        }

        [TestMethod]
        public async Task CreatePersistedURL_NoUser()
        {
            var mockedShortURL = "xyz";
            var originalURL = "www.averyknownwebsite.com";

            var persistedURLId = await SetupHandlerAndHandle(mockedShortURL, originalURL);

            using (var ctx = new MiniURLDbContext(DbOptions, DateTimeService))
            {
                var persistedURL = await ctx.PersistedURLs.SingleAsync(x => x.ShortURL == mockedShortURL);

                persistedURL.ShortURL.ShouldBe(mockedShortURL);
                persistedURL.URL.ShouldBe(originalURL);
                persistedURL.UserId.ShouldBeNull();
            }
        }

        [TestMethod]
        public async Task CreatePersistedURL_WithUser()
        {
            var mockedShortURL = "abcdefgh";
            var originalURL = "www.somewebsite.com";

            var userId = await SetupUser();
            var persistedURLId = await SetupHandlerAndHandle(mockedShortURL, originalURL, userId);

            using (var ctx = new MiniURLDbContext(DbOptions, DateTimeService))
            {
                var persistedURL = await ctx.PersistedURLs
                    .Include(x => x.User)
                    .SingleAsync(x => x.ShortURL == mockedShortURL);

                persistedURL.ShortURL.ShouldBe(mockedShortURL);
                persistedURL.URL.ShouldBe(originalURL);
                persistedURL.User.ShouldNotBeNull();
                persistedURL.User.ShouldBeOfType<User>();
                persistedURL.UserId.ShouldNotBeNull();
                persistedURL.UserId.ShouldBe(userId);
            }
        }

        private async Task<int> SetupHandlerAndHandle(string shortURL, string originalURL, int? userId = null)
        {
            var mockTokenGenerator = new Mock<ITokenGenerator>();
            mockTokenGenerator.Setup(x => x.GetUniqueKey()).Returns(shortURL);

            using (var ctx = new MiniURLDbContext(DbOptions, DateTimeService))
            {
                var request = new CreatePersistedURLCommand
                {
                    URL = originalURL,
                    UserId = userId ?? null
                };
                var handler = new CreatePersistedURLCommandHandler(ctx, mockTokenGenerator.Object);

                var result = await handler.Handle(request, new System.Threading.CancellationToken());

                result.ShouldNotBeNull();
                result.ShouldBeGreaterThan(0);
                
                return result;
            }
        }

        private async Task<int> SetupUser()
        {
            using (var ctx = new MiniURLDbContext(DbOptions, DateTimeService))
            {
                var entity = new User
                {
                    FirstName = "Første",
                    LastName = "Manden",
                    Email = "sup@sup.com"
                };

                ctx.Users.Add(entity);
                await ctx.SaveChangesAsync();

                return entity.Id;
            }
        }
    }
}