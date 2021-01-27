using PolishNewsHarvesterSdk.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PolishNewsHarvesterSdk.Http
{
    public interface IHttpManager
    {
        Task<HttpResponseMessage> SendGetRequestAsync(string url, string httpClientType);
    }
}
