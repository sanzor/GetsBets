using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetsBets.DataAccess.Common
{
    public interface IDatabaseConnector
    {
        NpgsqlConnection GetNpgSqlConnection();
    }
}
