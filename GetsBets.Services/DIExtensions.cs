using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddSingleton<IExtractionDaemonConfiguration>(daemonConfiguration);
            return services;
        }
    }
}
