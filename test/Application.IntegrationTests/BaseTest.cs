using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniURL.Application.Common.Interfaces;
using MiniURL.Domain.Entities;
using MiniURL.Infrastructure.Persistence;
using Moq;

namespace MiniURL.Application.IntegrationTests
{
    public class BaseTest
    {
        public DbContextOptions<MiniURLDbContext> DbOptions { get; set; } = new DbContextOptions<MiniURLDbContext>();

        public IDateTime DateTimeService { get; set; }

        public BaseTest()
        {
            DbOptions = new DbContextOptionsBuilder<MiniURLDbContext>()
                .UseInMemoryDatabase("MiniURLInMemoryDbForTesting")
                .Options;

            var dateTimeService = new Mock<IDateTime>();
            dateTimeService.Setup(x => x.Now).Returns(new DateTime(2020, 02, 15));
            DateTimeService = dateTimeService.Object;
        }

        public static async Task SeedDb(DbContextOptions<MiniURLDbContext> dbOptions, IDateTime dateTime)
        {
            using (var ctx = new MiniURLDbContext(dbOptions, dateTime))
            {
                ctx.Database.EnsureDeleted();

                var seeder = new MiniURLDbContextSeeder(ctx);

                try
                {
                    await seeder.SeedAllAsync(new System.Threading.CancellationToken());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task<int> CreateAUser(DbContextOptions<MiniURLDbContext> dbOptions, IDateTime dateTime)
        {
            using (var ctx = new MiniURLDbContext(dbOptions, dateTime))
            {
                ctx.Database.EnsureDeleted();

                var entity = SeedData.Users()[0];
                ctx.Users.Add(entity);
                await ctx.SaveChangesAsync();

                return entity.Id;
            }
        }

        protected static void ResetDatabase(DbContextOptions<MiniURLDbContext> dbOptions, IDateTime dateTime)
        {
            using (var ctx = new MiniURLDbContext(dbOptions, dateTime))
            {
                ctx.Database.EnsureDeleted();
            }
        }
    }
}