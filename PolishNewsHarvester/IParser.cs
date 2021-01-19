using System;

namespace PolishNewsHarvester
{
    public interface IParser
    {
        void ParseHtml(Func<string, int> htmlParser);
        void ParseXml(Func<string, int> xmlParser);
        void ParseJson(Func<string, int> jsonParser);
    }
}