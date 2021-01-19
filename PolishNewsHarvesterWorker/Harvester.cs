using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolishNewsHarvesterSdk.Dto;
using PolishNewsHarvesterWorker.NewsSites;

namespace PolishNewsHarvesterWorker
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
