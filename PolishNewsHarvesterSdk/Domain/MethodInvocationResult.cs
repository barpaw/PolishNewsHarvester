using PolishNewsHarvesterSdk.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolishNewsHarvesterSdk.Domain
{
    public class MethodInvocationResult
    {

        public string Name { get; init; }
        public ICollection<MethodParameter> Parameters { get; set; }
        public object ReturnObject { get; set; }
        public Type ReturnObjectType { get; set; }
        public Exception Exception { get; set; }
        public MethodInvocationState InvocationState { get; set; }

        public MethodInvocationResult(string name, ICollection<MethodParameter> parameters, object returnObject, Type returnObjectType, Exception exception, MethodInvocationState invocationState)
        {
            Name = name;
            Parameters = parameters;
            ReturnObject = returnObject;
            ReturnObjectType = returnObjectType;
            Exception = exception;
            InvocationState = invocationState;
        }
    }
}
