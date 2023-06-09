using GetsBets.DataAccess.Common;
using GetsBets.DataAccess.Contracts;
using GetsBets.Models;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.Extensions.Options;
using Npgsql;

namespace GetsBets.DataAccess.Postgres
{
    
    public class ExtractionRepository : IExtractionRepository
    {
        public readonly IDatabaseConnector databaseConnector;
        private const string INSERT_PROCEDURE = "insert";

        public EitherAsync<Error, Unit> InsertExtractionAsync(Extraction extraction)
        {
            var result=TryAsync(async () =>
            {
                using var connection = databaseConnector.GetNpgSqlConnection();
                using var command = new NpgsqlCommand(INSERT_PROCEDURE, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("ExtractionDate", NpgsqlTypes.NpgsqlDbType.Date, extraction.ExtractionDate);
                command.Parameters.AddWithValue("ExtractionTime", NpgsqlTypes.NpgsqlDbType.Time, extraction.ExtractionTime);
                command.Parameters.AddWithValue("Numbers", NpgsqlTypes.NpgsqlDbType.Text, extraction.Numbers);
                command.Parameters.AddWithValue("Bonus", NpgsqlTypes.NpgsqlDbType.Text, extraction.Bonus);
                await command.ExecuteNonQueryAsync();
                return Unit.Default;
            }).ToEither();
            return result;
        }
        public ExtractionRepository(IDatabaseConnector databaseConnector)
        {
            this.databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));
        }
    }
}
