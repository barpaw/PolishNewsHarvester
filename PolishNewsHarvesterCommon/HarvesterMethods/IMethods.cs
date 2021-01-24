using PolishNewsHarvesterSdk.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolishNewsHarvesterCommon.HarvesterMethods
{
    public interface IMethods
    {
        HttpResponseDto SendGetRequestAsync(string url);
        void GetHrefValuesByXpath(string url, string xpath);
    }
}
