using Microsoft.Extensions.Hosting;
using Serilog;

namespace GetsBets.Services
{
    public class ExtractionDaemon : IHostedService
    {
        private readonly IExtractionDaemonService _dailyWinnersDaemonService;
        private readonly IExtractionDaemonConfiguration _configuration;
        private ILogger _logger = Log.ForContext<ExtractionDaemon>();
        private Timer _timer;
        public Task StartAsync(CancellationToken cancellationToken)
        {

            var interval = GetRunInterval(_configuration)
                .Map(ok =>
                {
                    _timer = new Timer(async (time) => await _dailyWinnersDaemonService.InsertWinnersAsync(), null, TimeSpan.Zero, ok);
                    return ok;
                })
                .MapLeft(err =>
                {
                    _logger.Error($"Failed to launch daemon with error {err.Message}");
                    return err;
                });
            return Task.CompletedTask;
            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        public ExtractionDaemon(IExtractionDaemonConfiguration configuration, IExtractionDaemonService dailyWinnersDaemonService)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _dailyWinnersDaemonService = dailyWinnersDaemonService ?? throw new ArgumentNullException(nameof(dailyWinnersDaemonService));
        }
        private Either<Error,TimeSpan> GetRunInterval(IExtractionDaemonConfiguration configuration)
        {

            var span=TimeSpan.FromMinutes((double)configuration.RunExtractionAfterMinutes);
            return span;
        }
    }
}
