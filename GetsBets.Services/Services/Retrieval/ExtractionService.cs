using GetsBets.DataAccess.Contracts;
using GetsBets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetsBets.Services
{
    public class ExtractionService : IExtractionService
    {
        private readonly IExtractionAdapter _extractionAdapter;
        private readonly IExtractionDataService extractionService;
        private readonly ITopExtractedNumbersService _topExtractedNumbersService;
        private readonly IExtractionDaemonService _extractionDaemonService;

        public EitherAsync<Error, List<AggregatedExtraction>> GetExtractionsForDateAsync(GetExtractionsForDateParams getExtractionParams)
        {
            var rez = extractionService
                .GetExtractionsWithDateFilterAsync(getExtractionParams.DateFilter)
                .Bind(ls =>
                {
                    var list = _extractionAdapter.ParseAggregatedExtractions(ls.ToList()).ToAsync();
                    return list;
                });
            return rez;

        }
        public EitherAsync<Error, GetTopExtractedNumbersResult> GetTopExtractedNumbersForDateAsync(GetTopExtractedNumbersParams topMostExtractedNumbersParams)
        {
            var rez = extractionService
                .GetTopExtractedNumbersForDateAsync(topMostExtractedNumbersParams);
            return rez;
        }
        public EitherAsync<Error, GetTopExtractedNumbersResult> GetTopExtractedNumbersForDateAsync2(GetTopExtractedNumbersParams topExtractedParams)
        {
            var rez = extractionService
                .GetExtractionsWithDateFilterAsync(topExtractedParams.Date)
                .Bind(extractions =>
                {
                    return _topExtractedNumbersService.CalculateTopExtractedNumbers(new CalculateTopExtractedNumbersParams
                    {
                        Extractions = extractions,
                        TopLeastExtractedNumbersCount = topExtractedParams.TopLeastExtractedNumbersCount,
                        TopMostExtractedNumbersCount = topExtractedParams.TopMostExtractedNumbersCount
                    })
                    .ToAsync();
                });
            return rez;

        }

        public EitherAsync<Error, Unit> TriggerExtractionAsync()
        {
            return _extractionDaemonService.InsertWinnersAsync();
        }

        public ExtractionService(
            IExtractionAdapter extractionAdapter,
            IExtractionDataService extractionService,
            ITopExtractedNumbersService topExtractedNumbersService,
            IExtractionDaemonService daemonService)
        {
            _extractionDaemonService = daemonService ?? throw new ArgumentNullException(nameof(daemonService));
            this._extractionAdapter = extractionAdapter ?? throw new ArgumentNullException(nameof(extractionAdapter));
            this.extractionService = extractionService ?? throw new ArgumentNullException(nameof(extractionService));
            _topExtractedNumbersService = topExtractedNumbersService ?? throw new ArgumentNullException(nameof(topExtractedNumbersService));
        }




    }
}
