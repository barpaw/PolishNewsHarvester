using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolishNewsHarvester.Common.Dto;
using PolishNewsHarvester.NewsSites;

namespace PolishNewsHarvester
{
    class Harvester<TNewsSite> where TNewsSite : INewsSite
    {
        public Harvester()
        {
        }

        public ICollection<NewsMetadataDto> Harvest(TNewsSite newsSite)
        {
            return newsSite.Parse();
        }
    }
}
