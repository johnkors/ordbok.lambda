using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using OrdbokApi.Lib;
using OrdbokApi.Lib.Slack;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace Ordbok.Function
{
    public class GetPhraseFunction
    {
        private readonly SlackWebHookService _slackService;

        /// <summary>
        /// Default constructor that Lambda will invoke.
        /// </summary>
        public GetPhraseFunction()
        {
            _slackService = new SlackWebHookService(new OrdbokService());
        }
 
        public async Task<APIGatewayProxyResponse> Get(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var data = HttpUtility.ParseQueryString(request.Body);
            var username = data["user_name"];
            var text = data["text"];
  
            var slackSlashCommandUserInput = new SlackSlashCommandUserInput
            {
                Username = username,
                Phrase = text
            };
            var ordbok = await _slackService.GenerateSlackWebHookResponse(slackSlashCommandUserInput);

            return new APIGatewayProxyResponse()
            {
                Body = JsonConvert.SerializeObject(ordbok),
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json"}
                },
                StatusCode = 200
            };
        }
    }
}
