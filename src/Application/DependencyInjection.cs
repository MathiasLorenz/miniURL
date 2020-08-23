using Microsoft.Extensions.DependencyInjection;
using MiniURL.Application.URL;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

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
