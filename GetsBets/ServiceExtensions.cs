using GetsBets.Services;
using GetsBets.DataAccess.Postgres;

namespace GetsBets
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddGetsBetsServices(this IServiceCollection services,IConfiguration config)
        {
            services.AddCoreServices(config);
            services.AddDataAccess(config);
            return services;

        }
    }
}
