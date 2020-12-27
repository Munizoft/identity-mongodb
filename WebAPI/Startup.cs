using AspNetCore.Identity.Mongo;
using AspNetCore.Identity.Mongo.Model;
using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MongoDbGenericRepository;
using Munizoft.Identity.Entities;
using Munizoft.Identity.Infrastructure.Models;
using Munizoft.Identity.Infrastructure.Services;
using Munizoft.Identity.Persistence.MongoDB;
using Newtonsoft.Json.Serialization;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Munizoft.Identity.MongoDB
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //var mongoDbContext = new IdentityContext("mongodb+srv://admin:dWlVBTLDBfZVY6Rs@cluster0.b0chh.mongodb.net", "Identity");
            //services.AddIdentity<User, Role>()
            //    .AddMongoDbStores<User, Role, String>(mongoDbContext)
            //    .AddDefaultTokenProviders();

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

            services.ConfigureMongoDbIdentity<User, Role, String>(mongoDbIdentityConfiguration);

            services.AddOptions<Munizoft.Identity.Infrastructure.Models.IdentityOptions>();

            services.Configure<Munizoft.Identity.Infrastructure.Models.JwtOptions>(options =>
            {
                options.Key = Configuration["Jwt:Key"];
                options.Issuer = Configuration["Jwt:Issuer"];
                options.ExpireDays = Int32.Parse(Configuration["Jwt:ExpireDays"]);
            });
            services.AddOptions<PasswordHasherOptions>();

            var key = Encoding.ASCII.GetBytes(Configuration["Jwt:Key"]);

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            services.AddAuthorization(options =>
            {
                //options.AddPolicy("RequireSuperAdminRole", policy => policy.RequireRole("Super Admin"));
                //options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                //options.AddPolicy("RequireAdminsRole", policy => policy.RequireRole("Super Admin", "Admin"));
                //options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
                //options.AddPolicy("RequireAnyRole", policy => policy.RequireRole("Super Admin", "Admin", "User"));
            });

            services.AddSingleton<IdentityContext>(x => new IdentityContext("mongodb+srv://admin:dWlVBTLDBfZVY6Rs@cluster0.b0chh.mongodb.net", "Identity"));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddAutoMapper(typeof(UserService));

            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Munizoft - Identity MongoDB", Version = "v1" });
            //});

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
