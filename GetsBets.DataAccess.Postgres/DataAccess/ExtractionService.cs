using GetsBets.DataAccess.Contracts;
using GetsBets.Models;
using LanguageExt;
using LanguageExt.Common;

namespace GetsBets
{
    public class ExtractionService : IExtractionService
    {
      
        public IExtractionRepository _extractionRepositoryService { get; }

        public EitherAsync<Error, Unit> InsertExtractionsAsync(IEnumerable<Extraction> extractions)
        {
            return this._extractionRepositoryService.InsertExtractionsAsync(extractions);
        }
        public ExtractionService(IExtractionRepository extractionRepositoryService)
        {
            _extractionRepositoryService = extractionRepositoryService;
        }
    }
}
