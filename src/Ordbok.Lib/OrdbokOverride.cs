using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrdbokApi.Lib.Models;

namespace OrdbokApi.Lib
{
    public class OrdbokOverride : IOrdbokOverride
    {
        public string Phrase { get; }
        private readonly string _mainResponse;
        private readonly string _definition;

        public OrdbokOverride(string phrase, string mainResponse, string definition)
        {
            Phrase = phrase;
            _mainResponse = mainResponse;
            _definition = definition;
        }

        public Task<OrdbokResponse> GetResponseOverrideAsync()
        {
            var article = new OrdbokArticle()
            {
                Forklaring = _mainResponse,
            };
            article.Tydinger.Add(new Tyding("1", _definition, new string[]{}));
            var rickRoll = new Uri("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
            return Task.FromResult(new OrdbokResponse(new List<OrdbokArticle> { article}, link: rickRoll));
        }
    }
}