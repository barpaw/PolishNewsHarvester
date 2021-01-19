using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolishNewsHarvester
{
    interface INewsSite
    {
        public string SearchByTagUrl { get; init; }
        public ParserType ParserType { get; init; }
        public List<ArticleDto> ParseSearchByTag();
        public ArticleDto ParseArticle(ArticleDto articleDto);
    }
}
