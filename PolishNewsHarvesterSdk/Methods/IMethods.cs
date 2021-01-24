using PolishNewsHarvesterSdk.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolishNewsHarvesterSdk.Methods
{
    public interface IMethods
    {
        MethodInvocationResult SendGetRequestAsync(string url);
        MethodInvocationResult SendGetRequestAsyncTest(string url);
    }
}
