using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace GetsBets.Services
{
    internal class ExtractionClientFactory : IExtractionClientFactory
    {
        private readonly IConnectionMultiplexer _mux;
        private readonly IRedisConfiguration redisConfiguration;
        private readonly IExtractionClientConfiguration _extractionClientConfiguration;
        private readonly INumberPrepareService _numberPrepareService;
        public ExtractionClientFactory(
            IConnectionMultiplexer mux,
            IRedisConfiguration redisConfiguration,
            IExtractionClientConfiguration extractionClientConfiguration,
            INumberPrepareService numberPrepareService)
        {
            this._mux = mux ?? throw new ArgumentNullException(nameof(mux));
            this.redisConfiguration = redisConfiguration ?? throw new ArgumentNullException(nameof(redisConfiguration));
            this._extractionClientConfiguration = extractionClientConfiguration ?? throw new ArgumentNullException(nameof(extractionClientConfiguration));
            this._numberPrepareService = numberPrepareService ?? throw new ArgumentNullException(nameof(numberPrepareService));
        }

        public Either<Error, IExtractionClient> CreateExtractionClient(WebSocket websocket)
        {
            var client = Try(() =>
            {
                IExtractionClient client = new ExtractionClient(
                    websocket,
                    _mux.GetSubscriber(),
                    redisConfiguration,
                    _numberPrepareService,
                    _extractionClientConfiguration
                   );
                return client;
            }).ToEither(exc => Error.New(exc));
            return client;
        }

    }
}
