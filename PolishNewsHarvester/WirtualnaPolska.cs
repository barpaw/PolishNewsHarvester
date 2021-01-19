using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PolishNewsHarvester
{
    class WirtualnaPolska : INewsSite
    {
        public string SearchByTagUrl { get; init; }
        public string Tag { get; init; }
        public ParserType ParserType { get; init; }


        public WirtualnaPolska(string tag)
        {
            SearchByTagUrl = "https://wp.pl";
            ParserType = ParserType.HTML;
            Tag = tag;
        }

        public List<ArticleDto> ParseSearchByTag()
        {
            throw new NotImplementedException();
        }

        public ArticleDto ParseArticle(ArticleDto articleDto)
        {
            throw new NotImplementedException();
        }
    }
}
