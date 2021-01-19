using System.Collections.Generic;
using PolishNewsHarvesterSdk.Dto;

namespace PolishNewsHarvesterWorker.NewsSites
{
    interface INewsSite
    {
        public string SearchByTagUrl { get; init; }
        public string Tag { get; init; }
        public ICollection<NewsMetadataDto> Parse();
    }
}