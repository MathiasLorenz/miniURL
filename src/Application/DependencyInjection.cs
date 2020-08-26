using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MiniURL.Application.Common;
using MiniURL.Application.Common.Behaviors;
using MiniURL.Application.Common.Interfaces;
using System.Reflection;

namespace MiniURL.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IRNGCrypto, RNGCrypto>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
