using System.Collections.Generic;
using System.Threading.Tasks;
using PolishNewsHarvesterCommon.Dto;

namespace PolishNewsHarvesterCommon.NewsSites.Interfaces
{
    public interface INewsSiteSearchByTag
    {
        public string GetNewsByTagUrl { get; set; }

        Task<ICollection<NewsMetadataDto>> GetNewsByTag(string tag);
    }
}