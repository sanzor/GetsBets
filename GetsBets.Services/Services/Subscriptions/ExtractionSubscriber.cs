using GetsBets.Models;
using GetsBets.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GetsBets.Services
{
    public class ExtractionSubscriber : IExtractionSubscriber<SubscriptionEvent>
    {
        private readonly IConnectionMultiplexer _mux;
        private readonly string _redisSubscriptionChannel;
        public EitherAsync<Error, Unit> SubscribeAsync(Func<SubscriptionEvent, Task> onMessage)
        {
            var subResult = TryAsync(async () =>
            {
                async void Handler(RedisChannel channel, RedisValue value)
                {
                    var subscriptionEvent = JsonSerializer.Deserialize<SubscriptionEvent>(value);
                    var task = onMessage(subscriptionEvent);
                    await task;
                }
                await _mux.GetSubscriber().SubscribeAsync(_redisSubscriptionChannel, Handler);
                return Unit.Default;
            }).ToEither(err =>
            {
                return Error.New($"Could not subscribe to channel with reason {err.Message}");
            });
            return subResult;
        }
        public ExtractionSubscriber(IConnectionMultiplexer mux,IRedisConfiguration redisConfiguration)
        {
            this._mux = mux ?? throw new ArgumentNullException(nameof(mux));
            _redisSubscriptionChannel = redisConfiguration.ExtractionChannel ?? throw new ArgumentNullException(nameof(redisConfiguration.ExtractionChannel));
        }
    }
}
