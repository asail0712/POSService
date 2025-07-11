using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XPlan.Cache;
using XPlan.Database;
using XPlan.Exceptions;

namespace XPlan.Utility
{
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

        public static IServiceCollection InitialMongoDB(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDBSetting"));
            services.AddSingleton<IMongoClient>((sp) =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                return new MongoClient(settings.ConnectionString);
            });

            return services;
        }

        public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services, ILoggerFactory loggerFactory)
        {
            var configExpression    = new MapperConfigurationExpression();
            configExpression.AddMaps(AppDomain.CurrentDomain.GetAssemblies());

            var mapperConfig        = new MapperConfiguration(configExpression, loggerFactory);
            var mapper              = mapperConfig.CreateMapper();

            services.AddSingleton<IMapper>(mapper);
            return services;
        }
    }

}
