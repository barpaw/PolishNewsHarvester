using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using PolishNewsHarvesterSdk.Domain;
using PolishNewsHarvesterSdk.Methods;
using PolishNewsHarvesterSdk.Methods.Abstractions;

namespace PolishNewsHarvesterSdk.Targets
{
    public class WirtualnaPolska : IWirtualnaPolska
    {
        public string GetNewsByTagUrl { get; set; } = "https://wiadomosci.wp.pl/tag/";

        private readonly ILogger<WirtualnaPolska> _logger;
        private IMethods _methods;

        public WirtualnaPolska(ILogger<WirtualnaPolska> logger, IMethods methods)
        {
            _logger = logger;
            _methods = methods;
        }

        public ICollection<MethodInvocationResult> GetNewsByTag(string tag)
        {
            List<MethodInvocationResult> mIR = new List<MethodInvocationResult>();

            mIR.Add(_methods.SendGetRequestAsync($"{GetNewsByTagUrl}{tag}"));
            mIR.Add(_methods.SendGetRequestAsyncTest("zxc"));


            return mIR;
        }
    }
}