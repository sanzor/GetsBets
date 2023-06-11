using Microsoft.AspNetCore.Http;
using Serilog;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GetsBets.Services
{
    public class SocketWare
    {
        private RequestDelegate next;

        private IExtractionClientFactory factory;
        private ILogger logger = Log.ForContext<SocketWare>();
        public SocketWare(RequestDelegate _next, IExtractionClientFactory clientFactory)
        {
            this.next = _next;
            this.factory = clientFactory;
        }
        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                return;
            }
            using (var socket = await context.WebSockets.AcceptWebSocketAsync())
            {
                var loopingClient = await
                      this.factory.CreateExtractionClient(socket)
                     .ToAsync()
                     .Bind(client => client.RunAsync())
                     .Match(ok =>
                     {
                         logger.Information("Socket client finished normally");
                     }, err =>
                     {
                         logger.Error($"Socket client finished with error: {err.Message}");
                     });
            }
            //var rez=await TryAsync(async () =>
            //{
               
            //}).ToEither(err =>
            //{
            //    logger.Error($"Websocket connection ended with error:{err.Message}");
            //    return err;
            //});
            //;
           
        }
    }
}
