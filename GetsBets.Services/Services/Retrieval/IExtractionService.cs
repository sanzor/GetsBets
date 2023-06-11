using GetsBets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetsBets.Services
{
    public interface IExtractionService
    {
        EitherAsync<Error, Unit> TriggerExtractionAsync();
        /// <summary>
        /// Gets top most and least extracted numbers via stored procedure
        /// </summary>
        /// <param name="getExtractionsForDateParams"></param>
        /// <returns></returns>
        EitherAsync<Error, List<AggregatedExtraction>> GetExtractionsForDateAsync(GetExtractionsForDateParams getExtractionsForDateParams);
        EitherAsync<Error, GetTopExtractedNumbersResult> GetTopExtractedNumbersForDateAsync(GetTopExtractedNumbersParams getTopExtractedNumbersForDateParams);
        /// <summary>
        /// Gets top most and least  extracted  numbers in code
        /// </summary>
        /// <param name="getTopExtractedNumbersForDateParams"></param>
        /// <returns></returns>
        EitherAsync<Error, GetTopExtractedNumbersResult> GetTopExtractedNumbersForDateAsync2(GetTopExtractedNumbersParams getTopExtractedNumbersForDateParams);
    }
}
