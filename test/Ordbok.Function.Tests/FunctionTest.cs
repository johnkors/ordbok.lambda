using System.Collections.Generic;
using Amazon.Lambda.APIGatewayEvents;
using Xunit;
using Amazon.Lambda.TestUtilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Ordbok.Function;

namespace Orderbok.Function.Tests
{
    public class FunctionTest
    {
        [Fact]
        public void TestGetMethod()
        {
            var functions = new GetPhraseFunction();
           
            var context = new TestLambdaContext();

            string slackBody = "user_name=Steve&text=Test";
            var data = new APIGatewayProxyRequest
            {
                Body = slackBody
            };
            var response = functions.Get(data, context);
            Assert.Equal("{\"text\":\"", response.Body.Substring(0,9));
        }
    }
}
