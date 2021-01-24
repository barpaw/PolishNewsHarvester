using System.Collections.Generic;
using PolishNewsHarvesterSdk.Dto;

namespace PolishNewsHarvesterCommon.NewsSites.Interfaces
{
    public interface INewsSiteSearchByTag
    {
        public string GetNewsByTagUrl { get; set; }

        ICollection<NewsMetadataDto> GetNewsByTag(string tag);
    }
}