using System.Collections.Generic;

namespace OrdbokApi.Lib.Models
{
    /// <summary>
    /// En artikkel. Kan ha en eller flere tydinger av ord/uttrykket.
    /// </summary>
    public class OrdbokArticle
    {

        public OrdbokArticle()
        {
            Tydinger = new List<Tyding>();
        }

        /// <summary>
        /// En forklaring på oppslagsordet. Tilgjengelig dersom oppslagsordet ikke har tydinger.
        /// </summary>
        public string Forklaring { get; set; }

        /// <summary>
        /// En tyding av oppslagsord.
        /// </summary>
        public List<Tyding> Tydinger { get; set; }

        /// <summary>
        /// Oppslagsordet er likt søkefrasen ved direktetreff eller et relatert ord til frasen dersom man ikke fikk direktetreff.
        /// </summary>
        public string Oppslagsord { get; set; }
    }
}