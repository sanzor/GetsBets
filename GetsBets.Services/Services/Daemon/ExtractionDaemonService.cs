using GetsBets.DataAccess.Contracts;
using GetsBets.Models;
using Serilog;
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
        private readonly IExtractionDataService _extractionService;
        private readonly ILogger _logger = Log.ForContext<ExtractionDaemonService>();
        private readonly IBroadcastService<ExtractionEvent> _broadcastService;
        private readonly string _extractionChannel;
        public EitherAsync<Error, Unit> InsertWinnersAsync()
        {
            var result = _fetchExtractionService
                 .GetExtractionFromSourceAsync()
                 .Bind(rawExtractions =>
                 {
                     var extractions = _extractionAdapter
                     .ParseExtractions(rawExtractions)
                     .ToAsync();
                     return extractions;
                 })
                 .Bind(ext =>
                 {
                     var date = DateTime.UtcNow;
                     var today = new DateOnly(date.Year, date.Month, date.Day);
                      return _extractionService.GetExtractionsWithDateFilterAsync(today)
                     .Bind(todayExisting =>
                     {
                         var rez= ext.Where(x => !todayExisting.Any(y => y==x)).ToList();
                         if (rez.Count == 0)
                         {
                             return LeftAsync<Error, List<Extraction>>(Error.New("Extraction(s) already exist in te db"));
                         }
                         return RightAsync<Error,List<Extraction>>(rez);
                     });
                 })
                 .Bind(ls => _extractionService.InsertExtractionsAsync(ls).Map(unit => ls))
                 .Bind(ok =>
                 {
                     var @event = new ExtractionEvent
                     {
                         Extractions = ok
                     };
                     return _broadcastService.PublishMessageAsync(_extractionChannel, @event);
                 })
                 .MapLeft(err =>
                 {
                     _logger.Error($"Error in Daemon Service : {err.Message}");
                     return err;
                 });
            return result;


        }

        public ExtractionDaemonService(IFetchExtractionService fetchExtractionService,
            IExtractionAdapter adapter,
            IExtractionDataService extractionService,
            IBroadcastService<ExtractionEvent> broadcastService,
            IRedisConfiguration redisConfiguration)
        {
            this._extractionAdapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
            this._fetchExtractionService = fetchExtractionService ?? throw new ArgumentNullException(nameof(fetchExtractionService));
            this._extractionService = extractionService ?? throw new ArgumentNullException(nameof(extractionService));
            this._broadcastService = broadcastService ?? throw new ArgumentNullException(nameof(broadcastService));
            this._extractionChannel = redisConfiguration.ExtractionChannel;
        }
    }


}
