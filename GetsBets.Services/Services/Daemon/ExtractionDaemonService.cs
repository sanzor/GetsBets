using GetsBets.DataAccess.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetsBets.Services
{
    public class ExtractionDaemonService : IExtractionDaemonService
    {
        private readonly IFetchExtractionService _fetchExtractionService;
        private readonly IExtractionAdapter _extractionAdapter;
        private readonly IExtractionService _extractionService;
        public EitherAsync<Error, Unit> InsertWinnersAsync()
        {
            var result= _fetchExtractionService
                 .FetchExtractionsService()
                 .Bind(rawExtractions =>
                 {
                     var extractions = _extractionAdapter
                     .ParseExtractions(rawExtractions)
                     .ToAsync();
                     return extractions;
                 })
                 .Bind(_extractionService.InsertExtractionsAsync)
                 .Map(ok=>Unit.Default);
            return result;
                
               
        }
        public ExtractionDaemonService(IFetchExtractionService fetchExtractionService)
        {
            _fetchExtractionService = fetchExtractionService;
        }
        public ExtractionDaemonService(IFetchExtractionService fetchExtractionService,
            IExtractionAdapter adapter,
            IExtractionService extractionService)
        {
            this._extractionAdapter = adapter;
            this._fetchExtractionService = fetchExtractionService;
            this._extractionService = extractionService;
        }
    }

    
}
