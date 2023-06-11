using GetsBets.DataAccess.Common;
using GetsBets.DataAccess.Contracts;
using GetsBets.Models;
using LanguageExt;
using LanguageExt.Common;
using LanguageExt.UnitsOfMeasure;
using Microsoft.Extensions.Options;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace GetsBets.DataAccess.Postgres
{

    public class ExtractionRepository : IExtractionRepository
    {
        public readonly IDatabaseConnector databaseConnector;
        private const string INSERT_PROCEDURE = "insert";
        private const string GET_EXTRACTIONS_FOR_DATE_PROCEDURE = "get_extractions_for_date";
        private const string GET_TOP_EXTRACTED_NUMBERS_FOR_DATE = "get_top_extracted_numbers_for_date";
        private const string INSERT_QUERY = @"INSERT INTO public.extraction (data_extragere, ora_extragere, numere, bonus)
          SELECT @date, @time, @numbers, @bonus
          WHERE NOT EXISTS (
              SELECT 1 FROM public.extraction
              WHERE data_extragere = @date
                AND ora_extragere = @time
                AND numere = @numbers
                AND bonus = @bonus";
        private const string SIMPLE_INSERT = "COPY public.extragere (data_extragere,ora_extragere,numere,bonus) FROM STDIN (FORMAT BINARY)";

        public EitherAsync<Error, Unit> InsertExtractionsAsync(List<Extraction> extractions)
        {
            var result = TryAsync(async () =>
            {

                using NpgsqlConnection connection = databaseConnector.GetNpgSqlConnection();
                await connection.OpenAsync();
                using var writer = await connection.BeginBinaryImportAsync(SIMPLE_INSERT);
                //using var command = new NpgsqlCommand(INSERT_PROCEDURE, connection);
                //command.CommandType = CommandType.StoredProcedure;
                foreach (var item in extractions)
                {
                    try
                    {
                        await writer.StartRowAsync();
                        await writer.WriteAsync(item.ExtractionDate, NpgsqlDbType.Date);
                        await writer.WriteAsync(item.ExtractionTime, NpgsqlDbType.Time);
                        await writer.WriteAsync(item.Numbers, NpgsqlDbType.Text);
                        await writer.WriteAsync(item.Bonus, NpgsqlDbType.Text);
                    }
                    catch (PostgresException exc)
                    {
                        if (exc.SqlState == "23505")
                        {
                            continue;
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                await writer.CompleteAsync();
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
            foreach (var extraction in extractions)
            {
                dt.Rows.Add(extraction.ExtractionDate, extraction.ExtractionTime, extraction.Numbers, extraction.Bonus);
            }
            return dt;
        }

        public EitherAsync<Error, IEnumerable<Extraction>> GetExtractionsWithDateFilterAsync(DateOnly date)
        {
            var result = TryAsync(async () =>
            {
            using var connection = databaseConnector.GetNpgSqlConnection();
            await connection.OpenAsync();
            using var command = new NpgsqlCommand("SELECT * FROM public.extragere WHERE data_extragere = @date;", connection);
                command.Parameters.AddWithValue("@date", NpgsqlDbType.Date, date);
                var reader = await command.ExecuteReaderAsync();
                var extractions = await AddExtractionsAsync(reader);
                return extractions;
            }).ToEither();
            return result;
        }
        private async Task<IEnumerable<Extraction>> AddExtractionsAsync(NpgsqlDataReader reader)
        {
            List<Extraction> extractions = new List<Extraction>();
            while (await reader.ReadAsync())
            {
                var extraction = ToExtraction(reader);
                extractions.Add(extraction);
            }
            return extractions;
        }

        private Extraction ToExtraction(NpgsqlDataReader reader)
        {

            var date = (DateTime)reader["data_extragere"];
            var time = (TimeSpan)reader["ora_extragere"];
            var numbers = (string)reader["numere"];
            var bonus = (string)reader["bonus"];
            return new Extraction
            {
                ExtractionDate = new DateOnly(date.Year,date.Month,date.Day),
                ExtractionTime = new TimeOnly(time.Hours,time.Minutes,time.Seconds,time.Milliseconds),
                Numbers = numbers,
                Bonus = bonus
            };
        }
        public EitherAsync<Error, GetTopExtractedNumbersResult> GetTopExtractedNumbersForDateAsync(GetTopExtractedNumbersParams topExtractedParams)
        {
            var result = TryAsync(async () =>
            {
                using var connection = databaseConnector.GetNpgSqlConnection();
                await connection.OpenAsync();
                using var command = new NpgsqlCommand(GET_TOP_EXTRACTED_NUMBERS_FOR_DATE, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@top_count", NpgsqlDbType.Integer, topExtractedParams.TopMostExtractedNumbersCount);
                command.Parameters.AddWithValue("@least_count", NpgsqlDbType.Integer, topExtractedParams.TopLeastExtractedNumbersCount);
                command.Parameters.AddWithValue("@date", NpgsqlDbType.Date, topExtractedParams.Date);

                var reader = await command.ExecuteReaderAsync();


                return new GetTopExtractedNumbersResult
                {

                };
            }).ToEither();
            return result;
        }

        public ExtractionRepository(IDatabaseConnector databaseConnector)
        {
            this.databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));
        }
    }
}
