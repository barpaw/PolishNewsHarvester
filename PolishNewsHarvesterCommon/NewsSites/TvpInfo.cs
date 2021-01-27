using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public class TvpInfo : ITvpInfo
    {
        public string GetNewsByTagUrl { get; set; } = "https://www.tvp.info/szukaj?type=news&query=";

        private readonly ILogger<TvpInfo> _logger;
        private IMethods _methods;

        public TvpInfo(ILogger<TvpInfo> logger, IMethods methods)
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

                var jsObjectsList = _methods.GetNodesInnerTextByXpathAsStrings(new NewsMetadataDto(httpResponseDto), new List<string>() { "//script[@type='text/javascript']" });

                jsObjectsList.RemoveAll(str => !str.Contains("title") && !str.Contains("publication_start") && !str.Contains("url"));

                var jsonRaw = jsObjectsList.First();
                jsonRaw = jsonRaw.Replace("window.__directoryData =", "").Trim();
                jsonRaw = jsonRaw.Remove(jsonRaw.Length - 1);

                dynamic dynamicJson = JsonConvert.DeserializeObject(jsonRaw);

                List<string> hrefs = new List<string>();
                foreach (JObject item in dynamicJson.items)
                {
                    string url = item.GetValue("url").ToString();
                    _logger.LogInformation("{href}", url);

                    hrefs.Add(url);
                }

                var hrefsWithDomain = hrefs.Select(href => _methods.AddStringAtBeginning("https://www.tvp.info/", href)).ToList();

                var httpResponseDtosTasks = hrefsWithDomain.Select(async href => await _methods.SendGetRequestAsync(href, HttpClients.DefaultClient)).ToList();

                var httpResponseDtosArr = await Task.WhenAll(httpResponseDtosTasks);

                var newsMetadataDtos = httpResponseDtosArr.Select(httpResponseDto => new NewsMetadataDto(httpResponseDto)).ToList();

                var newsMetadataDtosWithTitles = newsMetadataDtos.Select(newsMetadataDto => _methods.GetNodeAttributeValueByXpath(newsMetadataDto, new List<string>() { "//*[@property='og:title']" }, "content", NewsMetadataValue.Title)).ToList();



                var newsMetadataDtosWithTitlesTimeRaw = newsMetadataDtos.Select(newsMetadataDto => _methods.GetNodeInnerTextByXpath(newsMetadataDto, new List<string>() { "//span[@class='date']" }, NewsMetadataValue.PublishedDateRaw)).ToList();

                var newsMetadataDtosWithTitlesTime = newsMetadataDtosWithTitlesTimeRaw.Select(newsMetadataDto => { newsMetadataDto.PublishedDate = DateTime.Parse(newsMetadataDto.PublishedDateRaw.Trim()); return newsMetadataDto; }).ToList();

                var newsMetadataDtosWithTitlesTimeTag = newsMetadataDtosWithTitlesTime.Select(newsMetadataDto => { newsMetadataDto.Tag = tag; return newsMetadataDto; }).ToList();

                var newsMetadataDtosWithTitlesTimeTagAuthors = newsMetadataDtos.Select(newsMetadataDto => _methods.GetNodeInnerTextByXpath(newsMetadataDto, new List<string>() { "//span[@class='name']" }, NewsMetadataValue.Authors)).ToList();

                newsMetadataDtosWithTitlesTimeTagAuthors.ForEach(newsMetadataDto => _logger.LogInformation(JsonConvert.SerializeObject(newsMetadataDto)));
                
                returnListNewsMetadataDto = newsMetadataDtosWithTitlesTimeTagAuthors;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{workerName}: Exception caught in method GetNewsByTag.", "TvpInfo");
            }

            return returnListNewsMetadataDto;

        }
    }
}
