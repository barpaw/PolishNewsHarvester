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
    public class PolskaAgencjaPrasowa : IPolskaAgencjaPrasowa
    {
        public string GetNewsByTagUrl { get; set; } = "https://www.pap.pl/wyszukiwanie/";

        private readonly ILogger<PolskaAgencjaPrasowa> _logger;
        private IMethods _methods;

        public PolskaAgencjaPrasowa(ILogger<PolskaAgencjaPrasowa> logger, IMethods methods)
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


                var hrefsWithoutDomain = _methods.GetNodesHrefsValuesByXpath(httpResponseDto.Body, "//a[@href]");

                hrefsWithoutDomain.RemoveAll(href => !href.StartsWith("/node"));

                var validHrefsWithoutDomain = hrefsWithoutDomain.Distinct().ToList();

                var validHrefsWithDomain = validHrefsWithoutDomain.Select(href => _methods.AddStringAtBeginning("https://www.pap.pl", href)).ToList();

                var httpResponseDtosTasks = validHrefsWithDomain.Select(async href => await _methods.SendGetRequestAsync(href, HttpClients.DefaultClient)).ToList();

                var httpResponseDtosArr = await Task.WhenAll(httpResponseDtosTasks);

                var newsMetadataDtos = httpResponseDtosArr.Select(httpResponseDto => new NewsMetadataDto(httpResponseDto)).ToList();

                var newsMetadataDtosWithTitles = newsMetadataDtos.Select(newsMetadataDto => _methods.GetNodeAttributeValueByXpath(newsMetadataDto, new List<string>() { "//*[@property='og:title']" }, "content", NewsMetadataValue.Title)).ToList();

                var newsMetadataDtosWithTitlesTimeRaw = newsMetadataDtos.Select(newsMetadataDto => _methods.GetNodeInnerTextByXpath(newsMetadataDto, new List<string>() { "//div[@class='moreInfo']" }, NewsMetadataValue.PublishedDateRaw)).ToList();

                var newsMetadataDtosWithTitlesTime = newsMetadataDtosWithTitlesTimeRaw.Select(newsMetadataDto => { newsMetadataDto.PublishedDate = DateTime.Parse(newsMetadataDto.PublishedDateRaw.Split(new string[] { "aktualizacja:" }, StringSplitOptions.None)[0].Trim()); return newsMetadataDto; }).ToList();

                var newsMetadataDtosWithTitlesTimeTag = newsMetadataDtosWithTitlesTime.Select(newsMetadataDto => { newsMetadataDto.Tag = tag; return newsMetadataDto; }).ToList();

                var newsMetadataDtosWithTitlesTimeTagAuthors = newsMetadataDtosWithTitlesTimeTag.Select(newsMetadataDto => { newsMetadataDto.Authors = "-"; return newsMetadataDto; }).ToList();

                // Z obserwacji wynikia, że wyszukiwarka na pap.pl wyszukuje po całych zaindeksowanych dokumentach HTML. 
                // Co za tym idzie jest dużo false-positivów w wynikach wyszukiwania po tagu/słowie kluczowym.
                // Dlatego upewniamy się, że w tekście artykułu występuje szukane przez nas słowo i odsiewamy false-positivy.
                var newsMetadataDtosWithTitlesTimeTagAuthorsValid = newsMetadataDtosWithTitlesTimeTagAuthors.Where(newsMetadataDto => _methods.GetNodeInnerTextByXpathAsString(newsMetadataDto, new List<string>() { "//article[@typeof='schema:Article']" }).ToLower().Contains(tag.ToLower())).ToList();






                newsMetadataDtosWithTitlesTimeTagAuthorsValid.ForEach(newsMetadataDto => _logger.LogInformation(JsonConvert.SerializeObject(newsMetadataDto)));

            }
            catch (Exception e)
            {
                _logger.LogError(e, "{workerName}: Exception caught in method GetNewsByTag.", "PolskaAgencjaPrasowa");
            }

            return returnListNewsMetadataDto;
        }
    }
}
