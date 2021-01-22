using PolishNewsHarvesterSdk.Domain;
using PolishNewsHarvesterSdk.Enums;
using PolishNewsHarvesterSdk.Methods.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolishNewsHarvesterSdk.Methods
{
    public class SendGetRequestAsyncTest : MethodBase, IMethod
    {
        private string Url { get; init; }


        public SendGetRequestAsyncTest(string url) : base(url)
        {
            Url = url;
        }

        public MethodInvocationResult InvokeMethod()
        {
            Console.WriteLine($"{MethodName} {Url} {Parameters.FirstOrDefault().Type} {Parameters.FirstOrDefault().Value}");

            return new MethodInvocationResult(MethodName, Parameters, "test", "test".GetType(), null, MethodInvocationState.MethodInvoked);
        }
    }
}
