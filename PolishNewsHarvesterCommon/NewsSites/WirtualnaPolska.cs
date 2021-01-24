using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using PolishNewsHarvesterCommon.HarvesterMethods;
using PolishNewsHarvesterSdk.Dto;

namespace PolishNewsHarvesterCommon.NewsSites
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


            var httpResponseDto = _methods.SendGetRequestAsync(GetNewsByTagUrl + tag);
            _methods.GetHrefValuesByXpath(httpResponseDto.Body, "//a[@class='f2PrHTUx']");

            return new List<NewsMetadataDto>();

        }
    }
}