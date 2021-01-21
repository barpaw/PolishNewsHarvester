using System;

namespace PolishNewsHarvesterSdk.Dto
{
    public class ParseMethodInputDto
    {
        public object ParseMethodInputObject { get; init; }
        public Type ParseMethodInputType { get; init; }

        public ParseMethodInputDto(object parseMethodInputObject, Type parseMethodInputType)
        {
            ParseMethodInputObject = parseMethodInputObject;
            ParseMethodInputType = parseMethodInputType;
        }
    }
}