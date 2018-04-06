using System.Threading.Tasks;

namespace OrdbokApi.Lib
{
    public interface IOrdbokService
    {
        Task<OrdbokResponse> GetOrdbokResponse(string phrase);
    }
}