using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using NScrape;
using OrdbokApi.Lib.Models;

namespace OrdbokApi.Lib
{
    public class OrdbokHtmlScraper : Scraper
    {
        public OrdbokHtmlScraper(string html) : base(html)
        {
        }

        public List<OrdbokArticle> GetArticles()
        {
            var tables = HtmlDocument.DocumentNode.Descendants("table");
            var table = tables.FirstOrDefault(n => n.HasAttributes && n.Attributes.Contains("id") && n.Attributes["id"].Value == "byttutBM");

            var articles = new List<OrdbokArticle>();
            if (table != null)
            {
                var rows = table.Descendants("tr").Skip(1); // skip header row
                foreach (var rowNode in rows)
                {
                    var article = new OrdbokArticle
                    {
                        Oppslagsord = GetOppslagsord(rowNode),
                        Tydinger = GetTydinger(rowNode)
                    };

                    article.Forklaring = !article.Tydinger.Any() ? GetForklaring(rowNode) : article.Oppslagsord;
                    articles.Add(article);
                }
            }
            return articles;
        }

        private string GetForklaring(HtmlNode rowNode)
        {
            var st = AllWithCssClass(rowNode, "tydingC kompakt").ToArray();
            var allInner = st[0].ChildNodes.Where(c => c.Name == "#text" || (c.Name == "span" && c.HasAttributes && c.Attributes["class"].Value.Contains("henvisning"))).Select(c => c.InnerText.Replace("  ", " ").Trim()).Distinct();
            return string.Join(" ", allInner);

            var utvidet = AllWithCssClass(rowNode, "utvidet");
            var forklaring = "";
            if (utvidet != null)
            {
                var utvitedNodes = utvidet.ToArray();
                if (utvitedNodes.Length > 1)
                {
                    forklaring = utvitedNodes[1].InnerText.Replace("  ", " ").Trim();
                }
            }
            return forklaring;
        }

        private string GetOppslagsord(HtmlNode rowNode)
        {
            var oppslagdiv = AllWithCssClass(rowNode, "tiptip").FirstOrDefault();
            var oppslagsord = "";
            if (oppslagdiv != null)
            {
                oppslagsord = oppslagdiv.InnerText;
            }
            return oppslagsord;
        }

        private List<Tyding> GetTydinger(HtmlNode rowNode)
        {
            var nodesWithTydingClass = AllWithCssClass(rowNode, "tyding utvidet");
            var tydinger = new List<Tyding>();
            foreach (var htmlNode in nodesWithTydingClass)
            {
                var def = GetTyding(htmlNode);

                int dontCare;
                if (int.TryParse(def.Nummer, out dontCare))
                {
                    if (!tydinger.Any(d => d.Equals(def))) // dups
                    {
                        tydinger.Add(def);
                    }
                }
            }
            return tydinger;
        }

        private Tyding GetTyding(HtmlNode htmlNode)
        {
            var spans = htmlNode.ChildNodes.Where(n => n.Name == "span").ToArray();
            var texts = htmlNode.ChildNodes.Where(n => n.Name == "#text").ToArray();
            var num = spans.Length >= 1 ? spans[0].InnerText : "[x]";
            var definition = texts.Length >= 2? texts[1].InnerText.Trim().Replace("  ", " ") : "[---]";
            var henvisning = AllWithCssClass(htmlNode, "henvisning").ToList();
            if (henvisning.Any())
            {
                definition = definition + " " + string.Join(", ", henvisning.Select(h => h.InnerText).Distinct());
            }
            definition = definition.TrimEnd(',').TrimStart(',').Trim();


            var eksempler = GetEksempler(htmlNode);
            

            return new Tyding(num, definition, eksempler);
        }

        private IEnumerable<string> GetEksempler(HtmlNode htmlNode)
        {
            var doeme = AllWithCssClass(htmlNode, "doeme utvidet");
            var firstSpanUnderDoeme = doeme.Select(n => n.ChildNodes.FirstOrDefault(c => c.Name == "span"));
            var spanTextUnderDoeme = firstSpanUnderDoeme.Select(n => n.InnerText).ToList();
            return spanTextUnderDoeme.Distinct();
        }

        private IEnumerable<HtmlNode> AllWithCssClass(HtmlNode node, string cssClass)
        {
            return node.Descendants().Where(n => n.Attributes.Contains("class") && n.Attributes["class"].Value.Contains(cssClass));
        }
    }
}