using Xunit;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using Ordbok.Function;
using OrdbokApi.Lib.Slack;
using Xunit.Abstractions;

namespace Orderbok.Function.Tests
{
    public class FunctionTest
    {
        private readonly ITestOutputHelper _helper;

        public FunctionTest(ITestOutputHelper helper)
        {
            _helper = helper;
        }

        [Fact]
        public void TestGetMethod()
        {
            var functions = new GetPhraseFunction();
            
            var slackData = new SlackSlashCommandUserInput
            {
                Phrase = "test", Username = "@john"
            };

            var context = new TestLambdaContext();
            var response = functions.Get(slackData, context);
            Assert.Equal(200, response.StatusCode);
            _helper.WriteLine(response.Body);
            Assert.Equal("{\"text\":", response.Body.Substring(0,8));
        }


    }
}
