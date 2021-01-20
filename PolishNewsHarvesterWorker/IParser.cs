using System;
using System.Collections.Generic;
using PolishNewsHarvesterSdk.Dto;

namespace PolishNewsHarvesterWorker
{
    public interface IParser
    {
        void FetchAndParse(string url, ICollection<Func<string, ParseMethodResultDto>> parsingMethods);
    }
}