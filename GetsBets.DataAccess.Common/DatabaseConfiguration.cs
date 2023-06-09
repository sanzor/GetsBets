using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetsBets.DataAccess.Common
{
    public class DatabaseConfiguration:IDatabaseConfiguration
    {
        private const string ConnectionStringKey = "connectionString";

        public string ConnectionString { get; set; }
        public static DatabaseConfiguration GetFromConfiguration(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection(ConnectionStringKey).Get<string>();
            return new DatabaseConfiguration { ConnectionString = connectionString };
        }
    }
}
