using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using PolishNewsHarvesterCommon.Dto;
using PolishNewsHarvesterSdk.Consts;
using PolishNewsHarvesterSdk.Enums;
using PolishNewsHarvesterSdk.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PolishNewsHarvesterCommon.HarvesterMethods
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

        public async Task<HttpResponseDto> SendGetRequestAsync(string url, string httpClientType)
        {
            var defaultHttpResponseDto = new HttpResponseDto(url, new byte[0], -1, string.Empty);

            try
            {
                var httpResponseMessage = await _httpManager.SendGetRequestAsync(url, httpClientType);

                var body = await httpResponseMessage.Content.ReadAsByteArrayAsync();
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
                _logger.LogError(e, "{workerName}: Exception caught in method SendGetRequestAsync.", "Methods");
                throw;
            }

            _logger.LogInformation("{workerName}: [{statusCode}] {url}", "SendGetRequestAsync", defaultHttpResponseDto.StatusCode, defaultHttpResponseDto.Url);

            return defaultHttpResponseDto;
        }
        public List<string> GetNodesHrefsValuesByXpath(string body, string xpath)
        {

            var hrefs = new List<string>();

            try
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(body);

                var nodes = htmlDoc.DocumentNode.SelectNodes(xpath);

                if (nodes != null)
                {

                    foreach (HtmlNode divNode in nodes)
                    {
                        hrefs.Add(divNode.Attributes["href"].Value.Trim());
                    }

                    _logger.LogInformation("{workerName}: Nodes count: {nodesCount}.", "GetHrefValuesByXpath", nodes.Count);
                    _logger.LogInformation("{workerName}: hrefs count: {nodesCount}.", "GetHrefValuesByXpath", hrefs.Count);

                }
                else
                {
                    throw new Exception("Nodes selected by given xpath equals null.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{workerName}: Exception caught in method GetHrefValuesByXpath.", "Methods");
                throw;
            }

            return hrefs;
        }

        public List<string> GetNodesHrefsValuesByXpathAndParentNodeXpath(string body, string parentNodeXpath)
        {

            var hrefs = new List<string>();

            try
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(body);

                var parentNodes = htmlDoc.DocumentNode.SelectNodes(parentNodeXpath);

                if (parentNodes != null)
                {

                    foreach (HtmlNode pNode in parentNodes)
                    {
                        _logger.LogInformation("{a}", pNode.InnerHtml);
                        var hrefNode = pNode.Descendants("a").FirstOrDefault();

                        _logger.LogInformation("{a}", hrefNode?.Attributes["href"]?.Value?.Trim());

                        if (hrefNode != null)
                        {
                            hrefs.Add(hrefNode.Attributes["href"].Value.Trim());
                        }
                        else
                        {
                            // throw new Exception("Nodes selected by given hrefXpath equals null.");
                        }


                    }

                    _logger.LogInformation("{workerName}: ParentNodes count: {nodesCount}.", "GetHrefValuesByXpath", parentNodes.Count);
                    _logger.LogInformation("{workerName}: hrefs count: {nodesCount}.", "GetHrefValuesByXpath", hrefs.Count);

                }
                else
                {
                    throw new Exception("Nodes selected by given parentXpath equals null.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{workerName}: Exception caught in method GetNodesHrefsValuesByXpathAndParentNodeXpath.", "Methods");
                throw;
            }

            return hrefs;
        }

        public List<string> GetHtmlNodesAsStrings(string body, string xpath)
        {
            var retNodes = new List<string>();

            try
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(body);

                var nodes = htmlDoc.DocumentNode.SelectNodes(xpath);

                if (nodes != null)
                {

                    foreach (HtmlNode pNode in nodes)
                    {

                        var html = pNode.InnerHtml;


                        _logger.LogInformation("{innerHtml}", html);

                        retNodes.Add(html);

                    }

                    _logger.LogInformation("{workerName}: Nodes count: {nodesCount}.", "GetHtmlNodesAsStrings", retNodes.Count);

                }
                else
                {
                    throw new Exception("Nodes selected by given xpath equals null.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{workerName}: Exception caught in method GetHtmlNodesAsStrings.", "Methods");
                throw;
            }

            return retNodes;
        }

        public string AddStringAtBeginning(string strToAppend, string originalStr)
        {
            return $"{strToAppend}{originalStr}";
        }
        public NewsMetadataDto GetNodesInnerTextByXpath(NewsMetadataDto newsMetadataDto, List<string> xpaths, NewsMetadataValue newsMetadataValue)
        {

            var retVal = string.Empty;

            try
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(newsMetadataDto.HtmlBody);


                HtmlNodeCollection validNodes = null;

                foreach (var xpath in xpaths)
                {
                    var nodes = htmlDoc.DocumentNode.SelectNodes(xpath);

                    if (nodes != null)
                    {
                        validNodes = nodes;
                        break;
                    }
                }

                if (validNodes != null)
                {

                    foreach (HtmlNode divNode in validNodes)
                    {

                        _logger.LogInformation("{workerName}: {as}", "GetNodesInnerTextByXpath", divNode.InnerText.Trim());
                        retVal = divNode.InnerText.Trim();
                    }

                }
                else
                {
                    _logger.LogError("Nodes selected by given xpath equals null. URL: {url} XPATH: {xpath}", newsMetadataDto.Url, String.Join(", ", xpaths));
                    retVal = "Brak.";
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{workerName}: Exception caught in method GetNodesInnerTextByXpath.", "Methods");
                throw;
            }

            newsMetadataDto.SetValue(newsMetadataValue, retVal);

            return newsMetadataDto;
        }

        public List<string> GetNodesInnerTextByXpathAsStrings(NewsMetadataDto newsMetadataDto, List<string> xpaths)
        {

            var retVal = new List<string>();

            try
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(newsMetadataDto.HtmlBody);


                HtmlNodeCollection validNodes = null;

                foreach (var xpath in xpaths)
                {
                    var nodes = htmlDoc.DocumentNode.SelectNodes(xpath);

                    if (nodes != null)
                    {
                        validNodes = nodes;
                        break;
                    }
                }

                if (validNodes != null)
                {

                    foreach (HtmlNode divNode in validNodes)
                    {

                        _logger.LogInformation("{workerName}: {as}", "GetNodesInnerTextByXpathAsStrings", divNode.InnerText.Trim());
                        retVal.Add(divNode.InnerText.Trim());
                    }

                }
                else
                {
                    _logger.LogError("Nodes selected by given xpath equals null. URL: {url} XPATH: {xpath}", newsMetadataDto.Url, String.Join(", ", xpaths));
                    retVal = new List<string>();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{workerName}: Exception caught in method GetNodesInnerTextByXpathAsStrings.", "Methods");
                throw;
            }

            return retVal;
        }

        public NewsMetadataDto GetNodeInnerTextByXpath(NewsMetadataDto newsMetadataDto, List<string> xpaths, NewsMetadataValue newsMetadataValue)
        {

            var retVal = string.Empty;

            try
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(newsMetadataDto.HtmlBody);

                HtmlNode validNode = null;

                foreach (var xpath in xpaths)
                {
                    var nodes = htmlDoc.DocumentNode.SelectNodes(xpath);

                    if (nodes != null)
                    {
                        validNode = nodes.First();
                        break;
                    }
                }

                if (validNode != null)
                {
                    _logger.LogInformation("{workerName}: {as}", "GetNodeInnerTextByXpath", validNode.InnerText.Trim());
                    retVal = validNode.InnerText.Trim();
                }
                else
                {
                    _logger.LogError("Nodes selected by given xpaths equals null. URL: {url} XPATH: {xpath}", newsMetadataDto.Url, String.Join(", ", xpaths));
                    retVal = "Brak.";
                }


            }
            catch (Exception e)
            {
                _logger.LogError(e, "{workerName}: Exception caught in method GetNodeInnerTextByXpath.", "Methods");
                throw;
            }

            newsMetadataDto.SetValue(newsMetadataValue, retVal);

            return newsMetadataDto;
        }
        public List<string> RemoveStringsFromList(List<string> originalList, List<string> stringsToRemove)
        {
            return originalList.Except(stringsToRemove).ToList();
        }
        public NewsMetadataDto GetNodeAttributeValueByXpath(NewsMetadataDto newsMetadataDto, List<string> xpaths, string attributeName, NewsMetadataValue newsMetadataValue)
        {

            var retVal = string.Empty;

            try
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(newsMetadataDto.HtmlBody);

                HtmlNode validNode = null;

                foreach (var xpath in xpaths)
                {
                    var nodes = htmlDoc.DocumentNode.SelectNodes(xpath);

                    if (nodes != null)
                    {
                        validNode = nodes.First();
                        break;
                    }
                }

                if (validNode != null && validNode.Attributes[attributeName].Value != null)
                {
                    _logger.LogInformation("{workerName}: {as}", "GetNodeAttributeValueByXpath", validNode.Attributes[attributeName].Value.Trim());
                    retVal = validNode.Attributes[attributeName].Value.Trim();

                }
                else
                {
                    _logger.LogError("Nodes selected by given xpaths equals null. URL: {url} XPATH: {xpath} ATTRIBUTE_NAME: {attributeName}", newsMetadataDto.Url, String.Join(", ", xpaths), attributeName);
                    retVal = "Brak.";
                }


            }
            catch (Exception e)
            {
                _logger.LogError(e, "{workerName}: Exception caught in method GetNodeAttributeValueByXpath.", "Methods");
                throw;
            }

            newsMetadataDto.SetValue(newsMetadataValue, retVal);

            return newsMetadataDto;
        }

        public string GetNodeInnerTextByXpathAsString(NewsMetadataDto newsMetadataDto, List<string> xpaths)
        {

            var retVal = string.Empty;

            try
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(newsMetadataDto.HtmlBody);

                HtmlNode validNode = null;

                foreach (var xpath in xpaths)
                {
                    var nodes = htmlDoc.DocumentNode.SelectNodes(xpath);

                    if (nodes != null)
                    {
                        validNode = nodes.First();
                        break;
                    }
                }

                if (validNode != null)
                {
                    _logger.LogInformation("{workerName}: {as}", "GetNodeInnerTextByXpathAsString", validNode.InnerText.Trim('\r', '\n'));
                    retVal = validNode.InnerText.Trim('\r', '\n');
                }
                else
                {
                    _logger.LogError("Nodes selected by given xpaths equals null. URL: {url} XPATH: {xpath}", newsMetadataDto.Url, String.Join(", ", xpaths));
                    retVal = "Brak.";
                }


            }
            catch (Exception e)
            {
                _logger.LogError(e, "{workerName}: Exception caught in method GetNodeInnerTextByXpathAsString.", "Methods");
                throw;
            }


            return retVal;
        }







    }
}
