using System.Collections.Generic;
using System.Net;
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
        private SlackWebHookService _slackService;

        /// <summary>
        /// Default constructor that Lambda will invoke.
        /// </summary>
        public GetPhraseFunction()
        {
            IOrdbokService ordbokService = new OrdbokService();
            _slackService = new SlackWebHookService(ordbokService);
        }


        /// <summary>
        /// A Lambda function to respond to HTTP Get methods from API Gateway
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The list of blogs</returns>
        public APIGatewayProxyResponse Get(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine("Get Request\n");

            var slackSlashCommandUserInput = new SlackSlashCommandUserInput
            {
                Phrase = "test",
                Username = "yolo"
            };
            var generateSlackWebHookResponse = _slackService.GenerateSlackWebHookResponse(slackSlashCommandUserInput).GetAwaiter().GetResult();
            var responseJson = JsonConvert.SerializeObject(generateSlackWebHookResponse);
            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = responseJson,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };

            return response;
        }
    }
}
