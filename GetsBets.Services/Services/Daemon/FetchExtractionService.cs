using GetsBets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GetsBets.Services
{
    internal class FetchExtractionService : IFetchExtractionService
    {
        private readonly IHttpClientFactory _httpClientFactory; 
        private const string URL = "https://lottowizz.com/api-data/json-numbers-ro-12/grecia_kino_20_80";
        public EitherAsync<Error, List<RawExtraction>> FetchExtractionsService()
        {
            return TryAsync(async () =>
            {
                using var client = _httpClientFactory.CreateClient("getsbets");
                var resp = await client.SendAsync(new HttpRequestMessage { RequestUri = new Uri(URL), Method = HttpMethod.Get });
                var data = await resp.Content.ReadAsStringAsync();
                var list = JsonSerializer.Deserialize<List<RawExtraction>>(data);
                return list;
            }).ToEither();
        }
        public FetchExtractionService(IHttpClientFactory factory)
        {
            _httpClientFactory = factory;
        }
    }
}
