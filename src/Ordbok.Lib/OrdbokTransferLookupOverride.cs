using System;
using System.Threading.Tasks;

namespace OrdbokApi.Lib
{
    public class OrdbokTransferLookupOverride :  IOrdbokOverride
    {
        private readonly string _lookupThisPhraseInstead;
        private Func<string, Task<OrdbokResponse>> _fetcher;

        public OrdbokTransferLookupOverride(string phrase, string lookupThisPhraseInstead, Func<string, Task<OrdbokResponse>> fetcher)
        {
            _lookupThisPhraseInstead = lookupThisPhraseInstead;
            Phrase = phrase;
            _fetcher = fetcher;
        }
        
        public Task<OrdbokResponse> GetResponseOverrideAsync()
        {
            return _fetcher(_lookupThisPhraseInstead);
        }

        public string Phrase { get; }
    }
}