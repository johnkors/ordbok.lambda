using System;
using System.Collections.Generic;
using OrdbokApi.Lib.Models;

namespace OrdbokApi.Lib
{
    /// <summary>
    /// En liste med artikler om frasen, samt en lenke til ordbok.uib.no for å se dataene i HTML-form.
    /// </summary>
    public class OrdbokResponse
    {
        public OrdbokResponse(IEnumerable<OrdbokArticle> articles, Uri link)
        {
            Artikler = articles ?? new List<OrdbokArticle>();
            Link = link;
        }
        public Uri Link { get; set; }
        public IEnumerable<OrdbokArticle> Artikler { get; set; }

        public static OrdbokResponse Empty()
        {
            return new OrdbokResponse(new List<OrdbokArticle>(), null);
        }
    }
}