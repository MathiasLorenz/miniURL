using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MiniURL.Application.Common.Interfaces;
using MiniURL.Infrastructure.Persistence;

namespace MiniURL.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await SeedDb(host);
            
            await host.RunAsync();
        }

        private async static Task SeedDb(IHost host)
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
                .ConfigureLogging(logging => 
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
