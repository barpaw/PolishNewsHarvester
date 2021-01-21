using System;
using System.Collections.Generic;

namespace PolishNewsHarvesterSdk.Dto
{
    public class NewsMetadataDto
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Tag { get; set; }
        public List<string> Authors { get; set; }
        public DateTimeOffset PublishedDate { get; set; }
    }
}
