using System;

namespace OrdbokApi.Lib
{
    public static class StringExtensions
    {
        public static bool Is(this string a, string b)
        {
            if(a != null)
                return a.Equals(b, StringComparison.InvariantCultureIgnoreCase);
            return b == null;
        }
    }
}