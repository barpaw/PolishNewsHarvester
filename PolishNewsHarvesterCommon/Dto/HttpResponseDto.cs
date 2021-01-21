namespace PolishNewsHarvesterSdk.Dto
{
    public class HttpResponseDto
    {
        public string Url { get; init; }
        public string Body { get; init; }
        public int StatusCode { get; init; }
        public string ResponseHeaders { get; init; }

        public HttpResponseDto(string url, string body, int statusCode, string responseHeaders)
        {
            Url = url;
            Body = body;
            StatusCode = statusCode;
            ResponseHeaders = responseHeaders;
        }
        
    }
}