using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniURL.Application.Common.Interfaces;
using MiniURL.Infrastructure.Persistence;
using MiniURL.Infrastructure.Services;

namespace MiniURL.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<MiniURLDbContext>(options =>
                    options.UseInMemoryDatabase("MiniURLInMemoryDb"));
            }
            else
            {
                services.AddDbContext<MiniURLDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(MiniURLDbContext).Assembly.FullName)));
            }

            services.AddScoped<IMiniURLDbContext>(provider => provider.GetService<MiniURLDbContext>());

            services.AddTransient<IDateTime, DateTimeService>();

            return services;
        }
    }
}
