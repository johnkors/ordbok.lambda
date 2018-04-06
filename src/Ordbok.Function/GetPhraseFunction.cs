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
            _slackService = new SlackWebHookService(new OrdbokService());
        }


        /// <summary>
        /// A Lambda function to respond to HTTP Get methods from API Gateway
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The list of blogs</returns>
        public APIGatewayProxyResponse Get(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine("Get Request\n for " + JsonConvert.SerializeObject(request));

            var slackInput = JsonConvert.DeserializeObject<SlackSlashCommandUserInput>(request.Body);
            var generateSlackWebHookResponse = _slackService.GenerateSlackWebHookResponse(slackInput).GetAwaiter().GetResult();
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
