using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PolishNewsHarvesterSdk.Dto;

namespace PolishNewsHarvesterWorker
{
    public class Parser : IParser
    {
        private readonly ILogger<Parser> _logger;
        private IConfiguration _configuration;
        private IFetcher _fetcher;


        public Parser(ILogger<Parser> logger, IConfiguration configuration, IFetcher fetcher)
        {
            _logger = logger;
            _configuration = configuration;
            _fetcher = fetcher;
        }

        public async void FetchAndParse(string url, ICollection<Func<int, int>> parsingMethods)
        {
            var httpResponseDto = await _fetcher.FetchUrl(url);
            
            // ? find how to invoke a multi operations on variable which will change state on each iteration.

            var responseBody = httpResponseDto.Body;
            /*
            foreach (var method in parsingMethods)
            {
                var parseMethodResultDto = method.Invoke(1);
                var asa = Convert.ChangeType(parseMethodResultDto.ParseMethodResult, parseMethodResultDto.ParseMethodResultType);
                _logger.LogInformation(asa.ToString());
            }
            */
      
          
        }
        
    }
}