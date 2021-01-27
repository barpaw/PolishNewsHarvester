using Microsoft.Extensions.Logging;
using PolishNewsHarvesterSdk.Consts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PolishNewsHarvesterSdk.Http
{
    public class HttpManager : IHttpManager
    {

        private readonly ILogger<HttpManager> _logger;
        private IHttpClientFactory _httpClientFactory;

        public HttpManager(ILogger<HttpManager> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> SendGetRequestAsync(string url, string httpClientType)
        {
            try
            {

                using (var httpClient = _httpClientFactory.CreateClient(httpClientType))
                {


                    return await httpClient.GetAsync(url);

                }
            }
            catch
            {
                throw;
            }

        }


    }
}
