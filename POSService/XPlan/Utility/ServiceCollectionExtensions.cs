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

using System.Text;
using XPlan.Utility.Caches;
using XPlan.Utility.Databases;
using XPlan.Utility.Exceptions;

namespace XPlan.Utility
{
    /// <summary>
    /// JWT 相關設定
    /// </summary>
    public class JwtOptions
    {
        public bool ValidateIssuer { get; set; }            = true;     // 驗證簽發者
        public bool ValidateAudience { get; set; }          = true;     // 驗證接收者
        public bool ValidateLifetime { get; set; }          = true;     // 驗證有效期限
        public bool ValidateIssuerSigningKey { get; set; }  = true;     // 驗證簽章金鑰
        public string Issuer { get; set; }                  = "";       // 簽發者
        public string Audience { get; set; }                = "";       // 接收者
        public string Secret { get; set; }                  = "";       // 秘密金鑰
    }

    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 加入全域例外過濾器
        /// </summary>
        public static IServiceCollection AddExceptionHandling<T>(this IServiceCollection services) where T : GlobalExceptionFilter
        {
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add<T>();
            });

            return services;
        }

        /// <summary>
        /// 加入快取設定及記憶體快取
        /// </summary>
        public static IServiceCollection AddCacheSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));
            services.AddMemoryCache();
            return services;
        }

        /// <summary>
        /// 初始化 MongoDB 連線與註冊相關服務
        /// </summary>
        public static IServiceCollection InitialMongoDB(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDBSettings>(configuration.GetSection("MongoDBSetting"));

            services.AddSingleton<IMongoClient>((sp) =>
            {
                var settings    = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
                var client      = new MongoClient(settings.ConnectionString);
                return client;
            });

            services.AddSingleton<IMongoDbContext, MongoDBContext>();

            return services;
        }

        /// <summary>
        /// 初始化 MongoDB.Entities 的 DB 連線
        /// </summary>
        public async static Task InitialMongoDBEntity(this IServiceCollection services, IConfiguration configuration)
        {
            var section                 = configuration.GetSection("MongoDBSetting");
            MongoDBSettings? dbSetting  = section.Get<MongoDBSettings>();

            if (dbSetting is null)
            {
                throw new InvalidOperationException("Missing or invalid MongoDBSetting section in configuration.");
            }

            await DB.InitAsync(dbSetting.DatabaseName, MongoClientSettings.FromConnectionString(dbSetting.ConnectionString));
        }

        /// <summary>
        /// 註冊 AutoMapper 映射設定並加入 DI
        /// </summary>
        public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services, ILoggerFactory loggerFactory)
        {
            var configExpression    = new MapperConfigurationExpression();
            configExpression.AddMaps(AppDomain.CurrentDomain.GetAssemblies());

            var mapperConfig        = new MapperConfiguration(configExpression, loggerFactory);
            var mapper              = mapperConfig.CreateMapper();

            services.AddSingleton<IMapper>(mapper);
            return services;
        }

        /// <summary>
        /// 設定 JWT 認證服務
        /// </summary>
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

        /// <summary>
        /// 在 Swagger 中加入 JWT 安全定義
        /// </summary>
        public static IServiceCollection AddJWTSecurity(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // 加入 JWT 認證設定
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description     = "請輸入 JWT Token，格式: Bearer {token}",
                    Name            = "Authorization",
                    In              = ParameterLocation.Header,
                    Type            = SecuritySchemeType.ApiKey,
                    Scheme          = "Bearer",
                    BearerFormat    = "JWT"
                });
            });
            return services;
        }
    }
}
