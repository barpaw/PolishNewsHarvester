using PolishNewsHarvesterSdk.Domain;
using PolishNewsHarvesterSdk.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolishNewsHarvesterSdk.Methods.Abstractions
{
    public abstract class MethodBase
    {

        public string MethodName { get; set; }
        public ICollection<MethodParameter> Parameters { get; set; }
        public MethodInvocationState InvocationState { get; set; }

        public MethodBase(params dynamic[] values)
        {
            InvocationState = MethodInvocationState.MethodIsRunning;
            MethodName = this.GetType().Name;
            CreateMethodParametersCollection(values);
        }

        private void CreateMethodParametersCollection(dynamic[] values)
        {
            if (values.Any())
            {
                Parameters = values.Select(param => new MethodParameter(param, param.GetType())).ToList();
            }
        }
    }
}
