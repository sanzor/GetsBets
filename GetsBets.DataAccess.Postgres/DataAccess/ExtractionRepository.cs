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
        private const string GET_EXTRACTIONS_FOR_DATE_PROCEDURE = "get_extractions_for_date";

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

        public EitherAsync<Error, IEnumerable<Extraction>> GetExtractionsForDateAsync(DateTime dateTime)
        {
            var result = TryAsync(async () =>
            {
                using var connection = databaseConnector.GetNpgSqlConnection();
                using var command = new NpgsqlCommand(GET_EXTRACTIONS_FOR_DATE_PROCEDURE, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
              
                var parameter = new NpgsqlParameter
                {
                    ParameterName = "@filter",
                    NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date,
                    Value = dateTime
                };


                var reader=await command.ExecuteReaderAsync();
                var extractions = await AddExtractionsAsync(reader);
                return extractions;
            }).ToEither();
            return result;
        }
        private async Task<IEnumerable<Extraction>> AddExtractionsAsync(NpgsqlDataReader reader)
        {
            List<Extraction> extractions=new List<Extraction>();
            while (await reader.ReadAsync())
            {
                var extraction =  ToExtraction(reader);
                extractions.Add(extraction);
            }
            return extractions;
        }
        private Extraction ToExtraction(NpgsqlDataReader reader)
        {
            var date = (DateOnly)reader["data_extragere"];
            var time = (TimeOnly)reader["ora_extragere"];
            var numbers = (string)reader["numere"];
            var bonus = (string)reader["bonus"];
            return new Extraction
            {
                ExtractionDate = date,
                ExtractionTime = time,
                Numbers = numbers,
                Bonus = bonus
            };
        }
        public ExtractionRepository(IDatabaseConnector databaseConnector)
        {
            this.databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));
        }
    }
}
