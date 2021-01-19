using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolishNewsHarvester
{
    class ArticleDto
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Tag { get; set; }
        public List<string> Authors { get; set; }
        public DateTimeOffset PublishedDate { get; set; }
    }
}
