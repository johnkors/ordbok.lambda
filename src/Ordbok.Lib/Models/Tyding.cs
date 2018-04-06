using System.Collections.Generic;
using System.Linq;

namespace OrdbokApi.Lib.Models
{
    public class Tyding
    {
        public string Nummer { get; set; }
        public string Tekst{ get; set; }
        public IEnumerable<string> Eksempler { get; set; }

        public Tyding(string nummer, string tekst, IEnumerable<string> eksempler)
        {
            Nummer = nummer;
            Tekst = tekst;
            Eksempler = eksempler ?? new List<string>() ;
        }

        public override string ToString()
        {
            return $"{Nummer}. {GetTekstAndEksempler()}";
        }

        protected bool Equals(Tyding other)
        {
            return string.Equals(Nummer, other.Nummer) && string.Equals(Tekst, other.Tekst);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Tyding) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Nummer != null ? Nummer.GetHashCode() : 0) * 397) ^ (Tekst != null ? Tekst.GetHashCode() : 0);
            }
        }

        public string GetTekstAndEksempler()
        {
            if (Eksempler.Any())
            {
                return $"{ Tekst}. Eks: {string.Join(", ", Eksempler)}.";
            }
            return Tekst;
        }
    }
}