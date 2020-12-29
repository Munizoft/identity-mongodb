using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Munizoft.Identity.Infrastructure.Services;
using Munizoft.Identity.Persistence.MongoDB;
using System;

namespace Munizoft.Identity.MongoDB.Extensions
{
    public static class IdentityExtensions
    {
        public static void InitIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var mongoDbIdentityConfiguration = new MongoDbIdentityConfiguration
            {
                MongoDbSettings = new MongoDbSettings
                {
                    ConnectionString = "mongodb+srv://admin:dWlVBTLDBfZVY6Rs@cluster0.b0chh.mongodb.net",
                    DatabaseName = "Identity"
                },

                IdentityOptionsAction = options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;

                    // Lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                    options.Lockout.MaxFailedAccessAttempts = 10;

                    // ApplicationUser settings
                    options.User.RequireUniqueEmail = true;
                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@.-_";
                }
            };

            services.ConfigureMongoDbIdentity<Munizoft.Identity.Entities.User, Munizoft.Identity.Entities.Role, String>(mongoDbIdentityConfiguration);

            services.AddOptions<Infrastructure.Models.IdentityOptions>();

            services.AddSingleton<IdentityContext>(x => new IdentityContext("mongodb+srv://admin:dWlVBTLDBfZVY6Rs@cluster0.b0chh.mongodb.net", "Identity"));

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
        }
    }
}
