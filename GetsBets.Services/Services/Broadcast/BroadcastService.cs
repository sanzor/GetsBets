using LanguageExt;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetsBets.Services
{
    internal class BroadcastService<T> : IBroadcastService<T>
    {
        private Serilog.ILogger _logger = Log.ForContext<BroadcastService<T>>();
        private IConnectionMultiplexer _mux;
        public EitherAsync<Error, Unit> PublishMessageAsync<T>(string channel,T message)
        {
            var result= TryAsync(async () =>
            {
                var data = JsonConvert.SerializeObject(message);
                await _mux.GetSubscriber().PublishAsync(channel, data);
                return data;
            }).ToEither(err =>
            {
                _logger.Error($"Failed to publish message with error {err.Message}");
                return err;
            })
            .Map(data =>
            {
                _logger.Information($"Succesfully published message: {data}");
                 return Unit.Default;
            });
            return result;
            
        }
        public BroadcastService(IConnectionMultiplexer mux)
        {
            _mux = mux ?? throw new ArgumentNullException(nameof(mux));
        }
    }
}
