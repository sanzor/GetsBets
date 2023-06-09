using GetsBets.DataAccess.Contracts;
using GetsBets.Models;
using LanguageExt;
using LanguageExt.Common;

namespace GetsBets
{
    public class ExtractionService : IExtractionService
    {
      
        public IExtractionRepository _extractionRepositoryService { get; }

        public EitherAsync<Error, Unit> InsertExtractionAsync(Extraction extraction)
        {
            return this._extractionRepositoryService.InsertExtractionAsync(extraction);
        }
        public ExtractionService(IExtractionRepository extractionRepositoryService)
        {
            _extractionRepositoryService = extractionRepositoryService;
        }
    }
}
