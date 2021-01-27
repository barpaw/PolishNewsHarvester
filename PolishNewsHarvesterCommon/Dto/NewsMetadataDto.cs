using PolishNewsHarvesterSdk.Enums;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PolishNewsHarvesterCommon.Dto
{
    public class NewsMetadataDto
    {
        public NewsMetadataDto(HttpResponseDto httpResponseDto)
        {
            Url = httpResponseDto.Url;
            HtmlBody = httpResponseDto.Body;
        }

        public string Title { get; set; }
        public string Url { get; set; }
        public string Tag { get; set; }
        public string Authors { get; set; }
        public DateTime PublishedDate { get; set; }
        public string PublishedDateRaw { get; set; }

        [JsonIgnore]
        public string HtmlBody { get; set; }

        public void SetValue(NewsMetadataValue newsMetadataValue, object o)
        {
            switch (newsMetadataValue)
            {
                case NewsMetadataValue.Authors:
                    Authors = o as string;
                    break;
                case NewsMetadataValue.Title:
                    Title = o as string;
                    break;
                case NewsMetadataValue.Tag:
                    Tag = o as string;
                    break;
                case NewsMetadataValue.HtmlBody:
                    HtmlBody = o as string;
                    break;
                case NewsMetadataValue.PublishedDate:
                    PublishedDate = DateTime.Parse(o as string);
                    break;
                case NewsMetadataValue.PublishedDateRaw:
                    PublishedDateRaw = o as string;
                    break;
                case NewsMetadataValue.Url:
                    Url = o as string;
                    break;
                default:
                    break;
            }
        }


    }
}
