using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetsBets.DataAccess.Common
{
    public static class DIExtensions
    {
        public static IServiceCollection RegisterDbConfiguration(this IServiceCollection services,IConfiguration configuration)
        {
            var databaseConfiguration=DatabaseConfiguration.GetFromConfiguration(configuration);
            services.AddSingleton<IDatabaseConfiguration>(databaseConfiguration);
            return services;
        }
    }
}
