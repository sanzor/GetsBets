using GetsBets.DataAccess.Common;
using GetsBets.DataAccess.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetsBets.DataAccess.Postgres
{
    public static class DIExtensions
    {
        public static IServiceCollection AddDataAccess(IServiceCollection services,IConfiguration configuration)
        {
            services.RegisterDbConfiguration(configuration);
            services.AddSingleton<IExtractionService, ExtractionService>();
            services.AddSingleton<IExtractionRepository, ExtractionRepository>();
            return services;
        }
    }
}
