﻿using GetsBets.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace GetsBets.Services
{
    internal class ExtractionClient : IExtractionClient
    {
        private readonly ISubscriber _subscriber;
        private readonly INumberPrepareService _numberPrepareService;
        private readonly IExtractionClientConfiguration _clientConfiguration;
        private readonly BlockingCollection<string> _outboundQueue = new BlockingCollection<string>();
        private readonly ConcurrentQueue<string> _numberQueue = new ConcurrentQueue<string>();
        private readonly WebSocket _webSocket;


        private readonly string RedisChannel;
        public EitherAsync<Error, Unit> RunAsync()
        {
            var loop = TryAsync(async () =>
            {
                await _subscriber.SubscribeAsync(this.RedisChannel, OnRedisMessageHandler);
                foreach (var item in _outboundQueue.GetConsumingEnumerable())
                {
                    ExtractionEvent extractionEvent = JsonConvert.DeserializeObject<ExtractionEvent>(item);
                    Task.Run(() => ProcessMessageAsync(extractionEvent));
                }

                return Unit.Default;
            }).ToEither();
            return loop;

        }
        private EitherAsync<Error, Unit> ProcessMessageAsync(ExtractionEvent @event)
        {
            return _numberPrepareService.PrepareRandomizedNumbers(@event.Extraction.Numbers)
                .ToAsync()
                .Bind(RunSendNumbersLoop);
        }
        private EitherAsync<Error, Unit> RunSendNumbersLoop(List<SendNumber> numbers)
        {
            var loopResult = TryAsync(async () =>
            {
                Queue<SendNumber> queue = new Queue<SendNumber>(numbers);
                DateTime previous, now;
                now = previous = DateTime.UtcNow;

                while (true)
                {
                    if (queue.Count == 0)
                    {
                        return Unit.Default;
                    }
                    if (now - previous > TimeSpan.FromSeconds(_clientConfiguration.SendNumberAfterSeconds))
                    {
                        var number = queue.Dequeue();
                        var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(number));
                        await _webSocket.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    await Task.Delay(_clientConfiguration.ThrottleMilliseconds);
                }

            })
                .ToEither()
                .MapAsync(async ok =>
                {
                    await CleanupSessionAsync();
                    return ok;
                });
            return loopResult;

        }

        public ExtractionClient(WebSocket websocket,
            ISubscriber subscriber,
            IRedisConfiguration config,
            INumberPrepareService numberPrepareService,
            IExtractionClientConfiguration _clientConfiguration)
        {
            _webSocket = websocket ?? throw new ArgumentNullException(nameof(websocket));
            this._subscriber = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            this._numberPrepareService = numberPrepareService ?? throw new ArgumentNullException(nameof(numberPrepareService));
            this.RedisChannel = config.ExtractionChannel ?? throw new ArgumentNullException(nameof(config.ExtractionChannel));
            _clientConfiguration = _clientConfiguration ??
                throw new ArgumentNullException(nameof(_clientConfiguration));
        }
        private Action<RedisChannel, RedisValue> onRedisMessageHandler = null;
        public Action<RedisChannel, RedisValue> OnRedisMessageHandler
        {
            get
            {
                if (this.onRedisMessageHandler == null)
                {
                    this.onRedisMessageHandler = new Action<RedisChannel, RedisValue>
                                                ((channel, value) => this._outboundQueue.Add(value));
                }
                return this.onRedisMessageHandler;
            }
        }
        private async Task CleanupSessionAsync()
        {
            await this._subscriber.UnsubscribeAsync(RedisChannel, this.OnRedisMessageHandler);
        }

    }
}