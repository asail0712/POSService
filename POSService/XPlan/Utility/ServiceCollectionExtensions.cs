using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using MongoDB.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.Service;
using XPlan.Utility.Caches;
using XPlan.Utility.Databases;
using XPlan.Utility.Exceptions;
using XPlan.Utility.Filter;

namespace XPlan.Utility
{
    public class JwtOptions
    {
        public bool ValidateIssuer { get; set; }            = true;
        public bool ValidateAudience { get; set; }          = true;
        public bool ValidateLifetime { get; set; }          = true;
        public bool ValidateIssuerSigningKey { get; set; }  = true;
        public string Issuer { get; set; }      = "";
        public string Audience { get; set; }    = "";
        public string Secret { get; set; }      = "";
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGlobalExceptionHandling(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            });

            return services;
        }

        public static IServiceCollection AddCacheSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));
            services.AddMemoryCache();
            return services;
        }

        public static IServiceCollection AddJwtPathAuthFilter(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<JwtPathAuthFilter>();
            });

            return services;
        }

        public static IServiceCollection InitialMongoDB(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDBSetting"));
            services.AddSingleton<IMongoClient>((sp) =>
            {
                var settings    = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                var client      = new MongoClient(settings.ConnectionString);                
                return client;
            });

            services.AddSingleton<IMongoDbContext, MongoDbContext>();

            return services;
        }

        public async static Task InitialMongoDBEntity(this IServiceCollection services, string databaseName)
        {
            await DB.InitAsync(databaseName);
        }

        public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services, ILoggerFactory loggerFactory)
        {
            var configExpression = new MapperConfigurationExpression();
            configExpression.AddMaps(AppDomain.CurrentDomain.GetAssemblies());

            var mapperConfig = new MapperConfiguration(configExpression, loggerFactory);
            var mapper = mapperConfig.CreateMapper();

            services.AddSingleton<IMapper>(mapper);
            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, JwtOptions jwtOptions)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer              = jwtOptions.ValidateIssuer,
                            ValidateAudience            = jwtOptions.ValidateAudience,
                            ValidateLifetime            = jwtOptions.ValidateLifetime,
                            ValidateIssuerSigningKey    = jwtOptions.ValidateIssuerSigningKey,
                            ValidIssuer                 = jwtOptions.Issuer,
                            ValidAudience               = jwtOptions.Audience,
                            IssuerSigningKey            = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
                        };
                    });

            services.AddSingleton<JwtOptions>(jwtOptions);

            return services;
        }

        public static IServiceCollection AddJWTSecurity(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // 加入 JWT 認證設定
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "請輸入 JWT Token，格式: Bearer {token}",
                    Name        = "Authorization",
                    In          = ParameterLocation.Header,
                    Type        = SecuritySchemeType.ApiKey,
                    Scheme      = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type    = ReferenceType.SecurityScheme,
                                Id      = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            return services;
        }
    }
}
