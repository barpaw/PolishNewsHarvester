using System.Collections.Generic;
using PolishNewsHarvesterSdk.Domain;
using PolishNewsHarvesterSdk.Methods;

namespace PolishNewsHarvesterSdk.Targets
{
    public class WirtualnaPolska : INewsSiteSearchByTag
    {
        public string GetNewsByTagUrl { get; init; }
        public string Tag { get; init; }

        public WirtualnaPolska(string tag)
        {
            Tag = tag;
        }

        public ICollection<MethodInvocationResult> GetNewsByTag()
        {
            List<MethodInvocationResult> mIR = new List<MethodInvocationResult>();

            mIR.Add(new SendGetRequestAsync("tesst").InvokeMethod());
            mIR.Add(new SendGetRequestAsyncTest("dsds").InvokeMethod());
            mIR.Add(new SendGetRequestAsync("dsdsds").InvokeMethod());
            mIR.Add(new SendGetRequestAsyncTest("dsdsdsdsds").InvokeMethod());
            mIR.Add(new SendGetRequestAsync("csccc").InvokeMethod());

            return mIR;
        }
    }
}