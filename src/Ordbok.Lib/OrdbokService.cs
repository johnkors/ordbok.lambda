using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OrdbokApi.Lib
{
    public class OrdbokService : IOrdbokService
    {
        private readonly List<IOrdbokOverride> _overrides;
        private static readonly HttpClient HttpClient = new HttpClient() { Timeout = new TimeSpan(0, 0, 0, 10) };

        public OrdbokService()
        {
            _overrides = new List<IOrdbokOverride>
            {
                new OrdbokOverride("jarle", "jarle m1(norr.jarle)", "Usedvanlig norrønt oppstikksvekkende vesen."),
                new OrdbokTransferLookupOverride("jorg", "god morgen", HttpGet),
                new OrdbokTransferLookupOverride("storsdag", "øl", HttpGet),
            };
        }

        public async Task<OrdbokResponse> GetOrdbokResponse(string phrase)
        {

            if (string.IsNullOrEmpty(phrase))
            {
                return OrdbokResponse.Empty();
            }

            if (_overrides.Any(o => o.Phrase.Is(phrase)))
            {
                var @override = _overrides.First(o => o.Phrase.Is(phrase));
                return await @override.GetResponseOverrideAsync();
            }

            return await HttpGet(phrase);
        }

        private static async Task<OrdbokResponse> HttpGet(string phrase)
        {
            var dokproUriTemplate = "http://ordbok.uib.no/perl/ordbok.cgi?OPP={0}&ant_bokmaal=1&bokmaal=+&ordbok=bokmaal&ava=ava&type=bare_oppslag&soeketype=v";
            var url = new Uri(string.Format(dokproUriTemplate, phrase));
            var httpRes = await HttpClient.GetAsync(url);

            if (httpRes.IsSuccessStatusCode)
            {
                var html = await httpRes.Content.ReadAsStringAsync();
                var scraper = new OrdbokHtmlScraper(html);
                return new OrdbokResponse(scraper.GetArticles(), url);
            }

            throw new NonSuccessfulHttpQueryAgainstUiBException(httpRes);
        }
    }
}