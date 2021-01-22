using System.Collections.Generic;
using PolishNewsHarvesterSdk.Domain;

namespace PolishNewsHarvesterSdk.Targets
{
    public interface INewsSiteSearchByTag : INewsSite
    {
        public string GetNewsByTagUrl { get; init; }
        public string Tag { get; init; }


        ICollection<MethodInvocationResult> GetNewsByTag();
    }
}