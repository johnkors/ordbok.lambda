using System.Threading.Tasks;

namespace OrdbokApi.Lib
{
    public interface IOrdbokOverride
    {
        string Phrase { get; }
        Task<OrdbokResponse> GetResponseOverrideAsync();
    }
}