using Microsoft.Extensions.Logging;
using PolishNewsHarvesterSdk.Domain;
using PolishNewsHarvesterSdk.Dto;
using PolishNewsHarvesterSdk.Enums;
using PolishNewsHarvesterSdk.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolishNewsHarvesterSdk.Methods
{
    public class Methods : IMethods
    {

        private readonly ILogger<Methods> _logger;
        private IHttpManager _httpManager;

        public Methods(ILogger<Methods> logger, IHttpManager httpManager)
        {
            _logger = logger;
            _httpManager = httpManager;
        }

        public MethodInvocationResult SendGetRequestAsync(string url)
        {
            List<MethodParameter> methodParameters = new List<MethodParameter>();
            methodParameters.Add(new MethodParameter(url, url.GetType()));


            var defaultHttpResponseDto = new HttpResponseDto(url, string.Empty, -1, string.Empty);

            try
            {
                var httpResponseMessage = _httpManager.SendGetRequestAsync(url).Result;

                var body = httpResponseMessage.Content.ReadAsStringAsync().Result;
                var statusCode = (int)httpResponseMessage.StatusCode;

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
                _logger.LogError(e, "{workerName}: Exception caught.", "SDK");
            }

            Console.WriteLine($"{defaultHttpResponseDto.Url}: {defaultHttpResponseDto.StatusCode}");

            Console.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod().Name} {url} {url.GetType()} {url}");
            return new MethodInvocationResult(System.Reflection.MethodBase.GetCurrentMethod().Name, methodParameters, "test", "test".GetType(), null, MethodInvocationState.MethodInvoked);
        }

        public MethodInvocationResult SendGetRequestAsyncTest(string url)
        {

            List<MethodParameter> methodParameters = new List<MethodParameter>();

            methodParameters.Add(new MethodParameter(url, url.GetType()));

            Console.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod().Name} {url} {url.GetType()} {url}");

            return new MethodInvocationResult(System.Reflection.MethodBase.GetCurrentMethod().Name, methodParameters, "test", "test".GetType(), null, MethodInvocationState.MethodInvoked);
        }
    }
}
