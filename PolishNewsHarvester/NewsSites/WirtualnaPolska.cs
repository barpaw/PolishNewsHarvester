using System;
using System.Collections.Generic;
using PolishNewsHarvester.Common.Dto;

namespace PolishNewsHarvester.NewsSites
{
    class WirtualnaPolska : INewsSite
    {
        public string SearchByTagUrl { get; init; }
        public string Tag { get; init; }
        
        public WirtualnaPolska(string tag)
        {
            SearchByTagUrl = "https://wiadomosci.wp.pl/tag/";
            Tag = tag;
        }

        public ICollection<NewsMetadataDto> Parse()
        {
            
            // Step 1 -> Get Response From SearchByTagUrl
            // Step 2 -> Parse Response To Get Articles Links
            // Step 3 -> Get Responses From Article Links
            // Step 4 -> Parse Article
            // Step 5 -> return
            
            
            
            
            throw new NotImplementedException();
        }
        
    }
}
