using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PolishNewsHarvesterCommon.Dto;
using PolishNewsHarvesterCommon.HarvesterMethods;
using PolishNewsHarvesterSdk.Consts;
using PolishNewsHarvesterSdk.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolishNewsHarvesterSdk.NewsSites
{
    public class Gazeta : IGazeta
    {
        public string GetNewsByTagUrl { get; set; } = "https://szukaj.gazeta.pl/wyszukaj/artykul?sortMode=DATE&pageNumber=1&dxx=126213&query=";

        private readonly ILogger<Gazeta> _logger;
        private IMethods _methods;

        public Gazeta(ILogger<Gazeta> logger, IMethods methods)
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


                var hrefsWithoutDomain = _methods.GetHtmlNodesAsStrings(httpResponseDto.Body, "//section[@class='elem normal']");

                var newsMetadataDtos = hrefsWithoutDomain.Select(html => new NewsMetadataDto(html)).ToList();

                var newsMetadataDtosTitles = newsMetadataDtos.Select(newsMetadataDto => _methods.GetNodeAttributeValueByXpath(newsMetadataDto, new List<string>() { "//a[@title]" }, "title", NewsMetadataValue.Title)).ToList();
               
                var newsMetadataDtosTitlesUrl = newsMetadataDtosTitles.Select(newsMetadataDto => _methods.GetNodeAttributeValueByXpath(newsMetadataDto, new List<string>() { "//a[@href]" }, "href", NewsMetadataValue.Url)).ToList();

                var newsMetadataDtosTitlesUrlAuthors = newsMetadataDtosTitles.Select(newsMetadataDto => _methods.GetNodeInnerTextByXpath(newsMetadataDto, new List<string>() { "//span[@class='author']" }, NewsMetadataValue.Authors)).ToList();

                var newsMetadataDtosTitlesUrlAuthorsTag = newsMetadataDtosTitlesUrlAuthors.Select(newsMetadataDto => { newsMetadataDto.Tag = tag; return newsMetadataDto; }).ToList();

                var newsMetadataDtosTitlesUrlAuthorsTagTime = newsMetadataDtosTitlesUrlAuthorsTag.Select(newsMetadataDto => _methods.GetNodeInnerTextByXpath(newsMetadataDto, new List<string>() { "//time[@datetime]" }, NewsMetadataValue.PublishedDate)).ToList();


                newsMetadataDtosTitlesUrlAuthorsTagTime.ForEach(newsMetadataDto => _logger.LogInformation(JsonConvert.SerializeObject(newsMetadataDto)));

                  returnListNewsMetadataDto = newsMetadataDtosTitlesUrlAuthorsTagTime;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{workerName}: Exception caught in method GetNewsByTag.", "Gazeta");
            }

            return returnListNewsMetadataDto;
        }
    }
}
