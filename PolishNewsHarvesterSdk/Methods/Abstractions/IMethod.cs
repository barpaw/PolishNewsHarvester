using PolishNewsHarvesterSdk.Domain;
using PolishNewsHarvesterSdk.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolishNewsHarvesterSdk.Methods.Abstractions
{
    public interface IMethod
    {
        MethodInvocationResult InvokeMethod();
    }
}
