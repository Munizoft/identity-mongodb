using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sole.Persistence.MongoDB;
using Sole.Services;
using System;

namespace Munizoft.Identity.MongoDB.Extensions
{
    public static class SoleExtensions
    {
        public static void InitSole(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.AddSingleton<SoleContext>(x => new SoleContext(configuration["DBConntectionString:SoleConnection"], configuration["DBConntectionString:SoleDatabase"]));

            services.AddScoped<IClientService, ClientService>();
        }
    }
}
