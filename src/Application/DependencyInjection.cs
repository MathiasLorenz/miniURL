using Microsoft.Extensions.DependencyInjection;
using MiniURL.Application.Common;
using MiniURL.Application.Common.Interfaces;

namespace MiniURL.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IRNGCrypto, RNGCrypto>();

            return services;
        }
    }
}
