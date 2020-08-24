using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniURL.Application.Common.Interfaces;
using MiniURL.Infrastructure.Persistence;

namespace MiniURL.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            SeedDb(host);
            
            host.Run();
        }

        private async static void SeedDb(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var ctx = services.GetRequiredService<IMiniURLDbContext>();

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

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
