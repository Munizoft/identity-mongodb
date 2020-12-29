using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Munizoft.Identity.Infrastructure.Models;
using Munizoft.Identity.Infrastructure.Services;
using Munizoft.Identity.MongoDB.Extensions;
using Newtonsoft.Json.Serialization;
using Sole.Services;
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
            services.InitIdentity(Configuration);

            services.InitCORS(Configuration);

            services.Configure<JwtOptions>(options =>
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

            services.InitSole(Configuration);

            services.AddAutoMapper(typeof(UserService), typeof(ClientService));

            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });

            services
                .AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Sole - Identity", Version = "v1" });
                });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.EnableCORS(Configuration);

            app.UseSwagger();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            if (env.IsDevelopment() || env.IsStaging() || env.IsProduction())
            {
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = "";
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sole - Identity");
                });
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
