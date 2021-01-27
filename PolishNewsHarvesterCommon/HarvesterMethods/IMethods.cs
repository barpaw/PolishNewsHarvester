using PolishNewsHarvesterCommon.Dto;
using PolishNewsHarvesterSdk.Consts;
using PolishNewsHarvesterSdk.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolishNewsHarvesterCommon.HarvesterMethods
{
    public interface IMethods
    {
        Task<HttpResponseDto> SendGetRequestAsync(string url, string httpClientType);
        List<string> GetNodesHrefsValuesByXpath(string url, string xpath);
        string AddStringAtBeginning(string strToAppend, string originalStr);
        NewsMetadataDto GetNodesInnerTextByXpath(NewsMetadataDto newsMetadataDto, List<string> xpaths, NewsMetadataValue newsMetadataValue);
        NewsMetadataDto GetNodeInnerTextByXpath(NewsMetadataDto newsMetadataDto, List<string> xpaths, NewsMetadataValue newsMetadataValue);
        List<string> RemoveStringsFromList(List<string> originalList, List<string> stringsToRemove);
        NewsMetadataDto GetNodeAttributeValueByXpath(NewsMetadataDto newsMetadataDto, List<string> xpaths, string attributeName, NewsMetadataValue newsMetadataValue);
        string GetNodeInnerTextByXpathAsString(NewsMetadataDto newsMetadataDto, List<string> xpaths);

    }
}
