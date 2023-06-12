using GetsBets.Services;
using GetsBets.DataAccess.Postgres;
using FluentValidation;
using Serilog;

namespace GetsBets
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddGetsBetsServices(this IServiceCollection services,IConfiguration config)
        {
            services.AddCoreServices(config);
            services.AddDataAccess(config);
            services.AddValidators();
            return services;

        }
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddSingleton<IValidator, GetTopExtractedNumbersParamsValidator>();
            return services;

        }
      
    }
}
