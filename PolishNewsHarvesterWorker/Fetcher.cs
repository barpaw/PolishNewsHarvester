using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PolishNewsHarvesterSdk.Dto;
using PolishNewsHarvesterSdk.Http;

namespace PolishNewsHarvesterWorker
{
    public class Fetcher : IFetcher
    {
        private readonly ILogger<Fetcher> _logger;
        private IConfiguration _configuration;
        private IHttpManager _httpManager;

        public Fetcher(ILogger<Fetcher> logger, IConfiguration configuration, IHttpManager httpManager)
        {
            _logger = logger;
            _configuration = configuration;
            _httpManager = httpManager;
        }


        public async Task<HttpResponseDto> FetchUrl(string url)
        {
            var defaultHttpResponseDto = new HttpResponseDto(url, string.Empty, -1, string.Empty);

            try
            {
                var httpResponseMessage = await _httpManager.SendGetRequestAsync(url);

                var body = await httpResponseMessage.Content.ReadAsStringAsync();
                var statusCode = (int) httpResponseMessage.StatusCode;
                
                // see https://stackoverflow.com/a/41836921
                var responseHeaders = Enumerable
                    .Empty<(String name, String value)>()
                    // Add the main Response headers as a flat list of value-tuples with potentially duplicate `name` values:
                    .Concat(
                        httpResponseMessage.Headers
                            .SelectMany(kvp => kvp.Value
                                .Select(v => (name: kvp.Key, value: v))
                            )
                    )
                    // Concat with the content-specific headers as a flat list of value-tuples with potentially duplicate `name` values:
                    .Concat(
                        httpResponseMessage.Content.Headers
                            .SelectMany(kvp => kvp.Value
                                .Select(v => (name: kvp.Key, value: v))
                            )
                    )
                    // Render to a string:
                    .Aggregate(
                        seed: new StringBuilder(),
                        func: (sb, pair) => sb.Append(pair.name).Append(": ").Append(pair.value).AppendLine(),
                        resultSelector: sb => sb.ToString()
                    );


                defaultHttpResponseDto = new HttpResponseDto(url, body, statusCode, responseHeaders);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{workerName}: Exception caught.", _configuration["app:workerName"]);
            }
            
            return defaultHttpResponseDto;
        }
    }
}