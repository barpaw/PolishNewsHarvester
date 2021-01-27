using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PolishNewsHarvesterCommon.Dto;
using PolishNewsHarvesterCommon.HarvesterMethods;
using PolishNewsHarvesterSdk.Consts;
using PolishNewsHarvesterSdk.Enums;

namespace PolishNewsHarvesterSdk.NewsSites
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

        public async Task<ICollection<NewsMetadataDto>> GetNewsByTag(string tag)
        {

            List<NewsMetadataDto> returnListNewsMetadataDto = new List<NewsMetadataDto>();

            try
            {

                var httpResponseDtoTask = _methods.SendGetRequestAsync(GetNewsByTagUrl + tag, HttpClients.DefaultClient);

                var httpResponseDtoArr = await Task.WhenAll(httpResponseDtoTask);
                var httpResponseDto = httpResponseDtoArr.FirstOrDefault();


                var hrefsWithoutDomain = _methods.GetNodesHrefsValuesByXpath(httpResponseDto.Body, "//a[@class='f2PrHTUx']");
                var hrefsWithDomain = hrefsWithoutDomain.Select(href => _methods.AddStringAtBeginning("https://wiadomosci.wp.pl", href)).ToList();

                // remove hrefs that do not ends with 'a'. (a == article)

                int removedHrefs = hrefsWithDomain.RemoveAll(href => !href.EndsWith('a'));

                _logger.LogInformation("{workerName}: Removed not-articles hrefs: {removedHrefs}.", "WirtualnaPolska", removedHrefs);

                var httpResponseDtosTasks = hrefsWithDomain.Select(async href => await _methods.SendGetRequestAsync(href, HttpClients.DefaultClient)).ToList();

                var httpResponseDtosArr = await Task.WhenAll(httpResponseDtosTasks);

                var newsMetadataDtos = httpResponseDtosArr.Select(httpResponseDto => new NewsMetadataDto(httpResponseDto)).ToList();

                var newsMetadataDtosWithTitles = newsMetadataDtos.Select(newsMetadataDto => _methods.GetNodesInnerTextByXpath(newsMetadataDto, new List<string>() { "//h1[@itemprop='headline']", "//h1[@itemprop='name']" }, NewsMetadataValue.Title)).ToList();

                var newsMetadataDtosWithTitlesAuthors = newsMetadataDtosWithTitles.Select(newsMetadataDto => _methods.GetNodeInnerTextByXpath(newsMetadataDto, new List<string>() { "//*[@class='f21Hub9j']" }, NewsMetadataValue.Authors)).ToList();

                var newsMetadataDtosWithTitlesAuthorsTags = newsMetadataDtosWithTitlesAuthors.Select(newsMetadataDto => { newsMetadataDto.Tag = tag; return newsMetadataDto; }).ToList();

                var newsMetadataDtosWithTitlesAuthorsTagsTime = newsMetadataDtosWithTitlesAuthorsTags.Select(newsMetadataDto => _methods.GetNodeAttributeValueByXpath(newsMetadataDto, new List<string>() { "//time[@datetime]" }, "datetime", NewsMetadataValue.PublishedDate)).ToList();


                newsMetadataDtosWithTitlesAuthorsTagsTime.ForEach(newsMetadataDto => _logger.LogInformation(JsonConvert.SerializeObject(newsMetadataDto)));

                returnListNewsMetadataDto = newsMetadataDtosWithTitlesAuthorsTagsTime;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{workerName}: Exception caught in method GetNewsByTag.", "WirtualnaPolska");
            }

            return returnListNewsMetadataDto;

        }
    }
}