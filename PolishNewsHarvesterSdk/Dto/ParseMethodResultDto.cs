using System;
using Microsoft.VisualBasic.CompilerServices;

namespace PolishNewsHarvesterSdk.Dto
{
    public class ParseMethodResultDto
    {
        public string ParseMethodName { get; init; }
        public object ParseMethodResult { get; init; }
        public Type ParseMethodResultType { get; init; }

        public ParseMethodResultDto(string parseMethodName, object parseMethodResult, Type parseMethodResultType)
        {
            ParseMethodName = parseMethodName;
            ParseMethodResult = parseMethodResult;
            ParseMethodResultType = parseMethodResultType;
        }


    }
}