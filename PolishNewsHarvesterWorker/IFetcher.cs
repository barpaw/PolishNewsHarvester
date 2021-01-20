using System.Threading.Tasks;
using PolishNewsHarvesterSdk.Dto;

namespace PolishNewsHarvesterWorker
{
    public interface IFetcher
    {
        Task<HttpResponseDto> FetchUrl(string url);
    }
}