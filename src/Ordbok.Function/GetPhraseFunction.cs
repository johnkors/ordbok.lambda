using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebHooks;
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
 
        public APIGatewayProxyResponse Get(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var data = HttpUtility.ParseQueryString(request.Body);
            var username = data["user_name"];
            var text = data["text"];
  
            context.Logger.LogLine("Request: " + JsonConvert.SerializeObject(request));
            context.Logger.LogLine("Keys: " + JsonConvert.SerializeObject(data.AllKeys));
            context.Logger.LogLine("data: " + JsonConvert.SerializeObject(data));
            context.Logger.LogLine("U: " + username);
            context.Logger.LogLine("text: " + text);
            var slackSlashCommandUserInput = new SlackSlashCommandUserInput
            {
                Username = username,
                Phrase = text
            };
            var ordbok = _slackService.GenerateSlackWebHookResponse(slackSlashCommandUserInput).GetAwaiter().GetResult();
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
