using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetsBets.Services
{
    public class RedisConfiguration:IRedisConfiguration
    {
        private const string RedisConfigurationKey = "redis";
        
        public string ConnectionString { get; set; }
        public string ExtractionChannel { get; set; }
        public static RedisConfiguration GetFromConfiguration(IConfiguration configuration)
        {
            var redisConfig = configuration.GetSection(RedisConfigurationKey).Get<RedisConfiguration>();
            return redisConfig;
        }
    }
}
