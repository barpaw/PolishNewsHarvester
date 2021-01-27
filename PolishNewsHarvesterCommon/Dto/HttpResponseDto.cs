namespace PolishNewsHarvesterCommon.Dto
{
    public class HttpResponseDto
    {
        public string Url { get; init; }
        public string Body { get; init; }
        public int StatusCode { get; init; }
        public string ResponseHeaders { get; init; }

        public HttpResponseDto(string url, byte[] body, int statusCode, string responseHeaders)
        {
            Url = url;
            Body = System.Text.Encoding.UTF8.GetString(body, 0, body.Length);
            StatusCode = statusCode;
            ResponseHeaders = responseHeaders;
        }

    }
}