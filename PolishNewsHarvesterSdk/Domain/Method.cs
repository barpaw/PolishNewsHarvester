using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using PolishNewsHarvesterSdk.Enums;

namespace PolishNewsHarvesterSdk.Domain
{
    public class Method
    {
        public MethodName Name { get; init; }
        public ICollection<Parameter> Parameters { get; set; }
        
        public object ReturnObject { get; init; }
        public Type ReturnObjectType { get; init; }
        
        public Exception Exception { get; init; }
        public MethodInvocationState InvocationState { get; set; }
    }
    
    
}