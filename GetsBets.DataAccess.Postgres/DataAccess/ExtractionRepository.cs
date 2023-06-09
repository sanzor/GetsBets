using GetsBets.DataAccess.Common;
using GetsBets.DataAccess.Contracts;
using GetsBets.Models;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;

namespace GetsBets.DataAccess.Postgres
{
    
    public class ExtractionRepository : IExtractionRepository
    {
        public readonly IDatabaseConnector databaseConnector;
        private const string INSERT_PROCEDURE = "insert";

        public EitherAsync<Error, Unit> InsertExtractionsAsync(List<Extraction> extractions)
        {
            var result=TryAsync(async () =>
            {
                using var connection = databaseConnector.GetNpgSqlConnection();
                using var command = new NpgsqlCommand(INSERT_PROCEDURE, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                var table = CreateDataTable(extractions);
                var parameter = new NpgsqlParameter
                {
                    ParameterName = "@extractions",
                    NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Array,
                    Value = table
                };
                
               
                await command.ExecuteNonQueryAsync();
                return Unit.Default;
            }).ToEither();
            return result;
        }
        private DataTable CreateDataTable(IEnumerable<Extraction> extractions)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("data_extragere", typeof(DateOnly));
            dt.Columns.Add("ora_extragere", typeof(TimeOnly));
            dt.Columns.Add("numere", typeof(string));
            dt.Columns.Add("bonus", typeof(string));
            foreach(var extraction in extractions)
            {
                dt.Rows.Add(extraction.ExtractionDate, extraction.ExtractionTime, extraction.Numbers, extraction.Bonus);
            }
            return dt;
        }
        public ExtractionRepository(IDatabaseConnector databaseConnector)
        {
            this.databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));
        }
    }
}
