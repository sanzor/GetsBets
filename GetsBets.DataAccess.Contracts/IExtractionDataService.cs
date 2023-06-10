using GetsBets.Models;
using LanguageExt;
using LanguageExt.Common;

namespace GetsBets.DataAccess.Contracts
{
    public interface IExtractionDataService
    {
        EitherAsync<Error, Unit> InsertExtractionsAsync(List<Extraction> extractions);
        EitherAsync<Error, IEnumerable<Extraction>> GetExtractionsWithDateFilterAsync(DateOnly date);
        EitherAsync<Error, GetTopExtractedNumbersResult> GetTopExtractedNumbersForDateAsync(GetTopExtractedNumbersParams topExtractedNumbersParams);
    }
}
