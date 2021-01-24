using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using PolishNewsHarvesterSdk.Dto;
using PolishNewsHarvesterSdk.Methods;

namespace PolishNewsHarvesterSdk.Targets
{
    public class WirtualnaPolska : IWirtualnaPolska
    {
        public string GetNewsByTagUrl { get; set; } = "https://wiadomosci.wp.pl/tag/";

        private readonly ILogger<WirtualnaPolska> _logger;
        private IMethods _methods;

        public WirtualnaPolska(ILogger<WirtualnaPolska> logger, IMethods methods)
        {
            _logger = logger;
            _methods = methods;
        }

        public ICollection<NewsMetadataDto> GetNewsByTag(string tag)
        {


            _methods.SendGetRequestAsync(GetNewsByTagUrl + tag);
            _methods.SendGetRequestAsyncTest("dsdadas");

            return new List<NewsMetadataDto>();

        }
    }
}