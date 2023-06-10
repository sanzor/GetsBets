using GetsBets.DataAccess.Contracts;
using GetsBets.Models;
using LanguageExt;
using LanguageExt.Common;

namespace GetsBets
{
    public class ExtractionDataService : IExtractionDataService
    {
      
        public IExtractionRepository _extractionRepository { get; }

        public EitherAsync<Error, Unit> InsertExtractionsAsync(List<Extraction> extractions)
        {
            return this._extractionRepository.InsertExtractionsAsync(extractions);
        }

     
        public EitherAsync<Error, IEnumerable<Extraction>> GetExtractionsWithDateFilterAsync(DateOnly date)
        {
            return this._extractionRepository.GetExtractionsWithDateFilterAsync(date);
        }

        public EitherAsync<Error, GetTopExtractedNumbersResult> GetTopExtractedNumbersForDateAsync(GetTopExtractedNumbersParams topExtractedNumbersParams)
        {
            return this._extractionRepository.GetTopExtractedNumbersForDateAsync(topExtractedNumbersParams);
        }

        public ExtractionDataService(IExtractionRepository extractionRepositoryService)
        {
            _extractionRepository = extractionRepositoryService??throw new ArgumentNullException(nameof(extractionRepositoryService));
        }
    }
}
