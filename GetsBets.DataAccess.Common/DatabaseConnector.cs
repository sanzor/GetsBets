using Npgsql;

namespace GetsBets.DataAccess.Common
{
    public class DatabaseConnector : IDatabaseConnector
    {
        private readonly IDatabaseConfiguration databaseConfiguration;
        public NpgsqlConnection GetNpgSqlConnection()
        {
            return new NpgsqlConnection(databaseConfiguration.ConnectionString);
        }
        public DatabaseConnector(IDatabaseConfiguration configuration)
        {
            databaseConfiguration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
    }
}