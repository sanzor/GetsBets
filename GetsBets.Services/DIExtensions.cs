using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    
namespace GetsBets.Services
{
    public static class DIExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services,IConfiguration configuration)
        {
            var daemonConfiguration = ExtractionDaemonConfiguration.GetFromConfiguration(configuration);
            var redisConfiguration = RedisConfiguration.GetFromConfiguration(configuration);
            services.AddSingleton<IExtractionDaemonConfiguration>(daemonConfiguration);
            services.AddStackExchangeRedis(configuration);
            
            return services;
        }
        private static IServiceCollection AddStackExchangeRedis(this IServiceCollection services,IConfiguration configuration)
        {
            var redisconfiguration = RedisConfiguration.GetFromConfiguration(configuration);
            services.AddSingleton<IRedisConfiguration>(redisconfiguration);
            services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(redisconfiguration.ConnectionString));
            return services;
        }
    }
}
