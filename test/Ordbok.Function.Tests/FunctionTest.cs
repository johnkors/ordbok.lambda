using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Xunit;
using Amazon.Lambda.TestUtilities;
using Ordbok.Function;

namespace Orderbok.Function.Tests
{
    public class FunctionTest
    {
        [Fact]
        public async Task TestGetMethod()
        {
            var functions = new GetPhraseFunction();
           
            var context = new TestLambdaContext();

            var slackBody = "user_name=Steve&text=Test";
            var data = new APIGatewayProxyRequest
            {
                Body = slackBody
            };
            var response = await functions.Get(data, context);
            Assert.Equal("{\"text\":\"", response.Body.Substring(0,9));
        }
    }
}
