using GetsBets.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GetsBets
{
    public class ConfigCheck : IHealthCheck
    {
        private readonly IConfiguration configuration;

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var config = GetConfiguration();
            foreach (var item in config)
            {
                if(item.Value is null)
                {
                    return Task.FromResult(HealthCheckResult.Unhealthy(item.Key));
                }
            }
            return Task.FromResult(HealthCheckResult.Healthy("config", config));
        }
        public ConfigCheck(IConfiguration configuration)
        {
            this.configuration = configuration??throw new ArgumentNullException(nameof(configuration));
        }
        public IReadOnlyDictionary<string,object> GetConfiguration()
        {
            Dictionary<string, object> result = new Dictionary<string, object>()
            {
                {"db_server_conneection_string", configuration.GetValue<string>("connectionString") },
                {"redis_conneection_string", configuration.GetValue<string>("redis:connectionString") },
                {"redis_channel",configuration.GetValue<string>("redis:extractionChannel") },
                {"extraction_client",configuration.GetValue<ExtractionClientConfiguration>("extractionClient") }
            };
            return result;
        }
    }
}
