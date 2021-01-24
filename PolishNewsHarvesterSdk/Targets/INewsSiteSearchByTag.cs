using System.Collections.Generic;
using PolishNewsHarvesterSdk.Domain;

namespace PolishNewsHarvesterSdk.Targets
{
    public interface INewsSiteSearchByTag : INewsSite
    {
        public string GetNewsByTagUrl { get; set; }

        ICollection<MethodInvocationResult> GetNewsByTag(string tag);
    }
}