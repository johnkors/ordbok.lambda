using System;
using System.Net.Http;

namespace OrdbokApi.Lib
{
    public class NonSuccessfulHttpQueryAgainstUiBException : Exception
    {
        public HttpResponseMessage HttpRes { get; }

        public NonSuccessfulHttpQueryAgainstUiBException(HttpResponseMessage httpRes)
        {
            HttpRes = httpRes;
        }
    }
}