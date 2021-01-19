using System.Collections.Generic;
using PolishNewsHarvester.Common.Dto;

namespace PolishNewsHarvester.NewsSites
{
    interface INewsSite
    {
        public string SearchByTagUrl { get; init; }
        public string Tag { get; init; }
        public ICollection<NewsMetadataDto> Parse();
    }
}