using System;
using System.Collections.Generic;
using PolishNewsHarvesterSdk.Domain;
using PolishNewsHarvesterSdk.Dto;

namespace PolishNewsHarvesterWorker
{
    public interface IParser
    {
        void FetchAndParse(string url, ICollection<Func<Method, Method>> parsingMethods);
    }
}