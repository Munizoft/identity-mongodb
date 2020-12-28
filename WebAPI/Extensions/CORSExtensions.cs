using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Munizoft.Identity.MongoDB.Extensions
{
    public static class CORSExtensions
    {
        #region Fields
        readonly static String AllowAllOrigins = "AllowAllOrigins";
        readonly static String AllowSpecificOrigins = "AllowSpecificOrigins";
        #endregion Fields

        public static void InitCORS(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var allowedHosts = configuration["AllowedHosts"].Split(';');

            if (allowedHosts.Any())
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(AllowSpecificOrigins,
                       builder => builder
                                    .WithOrigins(allowedHosts)
                                    .AllowAnyHeader()
                                    .AllowAnyMethod()
                                );
                });
            }
            else
            {
                services.AddCors();
            }
        }

        public static void EnableCORS(this IApplicationBuilder app, IConfiguration configuration)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var allowedHosts = configuration["AllowedHosts"].Split(';');

            if (allowedHosts.Any())
            {
                app.UseCors(AllowSpecificOrigins);
            }
            else
            {
                app.UseCors();
            }
        }
    }
}