using System.Collections.Generic;
using PolishNewsHarvesterSdk.Domain;
using PolishNewsHarvesterSdk.Dto;

namespace PolishNewsHarvesterSdk.Targets
{
    public interface INewsSiteSearchByTag
    {
        public string GetNewsByTagUrl { get; set; }

        ICollection<NewsMetadataDto> GetNewsByTag(string tag);
    }
}