using PolishNewsHarvesterSdk.Enums;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Web;

namespace PolishNewsHarvesterCommon.Dto
{
    public class NewsMetadataDto
    {
        public NewsMetadataDto(HttpResponseDto httpResponseDto)
        {
            Url = httpResponseDto.Url;
            HtmlBody = httpResponseDto.Body;
        }

        public NewsMetadataDto(string body)
        {
            HtmlBody = body;
        }

        private string title;
        private string url;
        private string tag;
        private string authors;

        public string Title { get { return title; } set { title = HttpUtility.HtmlDecode(value); } }
        public string Url { get { return url; } set { url = HttpUtility.UrlDecode(value); } }
        public string Tag { get { return tag; } set { tag = HttpUtility.HtmlDecode(value); } }
        public string Authors { get { return authors; } set { authors = HttpUtility.HtmlDecode(value); } }
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
