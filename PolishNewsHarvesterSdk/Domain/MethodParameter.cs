using System;

namespace PolishNewsHarvesterSdk.Domain
{
    public class MethodParameter
    {

        public object Value { get; init; }
        public Type Type { get; init; }

        public MethodParameter(object value, Type type)
        {
            Value = value;
            Type = type;
        }
    }
}