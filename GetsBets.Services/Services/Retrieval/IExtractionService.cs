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
        EitherAsync<Error, List<AggregatedExtraction>> GetExtractionsForDateAsync(GetExtractionsForDateParams getExtractionsForDateParams);
        EitherAsync<Error, GetTopExtractedNumbersResult> GetTopExtractedNumbersForDateAsync(GetTopExtractedNumbersParams getTopExtractedNumbersForDateParams);
    }
}
